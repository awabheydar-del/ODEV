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

        var normalLogs = new List<LogEntry>();

        foreach (var (id, name, baseHour, ip) in users)
        {
            for (int day = 0; day < 5; day++)
            {
                for (int i = 0; i < normalActions.Length; i++)
                {
                    normalLogs.Add(new LogEntry
                    {
                        Timestamp = DateTime.Now.AddDays(-day).AddHours(baseHour + i * 2).AddMinutes(rnd.Next(0, 59)),
                        UserId = id,
                        UserName = name,
                        Action = normalActions[i],
                        Resource = resources[i],
                        IPAddress = ip,
                        IsSuccess = true,
                        IsAnomaly = false,
                        AnomalyScore = 0
                    });
                }
            }
        }

        logService.AddRange(normalLogs);

        var anomalyLogs = new List<LogEntry>
        {
            new()
            {
                Timestamp = DateTime.Now.AddMinutes(-30),
                UserId = "user001",
                UserName = "Ahmet Yılmaz",
                Action = "Login",
                Resource = "/dashboard",
                IPAddress = "185.220.101.42",
                IsSuccess = true,
            },
            new()
            {
                Timestamp = DateTime.Now.AddMinutes(-15),
                UserId = "user001",
                UserName = "Ahmet Yılmaz",
                Action = "DeleteFile",
                Resource = "/critical/data.db",
                IPAddress = "192.168.1.100",
                IsSuccess = true,
            },
            new()
            {
                Timestamp = DateTime.Now.AddMinutes(-10),
                UserId = "user002",
                UserName = "Zeynep Kaya",
                Action = "ExecuteCommand",
                Resource = "/bin/sh",
                IPAddress = "91.234.56.78",
                IsSuccess = true,
            },
            new()
            {
                Timestamp = DateTime.Now.AddMinutes(-5),
                UserId = "user003",
                UserName = "Mehmet Demir",
                Action = "Login",
                Resource = "/dashboard",
                IPAddress = "10.0.0.5",
                IsSuccess = true,
            },
            new()
            {
                Timestamp = DateTime.Now.AddMinutes(-1),
                UserId = "user002",
                UserName = "Zeynep Kaya",
                Action = "DownloadFile",
                Resource = "/confidential/musteri_verileri.xlsx",
                IPAddress = "192.168.1.200",
                IsSuccess = true,
            },
        };

        foreach (var log in anomalyLogs)
        {
            anomalyService.AnalyzeAndMark(log);
            logService.Add(log);
        }
    }
}
