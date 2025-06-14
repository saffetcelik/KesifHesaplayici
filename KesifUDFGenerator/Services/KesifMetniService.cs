using System.Globalization;
using KesifUDFGenerator.Models;
using Microsoft.Extensions.Logging;

namespace KesifUDFGenerator.Services;

/// <summary>
/// Keşif metni oluşturma servisi implementasyonu
/// </summary>
public class KesifMetniService : IKesifMetniService
{
    private readonly ILogger<KesifMetniService> _logger;

    public KesifMetniService(ILogger<KesifMetniService> logger)
    {
        _logger = logger;
    }

    public string IcerikOlustur(KesifBilgileri kesifBilgileri, IEnumerable<BilirkisiSecimi> bilirkisiSecimleri)
    {
        try
        {
            var seciliBilirkisiler = bilirkisiSecimleri.Where(b => b.Secili).ToList();
            var bilirkisiMetni = string.Join(", ", seciliBilirkisiler.Select(b => b.ToString()));
            var toplamBilirkisiSayisi = seciliBilirkisiler.Sum(b => b.Adet);

            // Toplam gideri güncelle
            kesifBilgileri.ToplamGideriGuncelle(toplamBilirkisiSayisi);

            var kesifTarihiStr = kesifBilgileri.KesifTarihi.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
            var kesifSaatiStr = kesifBilgileri.KesifSaati.ToString(@"hh\:mm");
            var sonYatirmaGunuStr = kesifBilgileri.SonYatirmaGunu.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);

            var icerik = $"Taşınmaz başında {kesifTarihiStr} tarihinde saat {kesifSaatiStr} itibaren keşif icrasına, " +
                        $"keşfe gidilirken {bilirkisiMetni} refakate alınmasına, " +
                        $"bilirkişiler için {kesifBilgileri.BilirkisiUcreti:F0}TL ücret takdirine, " +
                        $"keşif aracı için {kesifBilgileri.KesifAraciUcreti:F0}TL ücret takdirine, " +
                        $"{kesifBilgileri.KesifHarci:F2}TL keşif harcı olmak üzere toplam {kesifBilgileri.ToplamGider:F2}TL " +
                        $"keşif giderinin delil avansından karşılanmasına, " +
                        $"davacı vekiline eksik delil avansını tamamlaması üzere {sonYatirmaGunuStr} tarihine kadar kesin süre verilmesine, " +
                        $"kesin sürenin gereği yerine getirilmediği takdirde HMK'nun 324. maddesi gereğince bu delile dayanmaktan " +
                        $"vazgeçmiş sayılacağının ihtarına, (davacı vekiline duruşma zaptının tebliği ile ihtarına)";

            _logger.LogInformation("Keşif metni başarıyla oluşturuldu");
            return icerik;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Keşif metni oluşturulurken hata oluştu");
            throw;
        }
    }
}
