using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KesifUDFGenerator.Models;
using KesifUDFGenerator.Services;
using KesifUDFGenerator.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace KesifUDFGenerator.ViewModels;

/// <summary>
/// Ana pencere için ViewModel
/// </summary>
public partial class MainViewModel : ObservableObject
{
    private readonly IKesifMetniService _kesifMetniService;
    private readonly ISettingsService _settingsService;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<MainViewModel> _logger;

    [ObservableProperty]
    private KesifBilgileri _kesifBilgileri = new();

    [ObservableProperty]
    private string _onizlemeMetni = string.Empty;

    [ObservableProperty]
    private AppSettings _appSettings = new();

    [ObservableProperty]
    private string _kopyalaButonMetni = "Metni Kopyala";

    /// <summary>
    /// Bilirkişi seçimleri koleksiyonu
    /// </summary>
    public ObservableCollection<BilirkisiSecimi> BilirkisiSecimleri { get; }

    /// <summary>
    /// Son yatırma süresi seçenekleri
    /// </summary>
    public ObservableCollection<int> SonYatirmaSuresiSecenekleri { get; }

    public MainViewModel(IKesifMetniService kesifMetniService, ISettingsService settingsService,
        IServiceProvider serviceProvider, ILogger<MainViewModel> logger)
    {
        _kesifMetniService = kesifMetniService;
        _settingsService = settingsService;
        _serviceProvider = serviceProvider;
        _logger = logger;

        // Bilirkişi seçimlerini başlat
        BilirkisiSecimleri = new ObservableCollection<BilirkisiSecimi>();

        // Son yatırma süresi seçenekleri
        SonYatirmaSuresiSecenekleri = new ObservableCollection<int>(Enumerable.Range(1, 10));

        // Keşif bilgileri değişikliklerini dinle
        KesifBilgileri.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName != nameof(KesifBilgileri.SonYatirmaGunu)) // Sonsuz döngüyü önle
            {
                OnizlemeGuncelle();
            }
        };

        // Ayarları yükle ve başlat
        _ = InitializeAsync();
    }

    /// <summary>
    /// Uygulamayı başlatma ve ayarları yükleme
    /// </summary>
    private async Task InitializeAsync()
    {
        try
        {
            // Ayarları yükle
            AppSettings = await _settingsService.LoadSettingsAsync();

            // Keşif bilgilerini ayarlara göre güncelle
            KesifBilgileri.BilirkisiUcreti = AppSettings.VarsayilanBilirkisiUcreti;
            KesifBilgileri.KesifAraciUcreti = AppSettings.VarsayilanKesifAraciUcreti;
            KesifBilgileri.KesifHarci = AppSettings.VarsayilanKesifHarci;
            KesifBilgileri.SonYatirmaSuresi = AppSettings.VarsayilanSonYatirmaSuresi;

            if (TimeSpan.TryParse(AppSettings.VarsayilanKesifSaati, out var saat))
            {
                KesifBilgileri.KesifSaati = saat;
            }

            // Bilirkişi seçimlerini ayarlara göre oluştur
            BilirkisiSecimleri.Clear();
            foreach (var bilirkisiTuru in AppSettings.BilirkisiTurleri)
            {
                var secim = new BilirkisiSecimi(bilirkisiTuru);
                secim.Secili = AppSettings.VarsayilanSeciliBilirkisiler.Contains(bilirkisiTuru);
                secim.PropertyChanged += (_, e) =>
                {
                    if (e.PropertyName != nameof(BilirkisiSecimi.TurAdi))
                    {
                        OnizlemeGuncelle();
                    }
                };
                BilirkisiSecimleri.Add(secim);
            }

            // İlk önizlemeyi oluştur
            OnizlemeGuncelle();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Uygulama başlatılırken hata oluştu");
        }
    }

    /// <summary>
    /// Ayarlar penceresini açma komutu
    /// </summary>
    [RelayCommand]
    private void AyarlariAc()
    {
        try
        {
            var settingsViewModel = _serviceProvider.GetRequiredService<SettingsViewModel>();
            var settingsWindow = new SettingsWindow(settingsViewModel)
            {
                Owner = Application.Current.MainWindow
            };

            if (settingsWindow.ShowDialog() == true)
            {
                // Ayarlar değiştirildi, yeniden yükle
                _ = InitializeAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ayarlar penceresi açılırken hata oluştu");
        }
    }

    /// <summary>
    /// Metni panoya kopyalama komutu
    /// </summary>
    [RelayCommand]
    private void MetniKopyala()
    {
        try
        {
            if (!string.IsNullOrEmpty(OnizlemeMetni))
            {
                Clipboard.SetText(OnizlemeMetni);
                _logger.LogInformation("Metin panoya kopyalandı");

                // Buton metnini değiştir
                KopyalaButonMetni = "Metin Kopyalandı";

                // 2 saniye sonra eski metne dön
                var timer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromSeconds(2)
                };
                timer.Tick += (sender, e) =>
                {
                    KopyalaButonMetni = "Metni Kopyala";
                    timer.Stop();
                };
                timer.Start();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Metin kopyalanırken hata oluştu");
        }
    }

    /// <summary>
    /// Önizlemeyi günceller
    /// </summary>
    private void OnizlemeGuncelle()
    {
        try
        {
            OnizlemeMetni = _kesifMetniService.IcerikOlustur(KesifBilgileri, BilirkisiSecimleri);
        }
        catch (Exception ex)
        {
            OnizlemeMetni = $"Önizleme oluşturulurken hata: {ex.Message}";
            _logger.LogWarning(ex, "Önizleme oluşturulurken hata oluştu");
        }
    }
}
