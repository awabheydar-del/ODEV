using ZeroDayAnomali.Models;

namespace ZeroDayAnomali.Services;

public class LogService
{
    private readonly List<LogEntry> _logs = new();
    private int _nextId = 1;

    public List<LogEntry> GetAll() => _logs.OrderByDescending(l => l.Timestamp).ToList();

    public LogEntry? GetById(int id) => _logs.FirstOrDefault(l => l.Id == id);

    public void Add(LogEntry log)
    {
        log.Id = _nextId++;
        _logs.Add(log);
    }

    public void AddRange(IEnumerable<LogEntry> logs)
    {
        foreach (var log in logs)
        {
            log.Id = _nextId++;
            _logs.Add(log);
        }
    }

    public List<LogEntry> GetAnomalies() =>
        _logs.Where(l => l.IsAnomaly).OrderByDescending(l => l.Timestamp).ToList();

    public int Count => _logs.Count;
    public int AnomalyCount => _logs.Count(l => l.IsAnomaly);
}
