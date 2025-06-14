using CommunityToolkit.Mvvm.ComponentModel;

namespace KesifUDFGenerator.Models;

/// <summary>
/// Uygulama ayarları modeli
/// </summary>
public partial class AppSettings : ObservableObject
{
    [ObservableProperty]
    private decimal _varsayilanBilirkisiUcreti = 2700m;

    [ObservableProperty]
    private decimal _varsayilanKesifAraciUcreti = 2000m;

    [ObservableProperty]
    private decimal _varsayilanKesifHarci = 4361.50m;

    [ObservableProperty]
    private int _varsayilanSonYatirmaSuresi = 7;

    [ObservableProperty]
    private string _varsayilanKesifSaati = "15:30";

    [ObservableProperty]
    private List<string> _bilirkisiTurleri = new()
    {
        "İnşaat Mühendisi",
        "Orman Yüksek Mühendisi", 
        "Fen Bilirkişisi",
        "Ziraat Bilirkişisi",
        "Gayrimenkul Değerleme Uzmanı",
        "Hesap Bilirkişisi",
        "Yazılım Bilirkişisi",
        "İç Denetçi Bilirkişi"
    };

    [ObservableProperty]
    private List<string> _varsayilanSeciliBilirkisiler = new()
    {
        "İnşaat Mühendisi",
        "Fen Bilirkişisi", 
        "Gayrimenkul Değerleme Uzmanı"
    };
}
