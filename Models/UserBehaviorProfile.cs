namespace ZeroDayAnomali.Models;

public class Profil
{
    public string kul_id { get; set; } = "";
    public string kul_adı { get; set; } = "";
    public List<int> normal_saatler { get; set; } = new();
    public List<string> normal_iplar { get; set; } = new();
    public List<string> normal_işlemler { get; set; } = new();
    public List<string> normal_kaynaklar { get; set; } = new();
}
