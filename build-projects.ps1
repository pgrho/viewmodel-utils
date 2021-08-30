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
    $sln = "${PSScriptRoot}\src\Shipwreck.ViewModelUtils.sln"
    
    Write-Host "Cleaning Shipwreck.ViewModelUtils.Models.csproj"
    Start-Process dotnet -ArgumentList "`clean ${PSScriptRoot}/src/Core/Models/Shipwreck.ViewModelUtils.Models.csproj -c Release" -Wait -PassThru

    Write-Host "Building Shipwreck.ViewModelUtils.Client.csproj"
    Start-Process dotnet -ArgumentList "`build ${PSScriptRoot}/src/Core/Client/Shipwreck.ViewModelUtils.Client.csproj -c Release" -Wait -PassThru

    Write-Host "Building Shipwreck.ViewModelUtils.Client.CoreFx.csproj"
    Start-Process dotnet -ArgumentList "`build ${PSScriptRoot}/src/Core/Client.CoreFx/Shipwreck.ViewModelUtils.Client.CoreFx.csproj -c Release" -Wait -PassThru

    Write-Host "Building Shipwreck.ViewModelUtils.Client.Newtonsoft.csproj"
    Start-Process dotnet -ArgumentList "`build ${PSScriptRoot}/src/Core/Client.Newtonsoft/Shipwreck.ViewModelUtils.Client.Newtonsoft.csproj -c Release" -Wait -PassThru
    
    Write-Host "Building Shipwreck.ViewModelUtils.Blazor.csproj"
    Start-Process dotnet -ArgumentList "`build ${PSScriptRoot}/src/Framework/Blazor/Shipwreck.ViewModelUtils.Blazor.csproj -c Release" -Wait -PassThru

    Write-Host "Building Shipwreck.ViewModelUtils.PresentationFramework.csproj"
    Start-Process dotnet -ArgumentList "`build ${PSScriptRoot}/src/Framework/PresentationFramework/Shipwreck.ViewModelUtils.PresentationFramework.csproj -c Release" -Wait -PassThru

    Write-Host "Building Shipwreck.ViewModelUtils.Core.XamarinForms.csproj"
    Start-Process $devenv.FullName -ArgumentList "`"$sln`" /Build Release /Project Core\XamarinForms\Shipwreck.ViewModelUtils.Core.XamarinForms.csproj" -Wait -PassThru

    Write-Host "Building Shipwreck.ViewModelUtils.XamarinForms.csproj"
    Start-Process $devenv.FullName -ArgumentList "`"$sln`" /Build Release /Project Framework\XamarinForms\Shipwreck.ViewModelUtils.XamarinForms.csproj" -Wait -PassThru

    if ($nugetSource -ne "" && $nugetApiKey -ne "") {
      
        $xd = [System.Xml.Linq.XDocument]::Load("$PSScriptRoot\src\Directory.build.props")
        $ver = $xd.Root.Element("PropertyGroup").Element("Version").Value

        Write-Host "Pushing pkgs of $ver"

        dotnet nuget push "$PSScriptRoot\src\*\*\bin\Release\Shipwreck.ViewModelUtils.*.$ver.nupkg" -s $nugetSource -k $nugetApiKey --skip-duplicate
    }
}