param(
    [string]$nugetSource = "",
    [string]$nugetApiKey = ""
) 
dotnet run --project "$PSScriptRoot\src\Build\Shipwreck.ViewModelUtils.Build.csproj" -- "/NuGetSource=$nugetSource" "/NuGetApiKey=$nugetApiKey"