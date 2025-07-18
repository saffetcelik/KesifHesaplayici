using CommunityToolkit.Mvvm.ComponentModel;
using System.Globalization;

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
    /// Keşif saati string formatında
    /// </summary>
    public string KesifSaatiString
    {
        get => KesifSaati.ToString(@"hh\:mm");
        set
        {
            if (TimeSpan.TryParseExact(value, @"hh\:mm", CultureInfo.InvariantCulture, out var result))
            {
                KesifSaati = result;
            }
        }
    }

    /// <summary>
    /// Bilirkişi ücreti string formatında
    /// </summary>
    public string BilirkisiUcretiString
    {
        get => BilirkisiUcreti.ToString("F2", CultureInfo.CurrentCulture);
        set
        {
            if (decimal.TryParse(value, NumberStyles.Number, CultureInfo.CurrentCulture, out var result))
            {
                BilirkisiUcreti = result;
            }
        }
    }

    /// <summary>
    /// Keşif aracı ücreti string formatında
    /// </summary>
    public string KesifAraciUcretiString
    {
        get => KesifAraciUcreti.ToString("F2", CultureInfo.CurrentCulture);
        set
        {
            if (decimal.TryParse(value, NumberStyles.Number, CultureInfo.CurrentCulture, out var result))
            {
                KesifAraciUcreti = result;
            }
        }
    }

    /// <summary>
    /// Keşif harcı string formatında
    /// </summary>
    public string KesifHarciString
    {
        get => KesifHarci.ToString("F2", CultureInfo.CurrentCulture);
        set
        {
            if (decimal.TryParse(value, NumberStyles.Number, CultureInfo.CurrentCulture, out var result))
            {
                KesifHarci = result;
            }
        }
    }

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

    partial void OnKesifSaatiChanged(TimeSpan value)
    {
        OnPropertyChanged(nameof(KesifSaatiString));
    }

    partial void OnBilirkisiUcretiChanged(decimal value)
    {
        OnPropertyChanged(nameof(BilirkisiUcretiString));
    }

    partial void OnKesifAraciUcretiChanged(decimal value)
    {
        OnPropertyChanged(nameof(KesifAraciUcretiString));
    }

    partial void OnKesifHarciChanged(decimal value)
    {
        OnPropertyChanged(nameof(KesifHarciString));
    }
}
