using ZeroDayAnomali.Models;

namespace ZeroDayAnomali.Services;

public class LogService
{
    private readonly List<İşlem> _logs = new();
    private int _nextId = 1;

    public List<İşlem> GetAll() => _logs.OrderByDescending(l => l.tarih).ToList();

    public İşlem? GetById(int id) => _logs.FirstOrDefault(l => l.işlem_id == id);

    public void Add(İşlem log)
    {
        log.işlem_id = _nextId++;
        _logs.Add(log);
    }

    public void AddRange(IEnumerable<İşlem> logs)
    {
        foreach (var log in logs)
        {
            log.işlem_id = _nextId++;
            _logs.Add(log);
        }
    }

    public List<İşlem> GetAnomalies() =>
        _logs.Where(l => l.anormal).OrderByDescending(l => l.tarih).ToList();

    public int Count => _logs.Count;
    public int AnomalyCount => _logs.Count(l => l.anormal);
}
