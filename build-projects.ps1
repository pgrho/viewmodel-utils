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
    $sln = "${PSScriptRoot}/src/Shipwreck.ViewModelUtils.sln"
    
    Write-Host "Cleaning $sln"
    $p = Start-Process $devenv.FullName -ArgumentList "`"$sln`" /Clean Release" -Wait -PassThru
    Write-Host "Cleaning $sln Exited ${p.ExitCode}"
    
    Write-Host "Building $sln"
    $p = Start-Process $devenv.FullName -ArgumentList "`"$sln`" /Build Release" -Wait -PassThru
    Write-Host "Building $sln Exited ${p.ExitCode}"
    
    if ($nugetSource -ne "" && $nugetApiKey -ne "") {
      
        $xd = [System.Xml.Linq.XDocument]::Load("$PSScriptRoot\src\Directory.build.props")
        $ver = $xd.Root.Element("PropertyGroup").Element("Version").Value

        Write-Host "Pushing pkgs of $ver"

        dotnet nuget push "$PSScriptRoot\src\*\*\bin\Release\Shipwreck.ViewModelUtils.*.$ver.nupkg" -s $nugetSource -k $nugetApiKey --skip-duplicate
    }
}