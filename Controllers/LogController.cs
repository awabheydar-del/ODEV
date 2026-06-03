using Microsoft.AspNetCore.Mvc;
using ZeroDayAnomali.Models;
using ZeroDayAnomali.Services;

namespace ZeroDayAnomali.Controllers;

public class LogController : Controller
{
    private readonly LogService _logService;
    private readonly AnomalyDetectionService _anomalyService;

    public LogController(LogService logService, AnomalyDetectionService anomalyService)
    {
        _logService = logService;
        _anomalyService = anomalyService;
    }

    public IActionResult Index()
    {
        var logs = _logService.GetAll();
        return View(logs);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(İşlem log)
    {
        log.tarih = DateTime.Now;
        _anomalyService.AnalyzeAndMark(log);
        _logService.Add(log);

        if (log.anormal)
        {
            TempData["AnomalyAlert"] = $"ZERO-DAY ANOMALİ TESPİT EDİLDİ! Skor: {log.puan}";
        }
        else
        {
            TempData["Success"] = "Log başarıyla eklendi.";
        }
        return RedirectToAction("Index");
    }
}
