using KesifUDFGenerator.Models;

namespace KesifUDFGenerator.Services;

/// <summary>
/// Ayarlar servisi arayüzü
/// </summary>
public interface ISettingsService
{
    /// <summary>
    /// Ayarları yükler
    /// </summary>
    Task<AppSettings> LoadSettingsAsync();

    /// <summary>
    /// Ayarları kaydeder
    /// </summary>
    Task SaveSettingsAsync(AppSettings settings);

    /// <summary>
    /// Varsayılan ayarları döndürür
    /// </summary>
    AppSettings GetDefaultSettings();
}
