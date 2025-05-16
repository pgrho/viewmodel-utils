using System.IO;
using System.IO.Compression;
using System.Xml.Linq;
using Microsoft.Extensions.Configuration;

namespace Shipwreck.ViewModelUtils.Build;

internal static class Program
{
    private class Parameter
    {
        public bool Clean { get; set; } = false;
        public bool Build { get; set; } = true;
        public string NugetSource { get; set; }
        public string NugetApiKey { get; set; }
    }

    public static async Task<int> Main(string[] args)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        var param = new ConfigurationBuilder().AddCommandLine(args).Build().Get<Parameter>() ?? new Parameter();

        var solDir = GetSolutionDirectory();

        Console.WriteLine("Solution Directory: {0}", solDir);

        var version = XDocument.Load(Path.Combine(solDir, "Directory.build.props")).Root.Element("PropertyGroup").Element("Version").Value;

        if (string.IsNullOrEmpty(version))
        {
            return 1;
        }

        Console.WriteLine("Package Version: {0}", version);
        Console.WriteLine("NuGet Source: {0}", param?.NugetSource);

        var csprojs = Directory.GetFiles(solDir, "*.csproj", SearchOption.AllDirectories)
                        .Where(e => e.IndexOf(".Demo.", StringComparison.InvariantCultureIgnoreCase) < 0
                                    && e.IndexOf(".Tests.", StringComparison.InvariantCultureIgnoreCase) < 0
                                    && e.IndexOf(".Build.", StringComparison.InvariantCultureIgnoreCase) < 0)
                        .OrderBy(e => e.IndexOf(".Core.", StringComparison.InvariantCultureIgnoreCase) >= 0 ? 0 : 1)
                        .ThenBy(e => e.IndexOf(".Models.", StringComparison.InvariantCultureIgnoreCase) >= 0 ? 0
                                    : e.IndexOf(".Client.", StringComparison.InvariantCultureIgnoreCase) >= 0 ? 1 : 2)
                        .ToList();

        var dotnetProjs = csprojs.Where(e => e.IndexOf("XamarinForms", StringComparison.InvariantCultureIgnoreCase) < 0).ToList();

        if (param.Clean)
        {
            foreach (var proj in dotnetProjs)
            {
                Console.WriteLine("Cleaning {0}", proj);
                var pc = await ExecuteAsync(new ProcessStartInfo("dotnet.exe")
                {
                    ArgumentList = { "clean", proj, "-c", "Release", "-v", "n" }
                }, encoding: Encoding.UTF8);
                if (pc.ExitCode != 0)
                {
                    return 1;
                }
            }
        }

        if (param.Build)
        {
            var devenv = new FileInfo("devenv.com");
            if (!devenv.Exists)
            {
                var regex = new Regex("^productPath:(.+)$");

                using var p = await ExecuteAsync(
                    new ProcessStartInfo(Environment.ExpandEnvironmentVariables(@"%PROGRAMFILES(X86)%\Microsoft Visual Studio\Installer\vswhere.exe"))
                    {
                        Arguments = "-latest -prerelease -version [17.0,18.0)"
                    }, stdout: e =>
                    {
                        var m = regex.Match(e);
                        if (m.Success)
                        {
                            devenv = new FileInfo(Path.Combine(Path.GetDirectoryName(m.Groups[1].Value.Trim()), "devenv.com"));
                        }
                    });

                if (p.ExitCode != 0 || !devenv.Exists)
                {
                    return 1;
                }
            }

            foreach (var proj in dotnetProjs)
            {
                Console.WriteLine("Building {0}", proj);
                var pc = await ExecuteAsync(new ProcessStartInfo("dotnet.exe")
                {
                    ArgumentList = { "build", proj, "-c", "Release", "-v", "n" }
                });
                if (pc.ExitCode != 0
                    || !ValidatePackage(proj, version))
                {
                    return 1;
                }

                if (Directory.Exists(Path.Combine(Path.GetDirectoryName(proj), "wwwroot")))
                {
                    Console.WriteLine("Building {0} (2)", proj);
                    var pc2 = await ExecuteAsync(new ProcessStartInfo("dotnet.exe")
                    {
                        ArgumentList = { "build", proj, "-c", "Release", "-v", "n" }
                    });
                    if (pc2.ExitCode != 0
                        || !ValidatePackage(proj, version))
                    {
                        return 1;
                    }
                }
            }
            foreach (var proj in csprojs.Except(dotnetProjs))
            {
                Console.WriteLine("Building {0}", proj);
                var pc = await ExecuteAsync(new ProcessStartInfo(devenv.FullName)
                {
                    ArgumentList = { Path.Combine(solDir, "Shipwreck.ViewModelUtils.slnx"), "/Build", "Release", "/Project", proj }
                }, encoding: Encoding.GetEncoding(932));
                if (pc.ExitCode != 0
                    || !ValidatePackage(proj, version))
                {
                    return 1;
                }
            }
        }

        if (!string.IsNullOrEmpty(param.NugetSource)
            || !string.IsNullOrEmpty(param.NugetApiKey))
        {
            var p = await ExecuteAsync(new ProcessStartInfo("dotnet.exe")
            {
                ArgumentList =
                    {
                        "nuget",
                        "push",
                        Path.Combine(solDir, "*", "*", "bin","Release", "Shipwreck.ViewModelUtils.*."+version+".nupkg"),
                        "-s",
                        param.NugetSource,
                        "-k",
                        param.NugetApiKey,
                        "--skip-duplicate"
                    }
            });

            if (p.ExitCode != 0)
            {
                return 1;
            }
        }
        return 0;
    }

    private static string GetSolutionDirectory([CallerFilePath] string fileName = null)
        => Path.GetDirectoryName(Path.GetDirectoryName(fileName));

    private static Task<Process> ExecuteAsync(ProcessStartInfo info, Action<string> stdout = null, Action<string> stderr = null, Encoding encoding = null)
    {
        var tcs = new TaskCompletionSource<Process>();

        try
        {
            info.UseShellExecute = false;
            info.RedirectStandardOutput = true;
            info.RedirectStandardError = true;
            info.CreateNoWindow = true;

            if (encoding != null)
            {
                info.StandardOutputEncoding = encoding;
                info.StandardErrorEncoding = encoding;
            }

            var p = new Process()
            {
                StartInfo = info,
                EnableRaisingEvents = true,
            };

            Task<string> stdoutTask = null;
            Func<Task<string>, string> stdoutHandler = null;
            Task<string> stderrTask = null;
            Func<Task<string>, string> stderrHandler = null;
            stdoutHandler = t =>
            {
                if (t.IsCompleted && t.Result != null)
                {
                    if (!string.IsNullOrEmpty(t.Result))
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($"[{p.Id}]{t.Result}");
                        Console.ResetColor();
                        stdout?.Invoke(t.Result);
                    }
                    stdoutTask = p.StandardOutput.ReadLineAsync().ContinueWith(stdoutHandler);
                }

                return t.Result;
            };
            stderrHandler = t =>
            {
                if (t.IsCompleted && t.Result != null)
                {
                    if (!string.IsNullOrEmpty(t.Result))
                    {
                        Console.Error?.WriteLine($"[{p.Id}]{t.Result}");
                        stderr?.Invoke(t.Result);
                    }
                    stderrTask = p.StandardError.ReadLineAsync().ContinueWith(stderrHandler);
                }

                return t.Result;
            };

            p.Exited += async (s, e) =>
            {
                while (stdoutTask?.Status < TaskStatus.RanToCompletion
                        || stderrTask?.Status < TaskStatus.RanToCompletion)
                {
                    await Task.WhenAny(new[] { stdoutTask, stderrTask }.Where(e => e != null));
                    if (stdoutTask?.Status >= TaskStatus.RanToCompletion)
                    {
                        stdoutTask = null;
                    }
                    if (stderrTask?.Status >= TaskStatus.RanToCompletion)
                    {
                        stderrTask = null;
                    }
                }
                Console.WriteLine("Exited {0} with {1}", p.StartInfo.FileName, p.ExitCode);
                tcs.TrySetResult(p);
            };

            Console.WriteLine("Starting {0}", p.StartInfo.FileName);
            p.Start();
            stdoutTask = p.StandardOutput.ReadLineAsync().ContinueWith(stdoutHandler);
            stderrTask = p.StandardError.ReadLineAsync().ContinueWith(stderrHandler);
        }
        catch (Exception ex)
        {
            tcs.TrySetException(ex);
        }

        return tcs.Task;
    }

    private static bool ValidatePackage(string proj, string version)
    {
        var pn = Path.GetFileNameWithoutExtension(proj);
        var nupkg = Path.Combine(Path.GetDirectoryName(proj), "bin", "Release", pn + "." + version + ".nupkg");
        if (!File.Exists(nupkg))
        {
            Console.Error?.WriteLine("NuGet package {0} not found.", nupkg);
            return false;
        }

        using (var fs = new FileStream(nupkg, FileMode.Open, FileAccess.Read))
        using (var za = new ZipArchive(fs, ZipArchiveMode.Read))
        {
            var xmlns = "http://schemas.microsoft.com/packaging/2013/05/nuspec.xsd";

            // validate dependency version
            var ns = za.GetEntry(pn + ".nuspec");
            using (var es = ns.Open())
            {
                var xd = XDocument.Load(es);
                foreach (var dep in xd.Root.Elements(XName.Get("metadata", xmlns))
                                        .Elements(XName.Get("dependencies", xmlns))
                                        .Elements(XName.Get("group", xmlns))
                                        .Elements(XName.Get("dependency", xmlns)))
                {
                    var id = dep.Attribute("id")?.Value;
                    var v = dep.Attribute("version")?.Value;
                    if (id?.StartsWith("Shipwreck.ViewModelUtils") == true && v != version)
                    {
                        Console.Error?.WriteLine("Found invalid version: {0} vs {1}", version, v);
                        return false;
                    }
                }
            }

            // validate staticwebassets
            var swa = za.GetEntry("build/Microsoft.AspNetCore.StaticWebAssets.props");

            if (swa != null)
            {
                using (var es = swa.Open())
                {
                    var xd = XDocument.Load(es);
                    foreach (var asset in xd.Root.Elements("ItemGroup").Elements("StaticWebAsset").Elements("RelativePath"))
                    {
                        var name = asset.Value;
                        if (!string.IsNullOrEmpty(name))
                        {
                            if (za.GetEntry("staticwebassets/" + name) == null)
                            {
                                Console.Error?.WriteLine("Found invalid staticwebassets: {0}", name);
                                return false;
                            }
                        }
                    }
                }
            }
        }

        return true;
    }
}
