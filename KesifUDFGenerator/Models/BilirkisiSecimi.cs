using CommunityToolkit.Mvvm.ComponentModel;

namespace KesifUDFGenerator.Models;

/// <summary>
/// Bilirkişi seçimi için model sınıfı
/// </summary>
public partial class BilirkisiSecimi : ObservableObject
{
    [ObservableProperty]
    private string _turAdi;

    [ObservableProperty]
    private bool _secili;

    [ObservableProperty]
    private int _adet = 1;

    public BilirkisiSecimi(string turAdi, bool secili = false, int adet = 1)
    {
        TurAdi = turAdi;
        Secili = secili;
        Adet = adet;
    }

    /// <summary>
    /// Bilirkişi seçiminin metin temsilini döndürür
    /// </summary>
    public override string ToString()
    {
        return Secili ? $"{Adet} {TurAdi}" : string.Empty;
    }
}
