param(
    [string]$nugetSource = "",
    [string]$nugetApiKey = ""
)
$devenv = New-Object System.IO.FileInfo -ArgumentList "devenv.exe"

if (!$devenv.Exists) {
    $psi = New-Object System.Diagnostics.ProcessStartInfo
    $psi.FileName = "${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe"
    $psi.RedirectStandardOutput = $true
    $psi.Arguments = "-latest -prerelease"
    $p = New-Object System.Diagnostics.Process
    $p.StartInfo = $psi
    $p.Start()
    $p.WaitForExit()
  
    $pp = $p.StandardOutput.ReadToEnd().Split([Environment]::NewLine) | Select-String "productPath:"
    $devenv = New-Object System.IO.FileInfo -ArgumentList $pp.ToString().Substring(13)
}

if ($devenv.Exists) {
    
    $projs = Get-ChildItem "${PSScriptRoot}\src\*\*\*.csproj"
    
    foreach ($fi in $projs) {
        $proj = $fi.FullName
        if ($proj -notlike "*XamarinForms*") {
            Write-Host "Cleaning $proj"
            $p = Start-Process dotnet -ArgumentList "clean $proj -c Release -v m" -PassThru
            $p.WaitForExit()
            if ($p.ExitCode -ne 0) {
                throw "Failed to clean ${proj}."
            }
        }
    }
    foreach ($fi in $projs) {
        $proj = $fi.FullName
        if ($proj -notlike "*XamarinForms*") {
            Write-Host "Building $proj"
            $p = Start-Process dotnet -ArgumentList "build $proj -c Release -v m" -PassThru
            $p.WaitForExit()
            if ($p.ExitCode -ne 0) {
                throw "Failed to build ${proj}."
            }
        }
    }
    
    $sln = "${PSScriptRoot}\src\Shipwreck.ViewModelUtils.sln"
    foreach ($proj in @("Core\XamarinForms\Shipwreck.ViewModelUtils.Core.XamarinForms.csproj", "Framework\XamarinForms\Shipwreck.ViewModelUtils.XamarinForms.csproj")) {
        Write-Host "Building $proj"
        $p = Start-Process $devenv.FullName -ArgumentList "`"$sln`" /Build Release /Project $proj" -PassThru
        $p.WaitForExit()
        if ($p.ExitCode -ne 0) {
            throw "Failed to build ${proj}."
        }
    }

    if (($nugetSource -ne "") -and ($nugetApiKey -ne "")) {
      
        $xd = [System.Xml.Linq.XDocument]::Load("$PSScriptRoot\src\Directory.build.props")
        $ver = $xd.Root.Element("PropertyGroup").Element("Version").Value

        Write-Host "Pushing pkgs of $ver"

        dotnet nuget push "$PSScriptRoot\src\*\*\bin\Release\Shipwreck.ViewModelUtils.*.$ver.nupkg" -s $nugetSource -k $nugetApiKey --skip-duplicate
    }
}