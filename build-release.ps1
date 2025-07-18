# Keşif Metni Oluşturucu - Release Build Script
# PowerShell version

param(
    [string]$Version = "1.0.0",
    [switch]$SkipZip = $false
)

$ErrorActionPreference = "Stop"

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Keşif Metni Oluşturucu Release Build" -ForegroundColor Cyan
Write-Host "Version: $Version" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host

# Proje dizinine git
Set-Location $PSScriptRoot

try {
    # Temizlik
    Write-Host "[1/6] Temizlik yapılıyor..." -ForegroundColor Yellow
    if (Test-Path "KesifUDFGenerator\bin\Release") { Remove-Item "KesifUDFGenerator\bin\Release" -Recurse -Force }
    if (Test-Path "KesifUDFGenerator\obj\Release") { Remove-Item "KesifUDFGenerator\obj\Release" -Recurse -Force }
    if (Test-Path "Release") { Remove-Item "Release" -Recurse -Force }
    Write-Host "Temizlik tamamlandı." -ForegroundColor Green
    Write-Host

    # Restore
    Write-Host "[2/6] NuGet paketleri geri yükleniyor..." -ForegroundColor Yellow
    dotnet restore "KesifUDFGenerator\KesifUDFGenerator.csproj"
    if ($LASTEXITCODE -ne 0) { throw "NuGet restore başarısız!" }
    Write-Host "NuGet restore tamamlandı." -ForegroundColor Green
    Write-Host

    # Build
    Write-Host "[3/6] Proje derleniyor..." -ForegroundColor Yellow
    dotnet build "KesifUDFGenerator\KesifUDFGenerator.csproj" -c Release
    if ($LASTEXITCODE -ne 0) { throw "Build başarısız!" }
    Write-Host "Build tamamlandı." -ForegroundColor Green
    Write-Host

    # Publish - Self-contained executable
    Write-Host "[4/6] Self-contained executable oluşturuluyor..." -ForegroundColor Yellow
    dotnet publish "KesifUDFGenerator\KesifUDFGenerator.csproj" `
        -c Release `
        -r win-x64 `
        --self-contained true `
        -p:PublishSingleFile=true `
        -p:PublishReadyToRun=true `
        -p:IncludeNativeLibrariesForSelfExtract=true `
        -o "Release\win-x64"
    if ($LASTEXITCODE -ne 0) { throw "Publish başarısız!" }
    Write-Host "Self-contained executable oluşturuldu." -ForegroundColor Green
    Write-Host

    # Release klasörünü düzenle
    Write-Host "[5/6] Release dosyaları düzenleniyor..." -ForegroundColor Yellow
    New-Item -ItemType Directory -Path "Release\KesifMetniOlusturucu" -Force | Out-Null

    # Ana executable'ı kopyala
    Copy-Item "Release\win-x64\KesifUDFGenerator.exe" "Release\KesifMetniOlusturucu\KesifMetniOlusturucu.exe"

    # Gerekli dosyaları kopyala
    Copy-Item "README.md" "Release\KesifMetniOlusturucu\"
    Copy-Item "LICENSE" "Release\KesifMetniOlusturucu\"

    # Başlatma script'i oluştur
    $startScript = @"
@echo off
title Keşif Metni Oluşturucu
cd /d "%~dp0"
KesifMetniOlusturucu.exe
"@
    $startScript | Out-File "Release\KesifMetniOlusturucu\Başlat.bat" -Encoding ASCII

    # Kullanım kılavuzu oluştur
    $usageGuide = @"
# Keşif Metni Oluşturucu - Kullanım Kılavuzu

## Kurulum
1. Bu klasörü istediğiniz yere kopyalayın
2. Başlat.bat dosyasına çift tıklayarak uygulamayı başlatın

## Sistem Gereksinimleri
- Windows 10/11 (64-bit)
- .NET Runtime gerekmez (self-contained)

## Özellikler
- Mahkeme keşif kararları için otomatik metin oluşturma
- Bilirkişi ücret hesaplamaları
- Modern ve kullanıcı dostu arayüz
- Özelleştirilebilir ayarlar

## Sürüm
Version: $Version
Build Date: $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')

## Destek
Herhangi bir sorun için GitHub repository'sinde issue açabilirsiniz.
"@
    $usageGuide | Out-File "Release\KesifMetniOlusturucu\KULLANIM.md" -Encoding UTF8

    Write-Host "Release dosyaları düzenlendi." -ForegroundColor Green
    Write-Host

    # ZIP dosyası oluştur
    if (-not $SkipZip) {
        Write-Host "[6/6] ZIP dosyası oluşturuluyor..." -ForegroundColor Yellow
        $zipPath = "Release\KesifMetniOlusturucu-v$Version.zip"
        Compress-Archive -Path "Release\KesifMetniOlusturucu\*" -DestinationPath $zipPath -Force
        Write-Host "ZIP dosyası oluşturuldu: $zipPath" -ForegroundColor Green
    } else {
        Write-Host "[6/6] ZIP oluşturma atlandı." -ForegroundColor Yellow
    }

    Write-Host
    Write-Host "========================================" -ForegroundColor Cyan
    Write-Host "Release build tamamlandı!" -ForegroundColor Green
    Write-Host "========================================" -ForegroundColor Cyan
    Write-Host
    Write-Host "Dosyalar: Release\KesifMetniOlusturucu\" -ForegroundColor White
    Write-Host

    # Executable boyutunu göster
    $exeFile = Get-Item "Release\KesifMetniOlusturucu\KesifMetniOlusturucu.exe"
    $sizeInMB = [math]::Round($exeFile.Length / 1MB, 2)
    Write-Host "Executable boyutu: $sizeInMB MB" -ForegroundColor White
    Write-Host
    Write-Host "Test etmek için: Release\KesifMetniOlusturucu\Başlat.bat" -ForegroundColor Yellow
    Write-Host

} catch {
    Write-Host
    Write-Host "HATA: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host
    exit 1
}
