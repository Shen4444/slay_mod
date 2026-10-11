# Builds the mod into .\dist (IOUMod.dll + IOUMod.pck + IOUMod.json).
# Usage:  powershell -ExecutionPolicy Bypass -File build.ps1
param(
    # Use the *_console.exe from the Godot zip so output shows in this window
    [string]$GodotExe = "D:\Godot_4.5.1_mono\Godot_v4.5.1-stable_mono_win64_console.exe"
)

$ErrorActionPreference = "Stop"
Set-Location $PSScriptRoot

if (-not (Test-Path $GodotExe)) {
    Write-Error "Godot not found at '$GodotExe'. Edit the path at the top of build.ps1 or pass -GodotExe <path>."
}

# Import resources (needed once after cloning) and compile the C# code
& $GodotExe --headless --import
& $GodotExe --headless --build-solutions --quit
if ($LASTEXITCODE -ne 0) { Write-Error "C# build failed" }

if (Test-Path dist) { Remove-Item -Recurse -Force dist }
New-Item -ItemType Directory dist | Out-Null

Copy-Item ".godot\mono\temp\bin\Debug\IOUMod.dll" dist\
& $GodotExe --headless --export-pack "Windows Desktop" dist\IOUMod.pck
if ($LASTEXITCODE -ne 0) { Write-Error "PCK export failed" }
Copy-Item IOUMod.json dist\

Write-Host "Build finished: $(Resolve-Path dist)"
