# 🚀 Keşif Metni Oluşturucu - Release Builder
# Bu script ile yerel olarak release paketi oluşturabilirsiniz

param(
    [Parameter(Mandatory=$true)]
    [string]$Version,
    [string]$Configuration = "Release"
)

Write-Host "🚀 Keşif Metni Oluşturucu Release Builder" -ForegroundColor Cyan
Write-Host "📦 Sürüm: $Version" -ForegroundColor Green
Write-Host "⚙️ Yapılandırma: $Configuration" -ForegroundColor Yellow

# Çalışma dizinini kontrol et
$projectPath = "KesifUDFGenerator/KesifUDFGenerator.csproj"
if (-not (Test-Path $projectPath)) {
    Write-Error "❌ Proje dosyası bulunamadı: $projectPath"
    Write-Host "💡 Bu scripti repo kök dizininden çalıştırın" -ForegroundColor Yellow
    exit 1
}

# Temizlik
Write-Host "🧹 Önceki build dosyalarını temizleniyor..." -ForegroundColor Yellow
if (Test-Path "publish") { Remove-Item "publish" -Recurse -Force }
if (Test-Path "releases") { Remove-Item "releases" -Recurse -Force }

# Restore
Write-Host "📥 Dependencies restore ediliyor..." -ForegroundColor Yellow
dotnet restore $projectPath
if ($LASTEXITCODE -ne 0) {
    Write-Error "❌ Restore başarısız!"
    exit 1
}

# Build
Write-Host "🏗️ Uygulama build ediliyor..." -ForegroundColor Yellow
dotnet build $projectPath --configuration $Configuration --no-restore
if ($LASTEXITCODE -ne 0) {
    Write-Error "❌ Build başarısız!"
    exit 1
}

# Publish - Self-Contained
Write-Host "📦 Self-contained exe oluşturuluyor..." -ForegroundColor Yellow
dotnet publish $projectPath `
    --configuration $Configuration `
    --runtime win-x64 `
    --self-contained true `
    --output "./publish" `
    -p:PublishSingleFile=true `
    -p:PublishTrimmed=false `
    -p:IncludeNativeLibrariesForSelfExtract=true

if ($LASTEXITCODE -ne 0) {
    Write-Error "❌ Publish başarısız!"
    exit 1
}

# Release klasörü oluştur
New-Item -ItemType Directory -Path "releases" -Force | Out-Null

# Dosyaları kopyala
$packageName = "KesifMetniOlusturucu-v$Version-win-x64"
$exeName = "$packageName.exe"

Write-Host "📁 Release paketi hazırlanıyor..." -ForegroundColor Yellow

# Ana exe dosyasını kopyala
Copy-Item "./publish/KesifUDFGenerator.exe" "./releases/$exeName"

# Ayarlar dosyasını kopyala
if (Test-Path "./publish/appsettings.json") {
    Copy-Item "./publish/appsettings.json" "./releases/appsettings.json"
}

# README dosyası oluştur
$readmeContent = @"
# 🎉 Keşif Metni Oluşturucu v$Version

## 📦 Kurulum
1. ``$exeName`` dosyasını istediğiniz klasöre kopyalayın
2. Çift tıklayarak çalıştırın - kurulum gerektirmez!
3. İlk çalıştırmada ayarlarınızı yapılandırın

## ⚙️ Yapılandırma
- ``appsettings.json`` dosyası ile özelleştirilebilir
- Varsayılan ücretler ayarlanabilir
- Bilirkişi türleri eklenebilir/düzenlenebilir

## 💻 Sistem Gereksinimleri
- Windows 10/11 (x64)
- .NET Runtime dahil (self-contained)

## 🆘 Destek
Sorun yaşarsanız GitHub Issues bölümünden bildirebilirsiniz.

---
© 2025 Keşif Tools. Tüm hakları saklıdır.
"@

$readmeContent | Out-File -FilePath "./releases/README.md" -Encoding UTF8

# ZIP paketi oluştur
Write-Host "🗜️ ZIP paketi oluşturuluyor..." -ForegroundColor Yellow
Compress-Archive -Path "./releases/$exeName", "./releases/appsettings.json", "./releases/README.md" -DestinationPath "./releases/$packageName.zip"

# Sonuç
Write-Host ""
Write-Host "✅ Release başarıyla oluşturuldu!" -ForegroundColor Green
Write-Host "📁 Konum: ./releases/" -ForegroundColor Cyan
Write-Host "📦 Dosyalar:" -ForegroundColor Cyan
Get-ChildItem "./releases/" | ForEach-Object {
    $size = if ($_.Length -gt 1MB) { "{0:N1} MB" -f ($_.Length / 1MB) } else { "{0:N0} KB" -f ($_.Length / 1KB) }
    Write-Host "   - $($_.Name) ($size)" -ForegroundColor White
}

Write-Host ""
Write-Host "🚀 GitHub Release için:" -ForegroundColor Yellow
Write-Host "   1. Git tag oluşturun: git tag v$Version" -ForegroundColor White
Write-Host "   2. Tag'i push edin: git push origin v$Version" -ForegroundColor White
Write-Host "   3. GitHub Actions otomatik release oluşturacak!" -ForegroundColor White
