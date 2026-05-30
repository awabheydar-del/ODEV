namespace ZeroDayAnomali.Models;

public class UserBehaviorProfile
{
    public string UserId { get; set; } = "";
    public string UserName { get; set; } = "";
    public List<int> NormalHours { get; set; } = new();
    public List<string> NormalIPs { get; set; } = new();
    public List<string> NormalActions { get; set; } = new();
    public List<string> NormalResources { get; set; } = new();
}
