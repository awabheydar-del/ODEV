namespace ZeroDayAnomali.Models;

public class LogEntry
{
    public int Id { get; set; }
    public DateTime Timestamp { get; set; }
    public string UserId { get; set; } = "";
    public string UserName { get; set; } = "";
    public string Action { get; set; } = "";
    public string Resource { get; set; } = "";
    public string IPAddress { get; set; } = "";
    public bool IsSuccess { get; set; } = true;
    public bool IsAnomaly { get; set; }
    public double AnomalyScore { get; set; }
    public string? AnomalyReason { get; set; }

    public string Severity
    {
        get
        {
            if (!IsAnomaly) return "Normal";
            if (AnomalyScore >= 80) return "Critical";
            if (AnomalyScore >= 60) return "High";
            return "Medium";
        }
    }
}
