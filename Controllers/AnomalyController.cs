using Microsoft.AspNetCore.Mvc;
using ZeroDayAnomali.Services;

namespace ZeroDayAnomali.Controllers;

public class AnomalyController : Controller
{
    private readonly LogService _logService;

    public AnomalyController(LogService logService)
    {
        _logService = logService;
    }

    public IActionResult Index()
    {
        var anomalies = _logService.GetAnomalies();
        return View(anomalies);
    }

    public IActionResult Details(int id)
    {
        var log = _logService.GetById(id);
        if (log == null || !log.anormal)
            return NotFound();
        return View(log);
    }
}
