# 📝 Version Updater
# Proje dosyasındaki sürüm numarasını günceller

param(
    [Parameter(Mandatory=$true)]
    [string]$NewVersion,
    [string]$ProjectFile = "KesifUDFGenerator/KesifUDFGenerator.csproj"
)

Write-Host "📝 Sürüm güncelleniyor: $NewVersion" -ForegroundColor Cyan

if (-not (Test-Path $ProjectFile)) {
    Write-Error "❌ Proje dosyası bulunamadı: $ProjectFile"
    exit 1
}

# XML dosyasını oku
[xml]$project = Get-Content $ProjectFile

# PropertyGroup'u bul
$propertyGroup = $project.Project.PropertyGroup | Where-Object { $_.Version -or $_.AssemblyVersion }

if (-not $propertyGroup) {
    Write-Error "❌ PropertyGroup bulunamadı!"
    exit 1
}

# Sürüm numaralarını güncelle
$propertyGroup.Version = $NewVersion
$propertyGroup.AssemblyVersion = "$NewVersion.0"
$propertyGroup.FileVersion = "$NewVersion.0"

# Dosyayı kaydet
$project.Save((Resolve-Path $ProjectFile).Path)

Write-Host "✅ Sürüm güncellendi!" -ForegroundColor Green
Write-Host "   Version: $NewVersion" -ForegroundColor White
Write-Host "   AssemblyVersion: $NewVersion.0" -ForegroundColor White
Write-Host "   FileVersion: $NewVersion.0" -ForegroundColor White
