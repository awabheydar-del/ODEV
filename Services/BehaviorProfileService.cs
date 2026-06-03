using ZeroDayAnomali.Models;

namespace ZeroDayAnomali.Services;

public class BehaviorProfileService
{
    private readonly Dictionary<string, Profil> _profiles = new();
    private readonly LogService _logService;

    public BehaviorProfileService(LogService logService)
    {
        _logService = logService;
    }

    public Profil GetOrCreateProfile(string kulId, string kulAdı)
    {
        if (!_profiles.ContainsKey(kulId))
        {
            _profiles[kulId] = new Profil
            {
                kul_id = kulId,
                kul_adı = kulAdı,
            };
        }
        return _profiles[kulId];
    }

    public void BuildProfileFromLogs()
    {
        _profiles.Clear();
        var logs = _logService.GetAll().Where(l => !l.anormal);

        foreach (var log in logs)
        {
            var profile = GetOrCreateProfile(log.kul_id, log.kul_adı);
            if (!profile.normal_saatler.Contains(log.tarih.Hour))
                profile.normal_saatler.Add(log.tarih.Hour);
            if (!profile.normal_iplar.Contains(log.ip))
                profile.normal_iplar.Add(log.ip);
            if (!profile.normal_işlemler.Contains(log.işlem))
                profile.normal_işlemler.Add(log.işlem);
            if (!profile.normal_kaynaklar.Contains(log.kaynak))
                profile.normal_kaynaklar.Add(log.kaynak);
        }
    }

    public Profil? GetProfile(string kulId)
    {
        return _profiles.GetValueOrDefault(kulId);
    }
}
