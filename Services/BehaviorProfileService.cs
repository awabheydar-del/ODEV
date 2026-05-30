using ZeroDayAnomali.Models;

namespace ZeroDayAnomali.Services;

public class BehaviorProfileService
{
    private readonly Dictionary<string, UserBehaviorProfile> _profiles = new();
    private readonly LogService _logService;

    public BehaviorProfileService(LogService logService)
    {
        _logService = logService;
    }

    public UserBehaviorProfile GetOrCreateProfile(string userId, string userName)
    {
        if (!_profiles.ContainsKey(userId))
        {
            _profiles[userId] = new UserBehaviorProfile
            {
                UserId = userId,
                UserName = userName,
            };
        }
        return _profiles[userId];
    }

    public void BuildProfileFromLogs()
    {
        _profiles.Clear();
        var logs = _logService.GetAll().Where(l => !l.IsAnomaly);

        foreach (var log in logs)
        {
            var profile = GetOrCreateProfile(log.UserId, log.UserName);
            if (!profile.NormalHours.Contains(log.Timestamp.Hour))
                profile.NormalHours.Add(log.Timestamp.Hour);
            if (!profile.NormalIPs.Contains(log.IPAddress))
                profile.NormalIPs.Add(log.IPAddress);
            if (!profile.NormalActions.Contains(log.Action))
                profile.NormalActions.Add(log.Action);
            if (!profile.NormalResources.Contains(log.Resource))
                profile.NormalResources.Add(log.Resource);
        }
    }

    public UserBehaviorProfile? GetProfile(string userId)
    {
        return _profiles.GetValueOrDefault(userId);
    }
}
