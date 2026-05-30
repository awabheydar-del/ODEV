using Microsoft.AspNetCore.Mvc;
using ZeroDayAnomali.Models;
using ZeroDayAnomali.Services;

namespace ZeroDayAnomali.Controllers;

public class HomeController : Controller
{
    private readonly LogService _logService;

    public HomeController(LogService logService)
    {
        _logService = logService;
    }

    public IActionResult Index()
    {
        var allLogs = _logService.GetAll();
        var anomalies = _logService.GetAnomalies();

        var model = new DashboardViewModel
        {
            TotalLogs = _logService.Count,
            TotalAnomalies = _logService.AnomalyCount,
            AnomalyRate = _logService.Count > 0
                ? Math.Round((double)_logService.AnomalyCount / _logService.Count * 100, 2)
                : 0,
            RecentAnomalies = anomalies.Take(10).ToList(),
            LogsLast24Hours = allLogs.Count(l => l.Timestamp >= DateTime.Now.AddHours(-24)),
            CriticalAnomalies = anomalies.Count(a => a.AnomalyScore >= 80),
            HighAnomalies = anomalies.Count(a => a.AnomalyScore >= 60 && a.AnomalyScore < 80),
            MediumAnomalies = anomalies.Count(a => a.AnomalyScore >= 50 && a.AnomalyScore < 60)
        };

        return View(model);
    }
}
