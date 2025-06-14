using CommunityToolkit.Mvvm.ComponentModel;

namespace KesifUDFGenerator.Models;

/// <summary>
/// Keşif bilgilerini tutan model sınıfı
/// </summary>
public partial class KesifBilgileri : ObservableObject
{
    [ObservableProperty]
    private DateTime _kesifTarihi = DateTime.Now.AddDays(GetNextWednesdayDays());

    [ObservableProperty]
    private TimeSpan _kesifSaati = new(15, 30, 0);

    [ObservableProperty]
    private decimal _bilirkisiUcreti = 2000m;

    [ObservableProperty]
    private decimal _kesifAraciUcreti = 1500m;

    [ObservableProperty]
    private decimal _kesifHarci = 3030.30m;

    [ObservableProperty]
    private int _sonYatirmaSuresi = 7;

    [ObservableProperty]
    private decimal _toplamGider;

    /// <summary>
    /// Son yatırma gününü hesaplar
    /// </summary>
    public DateTime SonYatirmaGunu => KesifTarihi.AddDays(-SonYatirmaSuresi);

    /// <summary>
    /// Toplam gideri günceller
    /// </summary>
    public void ToplamGideriGuncelle(int toplamBilirkisiSayisi)
    {
        ToplamGider = (BilirkisiUcreti * toplamBilirkisiSayisi) + KesifAraciUcreti + KesifHarci;
    }

    /// <summary>
    /// Bir sonraki çarşambaya kadar olan gün sayısını hesaplar
    /// </summary>
    private static int GetNextWednesdayDays()
    {
        var today = DateTime.Now;
        var daysAhead = 9 - (int)today.DayOfWeek; // 9 = next Wednesday (2 = this Wednesday)
        if (daysAhead <= 0) // Target day already happened this week
            daysAhead += 7;
        return daysAhead;
    }

    partial void OnKesifTarihiChanged(DateTime value)
    {
        OnPropertyChanged(nameof(SonYatirmaGunu));
    }

    partial void OnSonYatirmaSuresiChanged(int value)
    {
        OnPropertyChanged(nameof(SonYatirmaGunu));
    }

    partial void OnBilirkisiUcretiChanged(decimal value)
    {
        // ToplamGider otomatik olarak güncellenir, manuel çağırma gerekmez
    }

    partial void OnKesifAraciUcretiChanged(decimal value)
    {
        // ToplamGider otomatik olarak güncellenir, manuel çağırma gerekmez
    }

    partial void OnKesifHarciChanged(decimal value)
    {
        // ToplamGider otomatik olarak güncellenir, manuel çağırma gerekmez
    }
}
