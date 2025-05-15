param(
    [string]$projectDir,
    [string]$configuration,
    [string]$toastAssets,
    [string]$typeaheadAssets
)

$enc = [System.Text.Encoding]::UTF8

# gulpfileを実行するかチェック
$srcTime = [System.DateTime]::MinValue
foreach($f in @("package-lock.json", "gulpfile.js")) {
    $t = [System.IO.File]::GetLastWriteTime("${projectDir}${f}")
    if($t -gt $srcTime) {
        $srcTime = $t
    }
}
foreach($gsd in @("./Scripts/")) {
    $dp = $gsd
    if ($dp.StartsWith(".")) {
        $dp = "$projectDir$gsd"
    }
    Write-Host "Enumerating directory: $dp"
    foreach($sf in     [System.IO.Directory]::GetFiles($dp, "*", [System.IO.SearchOption]::AllDirectories)){
        $fi = New-Object -TypeName System.IO.FileInfo -ArgumentList $sf
        if($fi.LastWriteTime -gt $srcTime) {
            $srcTime = $fi.LastWriteTime
        }
    }
}

Write-Host "Source LastWriteTime: $srcTime"

$destTime = [System.DateTime]::MaxValue
foreach($df in @("wwwroot/Shipwreck.ViewModelUtils.Core.Blazor.js")) {
    $fi = New-Object -TypeName System.IO.FileInfo -ArgumentList "$projectDir$df"
    if ($fi.Exists -eq $false){
        $destTime = [System.DateTime]::MinValue
    } elseif($fi.LastWriteTime -lt $destTime) {
        $destTime = $fi.LastWriteTime
    }
}
Write-Host "Destination LastWriteTime: $destTime"

if ($srcTime -lt $destTime) {
    Write-Host "Skipped to run gulp task"
} else {
    cd $projectDir
    cd node_modules\.bin\
    .\gulp.CMD
}