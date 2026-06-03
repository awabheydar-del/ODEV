using ZeroDayAnomali.Models;

namespace ZeroDayAnomali.Services;

public static class SampleData
{
    public static void Seed(LogService logService, AnomalyDetectionService anomalyService)
    {
        var rnd = new Random(42);

        var users = new[]
        {
            (id: "user001", name: "Ahmet Yılmaz", baseHour: 8, ip: "192.168.1.100"),
            (id: "user002", name: "Zeynep Kaya", baseHour: 9, ip: "192.168.1.101"),
            (id: "user003", name: "Mehmet Demir", baseHour: 10, ip: "192.168.1.102"),
        };

        var normalActions = new[] { "Login", "ViewFile", "EditFile", "ViewFile", "Logout" };
        var resources = new[] { "/dashboard", "/documents/report.pdf", "/projects/plan.docx", "/data/analytics.xlsx", "/dashboard" };

        var normalLogs = new List<İşlem>();

        foreach (var (id, name, baseHour, ip) in users)
        {
            for (int day = 0; day < 5; day++)
            {
                for (int i = 0; i < normalActions.Length; i++)
                {
                    normalLogs.Add(new İşlem
                    {
                        tarih = DateTime.Now.AddDays(-day).AddHours(baseHour + i * 2).AddMinutes(rnd.Next(0, 59)),
                        kul_id = id,
                        kul_adı = name,
                        işlem = normalActions[i],
                        kaynak = resources[i],
                        ip = ip,
                        başarılı = true,
                        anormal = false,
                        puan = 0
                    });
                }
            }
        }

        logService.AddRange(normalLogs);

        var anomalyLogs = new List<İşlem>
        {
            new()
            {
                tarih = DateTime.Now.AddMinutes(-30),
                kul_id = "user001",
                kul_adı = "Ahmet Yılmaz",
                işlem = "Login",
                kaynak = "/dashboard",
                ip = "185.220.101.42",
                başarılı = true,
            },
            new()
            {
                tarih = DateTime.Now.AddMinutes(-15),
                kul_id = "user001",
                kul_adı = "Ahmet Yılmaz",
                işlem = "DeleteFile",
                kaynak = "/critical/data.db",
                ip = "192.168.1.100",
                başarılı = true,
            },
            new()
            {
                tarih = DateTime.Now.AddMinutes(-10),
                kul_id = "user002",
                kul_adı = "Zeynep Kaya",
                işlem = "ExecuteCommand",
                kaynak = "/bin/sh",
                ip = "91.234.56.78",
                başarılı = true,
            },
            new()
            {
                tarih = DateTime.Now.AddMinutes(-5),
                kul_id = "user003",
                kul_adı = "Mehmet Demir",
                işlem = "Login",
                kaynak = "/dashboard",
                ip = "10.0.0.5",
                başarılı = true,
            },
            new()
            {
                tarih = DateTime.Now.AddMinutes(-1),
                kul_id = "user002",
                kul_adı = "Zeynep Kaya",
                işlem = "DownloadFile",
                kaynak = "/confidential/musteri_verileri.xlsx",
                ip = "192.168.1.200",
                başarılı = true,
            },
        };

        foreach (var log in anomalyLogs)
        {
            anomalyService.AnalyzeAndMark(log);
            logService.Add(log);
        }
    }
}
