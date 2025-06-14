using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KesifUDFGenerator.Models;
using KesifUDFGenerator.Services;
using Microsoft.Extensions.Logging;

namespace KesifUDFGenerator.ViewModels;

/// <summary>
/// Ayarlar penceresi için ViewModel
/// </summary>
public partial class SettingsViewModel : ObservableObject
{
    private readonly ISettingsService _settingsService;
    private readonly ILogger<SettingsViewModel> _logger;

    [ObservableProperty]
    private AppSettings _settings = new();

    [ObservableProperty]
    private string _yeniBilirkisiTuru = string.Empty;

    /// <summary>
    /// Bilirkişi türleri koleksiyonu
    /// </summary>
    public ObservableCollection<string> BilirkisiTurleri { get; }

    /// <summary>
    /// Pencereyi kapatma action'ı
    /// </summary>
    public Action? CloseWindow { get; set; }

    public SettingsViewModel(ISettingsService settingsService, ILogger<SettingsViewModel> logger)
    {
        _settingsService = settingsService;
        _logger = logger;
        
        BilirkisiTurleri = new ObservableCollection<string>();
        
        // Ayarları yükle
        _ = LoadSettingsAsync();
    }

    /// <summary>
    /// Ayarları yükleme komutu
    /// </summary>
    [RelayCommand]
    private async Task LoadSettingsAsync()
    {
        try
        {
            Settings = await _settingsService.LoadSettingsAsync();
            
            // Bilirkişi türlerini koleksiyona ekle
            BilirkisiTurleri.Clear();
            foreach (var tur in Settings.BilirkisiTurleri)
            {
                BilirkisiTurleri.Add(tur);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ayarlar yüklenirken hata oluştu");
            MessageBox.Show($"Ayarlar yüklenirken hata oluştu: {ex.Message}", "Hata", 
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    /// <summary>
    /// Ayarları kaydetme komutu
    /// </summary>
    [RelayCommand]
    private async Task SaveSettingsAsync()
    {
        try
        {
            // Koleksiyondaki değişiklikleri settings'e aktar
            Settings.BilirkisiTurleri = BilirkisiTurleri.ToList();

            await _settingsService.SaveSettingsAsync(Settings);

            MessageBox.Show("Ayarlar başarıyla kaydedildi!", "Başarılı",
                MessageBoxButton.OK, MessageBoxImage.Information);

            // Pencereyi kapat
            CloseWindow?.Invoke();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ayarlar kaydedilirken hata oluştu");
            MessageBox.Show($"Ayarlar kaydedilirken hata oluştu: {ex.Message}", "Hata",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    /// <summary>
    /// Yeni bilirkişi türü ekleme komutu
    /// </summary>
    [RelayCommand]
    private void YeniBilirkisiEkle()
    {
        if (string.IsNullOrWhiteSpace(YeniBilirkisiTuru))
            return;

        if (BilirkisiTurleri.Contains(YeniBilirkisiTuru))
        {
            MessageBox.Show("Bu bilirkişi türü zaten mevcut!", "Uyarı", 
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        BilirkisiTurleri.Add(YeniBilirkisiTuru);
        YeniBilirkisiTuru = string.Empty;
    }

    /// <summary>
    /// Bilirkişi türü silme komutu
    /// </summary>
    [RelayCommand]
    private void BilirkisiSil(string? bilirkisiTuru)
    {
        if (string.IsNullOrEmpty(bilirkisiTuru))
            return;

        var result = MessageBox.Show($"'{bilirkisiTuru}' bilirkişi türünü silmek istediğinizden emin misiniz?", 
            "Onay", MessageBoxButton.YesNo, MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            BilirkisiTurleri.Remove(bilirkisiTuru);
        }
    }

    /// <summary>
    /// Varsayılan ayarlara dönme komutu
    /// </summary>
    [RelayCommand]
    private void VarsayilanAyarlariYukle()
    {
        var result = MessageBox.Show("Tüm ayarları varsayılan değerlere döndürmek istediğinizden emin misiniz?", 
            "Onay", MessageBoxButton.YesNo, MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            Settings = _settingsService.GetDefaultSettings();
            
            BilirkisiTurleri.Clear();
            foreach (var tur in Settings.BilirkisiTurleri)
            {
                BilirkisiTurleri.Add(tur);
            }
        }
    }
}
