# Keşif Metni Oluşturucu

Modern .NET 8 WPF uygulaması ile geliştirilmiş mahkeme keşif kararları için metin oluşturucu.

## Ön İzleme

![Screenshot_1](https://github.com/user-attachments/assets/c3507480-c986-414b-915b-8da312d2e0c8)


## Özellikler

- **Kompakt Modern UI**: Material Design ile tasarlanmış minimal arayüz
- **Custom Window Chrome**: Modern kapatma ve minimize butonları
- **MVVM Pattern**: Clean Architecture ile geliştirilmiş
- **Dependency Injection**: Microsoft.Extensions.Hosting ile yapılandırılmış
- **Logging**: Kapsamlı loglama desteği
- **Real-time Preview**: Anlık önizleme özelliği
- **Clipboard Integration**: Tek tıkla metin kopyalama
- **Otomatik Hesaplama**: Toplam gider otomatik hesaplanır
- **Responsive Design**: Tüm öğeler kompakt alanda düzenli yerleşim
- **Ayarlar Sistemi**: JSON tabanlı kullanıcı ayarları
- **Özelleştirilebilir Bilirkişiler**: Yeni bilirkişi türleri ekleme/silme
- **Varsayılan Değerler**: Kullanıcı tanımlı varsayılan ücretler
- **Türkçe Lokalizasyon**: Tarih seçici ve arayüz Türkçe

## Teknolojiler

- **.NET 8**: En son .NET framework
- **WPF**: Windows Presentation Foundation
- **Material Design**: Modern UI kütüphanesi
- **CommunityToolkit.Mvvm**: MVVM pattern için
- **Microsoft.Extensions.Hosting**: Dependency injection ve configuration

## 📦 İndirme ve Kurulum

### 🚀 Hızlı Başlangıç (Önerilen)
1. [Releases](https://github.com/[repository-url]/releases) sayfasından en son sürümü indirin
2. ZIP dosyasını çıkarın
3. `Başlat.bat` dosyasına çift tıklayın
4. Hepsi bu kadar! 🎉

### 💻 Sistem Gereksinimleri
- Windows 10/11 (64-bit)
- **Hiçbir ek yazılım gerekmez** (.NET Runtime dahil)

### 👨‍💻 Geliştirici Kurulumu
```bash
# Projeyi klonlayın
git clone [repository-url]

# Proje dizinine gidin
cd KesifHesaplayici

# Uygulamayı çalıştırın
dotnet run --project KesifUDFGenerator
```

**Gereksinimler (sadece geliştirme için):**
- .NET 8 SDK
- Windows 10/11

## Kullanım

1. **Keşif Tarihi**: Varsayılan olarak bir sonraki çarşamba günü seçilir
2. **Keşif Saati**: Varsayılan 15:30
3. **Bilirkişiler**: İhtiyaç duyulan bilirkişi türlerini seçin ve adetlerini belirleyin
4. **Ücret Bilgileri**: Bilirkişi, araç ve harç ücretlerini girin
5. **Son Yatırma Süresi**: Gün sayısını seçin
6. **Önizleme**: Sağ panelde oluşturulan metin görüntülenir
7. **Metni Kopyala**: Butona tıklayarak metni panoya kopyalayın

### Yeni Özellikler
- **Kompakt Tasarım**: Tüm öğeler tek ekranda görünür
- **Custom Window**: Modern başlık çubuğu ve kontroller
- **Drag & Drop**: Pencereyi sürükleyerek taşıyabilirsiniz
- **Instant Copy**: Tek tıkla metin kopyalama
- **Ayarlar Menüsü**: Sol üst köşedeki dişli ikonu ile ayarlara erişim
- **Dinamik Bilirkişiler**: Ayarlardan yeni bilirkişi türleri ekleyebilirsiniz
- **Varsayılan Değerler**: Tüm ücretler ve süreler özelleştirilebilir
- **JSON Ayarlar**: Ayarlar %AppData%\KesifMetniOlusturucu\settings.json dosyasında saklanır

## 🚀 Hızlı Başlatma

### Başlatma
```batch
# Uygulamayı başlat
Başlat.bat
```

### Manuel Çalıştırma
```bash
# Proje dizininde
dotnet run --project KesifUDFGenerator
```

## Proje Yapısı

```
KesifUDFGenerator/
├── Models/                 # Veri modelleri
│   ├── BilirkisiTuru.cs
│   ├── BilirkisiSecimi.cs
│   └── KesifBilgileri.cs
├── ViewModels/             # MVVM ViewModels
│   └── MainViewModel.cs
├── Services/               # İş mantığı servisleri
│   ├── IKesifMetniService.cs
│   └── KesifMetniService.cs
├── Converters/             # XAML Converters
│   ├── DecimalToStringConverter.cs
│   └── TimeSpanToStringConverter.cs
├── App.xaml               # Uygulama giriş noktası
├── MainWindow.xaml        # Ana pencere UI
└── appsettings.json       # Konfigürasyon
```

## Özellikler Detayı

### Bilirkişi Türleri
- İnşaat Mühendisi
- Orman Yüksek Mühendisi
- Fen Bilirkişisi
- Ziraat Bilirkişisi
- Gayrimenkul Değerleme Uzmanı
- Hesap Bilirkişisi
- Yazılım Bilirkişisi
- İç Denetçi Bilirkişi

### Varsayılan Değerler
- Bilirkişi Ücreti: 2.700 TL
- Keşif Aracı Ücreti: 2.000 TL
- Keşif Harcı: 4.361,50 TL
- Son Yatırma Süresi: 7 gün

## 🚀 Kullanım

### Hızlı Başlatma
```batch
# Release sürümü için
Başlat.bat
```

### Geliştirici Çalıştırma
```bash
# Kaynak koddan çalıştırma
dotnet run --project KesifUDFGenerator
```

## 📁 Proje Yapısı

```
kesif/
├── Başlat.bat              # Uygulama başlatıcı
├── settings.json           # Kullanıcı ayarları (otomatik oluşur)
├── README.md               # Proje dokümantasyonu
└── KesifUDFGenerator/      # Ana proje
    ├── Models/             # Veri modelleri + AppSettings
    ├── ViewModels/         # MVVM + SettingsViewModel
    ├── Views/              # SettingsWindow
    ├── Services/           # SettingsService + KesifMetniService
    ├── Converters/         # XAML Converters
    ├── MainWindow.xaml     # Ana pencere
    └── App.xaml           # Uygulama giriş noktası
```

## 🔧 Geliştirme

### Build
```bash
dotnet build
```

### Release Build
```bash
# PowerShell script kullanarak
.\build-release.ps1

# Veya batch script
.\build-release.bat
```

### Manuel Publish
```bash
dotnet publish -c Release -r win-x64 --self-contained
```

## Sorun Giderme

### Stack Overflow Hatası
Eğer "Yığın için yeni bir sınır koruma sayfası oluşturulamaz" hatası alırsanız:
- Uygulamayı kapatın
- `dotnet clean` komutu çalıştırın
- `dotnet build` ile yeniden derleyin
- Uygulamayı tekrar başlatın

### Binding Hataları
XAML binding hatalarını önlemek için:
- PropertyChanged event'lerinde sonsuz döngü kontrolü yapılmıştır
- Computed property'ler için manuel OnPropertyChanged çağrıları kaldırılmıştır

## Lisans

Bu proje MIT lisansı altında lisanslanmıştır.

## Katkıda Bulunma

1. Fork edin
2. Feature branch oluşturun (`git checkout -b feature/AmazingFeature`)
3. Değişikliklerinizi commit edin (`git commit -m 'Add some AmazingFeature'`)
4. Branch'inizi push edin (`git push origin feature/AmazingFeature`)
5. Pull Request oluşturun

## İletişim

Herhangi bir sorunuz için issue açabilirsiniz.
