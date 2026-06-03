namespace ZeroDayAnomali.Models;

public class İşlem
{
    public int işlem_id { get; set; }
    public DateTime tarih { get; set; }
    public string kul_id { get; set; } = "";
    public string kul_adı { get; set; } = "";
    public string işlem { get; set; } = "";
    public string kaynak { get; set; } = "";
    public string ip { get; set; } = "";
    public bool başarılı { get; set; } = true;
    public bool anormal { get; set; }
    public double puan { get; set; }
    public string? neden { get; set; }
}
