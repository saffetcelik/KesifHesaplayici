@echo off
chcp 65001 >nul
title 🚀 Keşif Metni Oluşturucu - Release Builder

echo.
echo ╔══════════════════════════════════════════════════════════════╗
echo ║                🚀 Release Builder                            ║
echo ║              Keşif Metni Oluşturucu                         ║
echo ╚══════════════════════════════════════════════════════════════╝
echo.

set version=1.0.0
echo 📦 Sürüm numarası: %version%

echo.
echo 🔄 Release oluşturuluyor...
echo.

powershell -ExecutionPolicy Bypass -File "scripts/build-release.ps1" -Version "%version%"

if %ERRORLEVEL% EQU 0 (
    echo.
    echo ✅ Release başarıyla oluşturuldu!
    echo 📁 releases/ klasörünü kontrol edin
    echo.
    echo 🚀 GitHub'a yüklemek için:
    echo    git tag v%version%
    echo    git push origin v%version%
    echo.
) else (
    echo.
    echo ❌ Release oluşturma başarısız!
    echo.
)

pause
