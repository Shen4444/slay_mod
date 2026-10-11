# Builds the mod into .\dist (IOUMod.dll + IOUMod.pck + IOUMod.json).
# Usage:  powershell -ExecutionPolicy Bypass -File build.ps1
param(
    # Path to Godot's *_console.exe. If omitted, it is read from godot_path.txt
    # (one line, not committed) so each machine can keep its own path.
    [string]$GodotExe
)

$ErrorActionPreference = "Stop"
Set-Location $PSScriptRoot

if (-not $GodotExe -and (Test-Path godot_path.txt)) {
    $GodotExe = (Get-Content godot_path.txt -First 1).Trim().Trim('"')
}
if (-not $GodotExe -or -not (Test-Path $GodotExe)) {
    Write-Error "Godot not found at '$GodotExe'. Put the path to Godot's _console.exe in godot_path.txt or pass -GodotExe <path>."
}

# Import resources (needed once after cloning)
& $GodotExe --headless --import

# Compile the C# code. dotnet is used directly because Godot's
# --build-solutions can leave the headless editor running after the build.
dotnet build IOUMod.csproj -c Debug
if ($LASTEXITCODE -ne 0) { Write-Error "C# build failed" }

if (Test-Path dist) { Remove-Item -Recurse -Force dist }
New-Item -ItemType Directory dist | Out-Null

Copy-Item ".godot\mono\temp\bin\Debug\IOUMod.dll" dist\
& $GodotExe --headless --export-pack "Windows Desktop" dist\IOUMod.pck
if ($LASTEXITCODE -ne 0) { Write-Error "PCK export failed" }
Copy-Item IOUMod.json dist\

Write-Host "Build finished: $(Resolve-Path dist)"
