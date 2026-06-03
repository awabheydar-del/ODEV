namespace ZeroDayAnomali.Models;

public class DashboardViewModel
{
    public int TotalLogs { get; set; }
    public int TotalAnomalies { get; set; }
    public double AnomalyRate { get; set; }
    public List<İşlem> RecentAnomalies { get; set; } = new();
    public int LogsLast24Hours { get; set; }
    public int CriticalAnomalies { get; set; }
    public int HighAnomalies { get; set; }
    public int MediumAnomalies { get; set; }
}
