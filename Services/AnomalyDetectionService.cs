using ZeroDayAnomali.Models;

namespace ZeroDayAnomali.Services;

public class AnomalyDetectionService
{
    private readonly BehaviorProfileService _profileService;
    private readonly LogService _logService;
    private const double Threshold = 50;

    public AnomalyDetectionService(BehaviorProfileService profileService, LogService logService)
    {
        _profileService = profileService;
        _logService = logService;
    }

    public (bool anormal, double puan, string neden) Analyze(İşlem log)
    {
        _profileService.BuildProfileFromLogs();
        var profile = _profileService.GetProfile(log.kul_id);

        if (profile == null)
            return (false, 0, "Yeni kullanıcı - profil oluşturuluyor");

        double puan = 0;
        var reasons = new List<string>();

        if (!profile.normal_saatler.Contains(log.tarih.Hour))
        {
            puan += 30;
            reasons.Add($"Olağan dışı saat: {log.tarih.Hour}:00");
        }

        if (!profile.normal_iplar.Contains(log.ip))
        {
            puan += 25;
            reasons.Add($"Bilinmeyen IP: {log.ip}");
        }

        if (!profile.normal_işlemler.Contains(log.işlem))
        {
            puan += 25;
            reasons.Add($"Alışılmadık işlem: {log.işlem}");
        }

        if (!profile.normal_kaynaklar.Contains(log.kaynak))
        {
            puan += 20;
            reasons.Add($"Bilinmeyen kaynak: {log.kaynak}");
        }

        bool anormal = puan >= Threshold;
        string neden = reasons.Count > 0 ? string.Join(" | ", reasons) : "Normal";

        return (anormal, puan, neden);
    }

    public İşlem AnalyzeAndMark(İşlem log)
    {
        var (anormal, puan, neden) = Analyze(log);
        log.anormal = anormal;
        log.puan = puan;
        log.neden = neden;
        return log;
    }
}
