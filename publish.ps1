# Uploads the built mod (.\dist) to the Steam Workshop with Mega Crit's ModUploader.
# Usage:  powershell -ExecutionPolicy Bypass -File publish.ps1
# Run build.ps1 first. Steam must be running and logged in.
param(
    # Path to ModUploader.exe. If omitted, it is read from uploader_path.txt
    # (one line, not committed), like godot_path.txt.
    [string]$UploaderExe
)

$ErrorActionPreference = "Stop"
Set-Location $PSScriptRoot

if (-not $UploaderExe -and (Test-Path uploader_path.txt)) {
    $UploaderExe = (Get-Content uploader_path.txt -First 1).Trim().Trim('"')
}
if (-not $UploaderExe -or -not (Test-Path $UploaderExe)) {
    Write-Error "ModUploader.exe not found at '$UploaderExe'. Put its path in uploader_path.txt or pass -UploaderExe <path>."
}
if (-not (Test-Path dist\IOUMod.dll)) {
    Write-Error "dist\IOUMod.dll not found. Run build.ps1 first."
}

# The workshop item contains a folder named after the mod, holding its files
$content = "workshop\content"
if (Test-Path $content) { Remove-Item -Recurse -Force $content }
New-Item -ItemType Directory "$content\IOUMod" | Out-Null
Copy-Item dist\IOUMod.* "$content\IOUMod\"

$workspace = Resolve-Path workshop
# The uploader looks for its Steam files next to itself
Push-Location (Split-Path $UploaderExe)
try {
    & $UploaderExe upload -w $workspace
    if ($LASTEXITCODE -ne 0) { Write-Error "Upload failed; see mod-uploader.log next to ModUploader.exe" }
}
finally {
    Pop-Location
}

Write-Host "Upload finished. workshop\mod_id.txt now holds the Workshop item ID; commit it so updates go to the same item."
