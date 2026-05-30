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

    public (bool isAnomaly, double score, string reason) Analyze(LogEntry log)
    {
        _profileService.BuildProfileFromLogs();
        var profile = _profileService.GetProfile(log.UserId);

        if (profile == null)
            return (false, 0, "Yeni kullanıcı - profil oluşturuluyor");

        double score = 0;
        var reasons = new List<string>();

        if (!profile.NormalHours.Contains(log.Timestamp.Hour))
        {
            score += 30;
            reasons.Add($"Olağan dışı saat: {log.Timestamp.Hour}:00");
        }

        if (!profile.NormalIPs.Contains(log.IPAddress))
        {
            score += 25;
            reasons.Add($"Bilinmeyen IP: {log.IPAddress}");
        }

        if (!profile.NormalActions.Contains(log.Action))
        {
            score += 25;
            reasons.Add($"Alışılmadık işlem: {log.Action}");
        }

        if (!profile.NormalResources.Contains(log.Resource))
        {
            score += 20;
            reasons.Add($"Bilinmeyen kaynak: {log.Resource}");
        }

        bool isAnomaly = score >= Threshold;
        string reason = reasons.Count > 0 ? string.Join(" | ", reasons) : "Normal";

        return (isAnomaly, score, reason);
    }

    public LogEntry AnalyzeAndMark(LogEntry log)
    {
        var (isAnomaly, score, reason) = Analyze(log);
        log.IsAnomaly = isAnomaly;
        log.AnomalyScore = score;
        log.AnomalyReason = reason;
        return log;
    }
}
