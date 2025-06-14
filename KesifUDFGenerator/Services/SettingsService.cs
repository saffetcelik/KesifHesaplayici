using System.IO;
using System.Text.Json;
using KesifUDFGenerator.Models;
using Microsoft.Extensions.Logging;

namespace KesifUDFGenerator.Services;

/// <summary>
/// Ayarlar servisi implementasyonu
/// </summary>
public class SettingsService : ISettingsService
{
    private readonly ILogger<SettingsService> _logger;
    private readonly string _settingsFilePath;

    public SettingsService(ILogger<SettingsService> logger)
    {
        _logger = logger;
        // Proje kök dizinini al
        var currentDirectory = Directory.GetCurrentDirectory();
        _settingsFilePath = Path.Combine(currentDirectory, "settings.json");
    }

    public async Task<AppSettings> LoadSettingsAsync()
    {
        try
        {
            if (!File.Exists(_settingsFilePath))
            {
                _logger.LogInformation("Ayarlar dosyası bulunamadı, varsayılan ayarlar kullanılıyor");
                return GetDefaultSettings();
            }

            var json = await File.ReadAllTextAsync(_settingsFilePath);
            var settings = JsonSerializer.Deserialize<AppSettings>(json);
            
            if (settings == null)
            {
                _logger.LogWarning("Ayarlar dosyası okunamadı, varsayılan ayarlar kullanılıyor");
                return GetDefaultSettings();
            }

            _logger.LogInformation("Ayarlar başarıyla yüklendi");
            return settings;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ayarlar yüklenirken hata oluştu");
            return GetDefaultSettings();
        }
    }

    public async Task SaveSettingsAsync(AppSettings settings)
    {
        try
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            var json = JsonSerializer.Serialize(settings, options);
            await File.WriteAllTextAsync(_settingsFilePath, json);
            
            _logger.LogInformation("Ayarlar başarıyla kaydedildi");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ayarlar kaydedilirken hata oluştu");
            throw;
        }
    }

    public AppSettings GetDefaultSettings()
    {
        return new AppSettings();
    }
}
