# Copies .\dist into the game's mods folder.
# Usage:  powershell -ExecutionPolicy Bypass -File install.ps1
param(
    [string]$GameDir = "C:\Steam\steamapps\common\Slay the Spire 2"
)

$ErrorActionPreference = "Stop"
Set-Location $PSScriptRoot

if (-not (Test-Path dist\IOUMod.dll)) {
    Write-Error "dist\IOUMod.dll not found. Run build.ps1 first."
}

$target = Join-Path $GameDir "mods\IOUMod"
New-Item -ItemType Directory -Force $target | Out-Null
Copy-Item dist\IOUMod.* $target -Force

Write-Host "Installed to $target"
