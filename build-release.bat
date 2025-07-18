@echo off
title Keşif Metni Oluşturucu - Release Build
echo ========================================
echo Keşif Metni Oluşturucu Release Build
echo ========================================
echo.

REM Proje dizinine git
cd /d "%~dp0"

REM Temizlik
echo [1/5] Temizlik yapılıyor...
if exist "KesifUDFGenerator\bin\Release" rmdir /s /q "KesifUDFGenerator\bin\Release"
if exist "KesifUDFGenerator\obj\Release" rmdir /s /q "KesifUDFGenerator\obj\Release"
if exist "Release" rmdir /s /q "Release"
echo Temizlik tamamlandı.
echo.

REM Restore
echo [2/5] NuGet paketleri geri yükleniyor...
dotnet restore KesifUDFGenerator\KesifUDFGenerator.csproj
if %ERRORLEVEL% neq 0 (
    echo HATA: NuGet restore başarısız!
    pause
    exit /b 1
)
echo NuGet restore tamamlandı.
echo.

REM Build
echo [3/5] Proje derleniyor...
dotnet build KesifUDFGenerator\KesifUDFGenerator.csproj -c Release
if %ERRORLEVEL% neq 0 (
    echo HATA: Build başarısız!
    pause
    exit /b 1
)
echo Build tamamlandı.
echo.

REM Publish - Self-contained executable
echo [4/5] Self-contained executable oluşturuluyor...
dotnet publish KesifUDFGenerator\KesifUDFGenerator.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:PublishReadyToRun=true -p:IncludeNativeLibrariesForSelfExtract=true -o Release\win-x64
if %ERRORLEVEL% neq 0 (
    echo HATA: Publish başarısız!
    pause
    exit /b 1
)
echo Self-contained executable oluşturuldu.
echo.

REM Release klasörünü düzenle
echo [5/5] Release dosyaları düzenleniyor...
mkdir Release\KesifMetniOlusturucu 2>nul

REM Ana executable'ı kopyala
copy "Release\win-x64\KesifUDFGenerator.exe" "Release\KesifMetniOlusturucu\KesifMetniOlusturucu.exe"

REM Gerekli dosyaları kopyala
copy "README.md" "Release\KesifMetniOlusturucu\"
copy "LICENSE" "Release\KesifMetniOlusturucu\"

REM Başlatma script'i oluştur
echo @echo off > "Release\KesifMetniOlusturucu\Başlat.bat"
echo title Keşif Metni Oluşturucu >> "Release\KesifMetniOlusturucu\Başlat.bat"
echo cd /d "%%~dp0" >> "Release\KesifMetniOlusturucu\Başlat.bat"
echo KesifMetniOlusturucu.exe >> "Release\KesifMetniOlusturucu\Başlat.bat"

REM Kullanım kılavuzu oluştur
echo # Keşif Metni Oluşturucu - Kullanım Kılavuzu > "Release\KesifMetniOlusturucu\KULLANIM.md"
echo. >> "Release\KesifMetniOlusturucu\KULLANIM.md"
echo ## Kurulum >> "Release\KesifMetniOlusturucu\KULLANIM.md"
echo 1. Bu klasörü istediğiniz yere kopyalayın >> "Release\KesifMetniOlusturucu\KULLANIM.md"
echo 2. Başlat.bat dosyasına çift tıklayarak uygulamayı başlatın >> "Release\KesifMetniOlusturucu\KULLANIM.md"
echo. >> "Release\KesifMetniOlusturucu\KULLANIM.md"
echo ## Sistem Gereksinimleri >> "Release\KesifMetniOlusturucu\KULLANIM.md"
echo - Windows 10/11 (64-bit) >> "Release\KesifMetniOlusturucu\KULLANIM.md"
echo - .NET Runtime gerekmez (self-contained) >> "Release\KesifMetniOlusturucu\KULLANIM.md"
echo. >> "Release\KesifMetniOlusturucu\KULLANIM.md"
echo ## Özellikler >> "Release\KesifMetniOlusturucu\KULLANIM.md"
echo - Mahkeme keşif kararları için otomatik metin oluşturma >> "Release\KesifMetniOlusturucu\KULLANIM.md"
echo - Bilirkişi ücret hesaplamaları >> "Release\KesifMetniOlusturucu\KULLANIM.md"
echo - Modern ve kullanıcı dostu arayüz >> "Release\KesifMetniOlusturucu\KULLANIM.md"
echo - Özelleştirilebilir ayarlar >> "Release\KesifMetniOlusturucu\KULLANIM.md"

REM ZIP dosyası oluştur (eğer 7-Zip varsa)
if exist "C:\Program Files\7-Zip\7z.exe" (
    echo ZIP dosyası oluşturuluyor...
    "C:\Program Files\7-Zip\7z.exe" a -tzip "Release\KesifMetniOlusturucu-v1.0.0.zip" "Release\KesifMetniOlusturucu\*"
    echo ZIP dosyası oluşturuldu: Release\KesifMetniOlusturucu-v1.0.0.zip
) else (
    echo 7-Zip bulunamadı, ZIP dosyası oluşturulamadı.
    echo Manuel olarak Release\KesifMetniOlusturucu klasörünü sıkıştırabilirsiniz.
)

echo.
echo ========================================
echo Release build tamamlandı!
echo ========================================
echo.
echo Dosyalar: Release\KesifMetniOlusturucu\
echo.
echo Executable boyutu:
dir "Release\KesifMetniOlusturucu\KesifMetniOlusturucu.exe" | find ".exe"
echo.
echo Test etmek için: Release\KesifMetniOlusturucu\Başlat.bat
echo.
pause
