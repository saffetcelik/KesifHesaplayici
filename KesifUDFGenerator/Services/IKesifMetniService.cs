using KesifUDFGenerator.Models;

namespace KesifUDFGenerator.Services;

/// <summary>
/// Keşif metni oluşturma servisi arayüzü
/// </summary>
public interface IKesifMetniService
{
    /// <summary>
    /// Keşif bilgilerinden metin içeriği oluşturur
    /// </summary>
    /// <param name="kesifBilgileri">Keşif bilgileri</param>
    /// <param name="bilirkisiSecimleri">Bilirkişi seçimleri</param>
    /// <returns>Keşif metni</returns>
    string IcerikOlustur(KesifBilgileri kesifBilgileri, IEnumerable<BilirkisiSecimi> bilirkisiSecimleri);
}
