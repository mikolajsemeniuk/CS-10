using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Entities;
using server.Enums;
using server.Inputs;
using server.Interfaces;
using server.Payloads;

namespace server.Repositories;

public class LogRepository : ILogRepository
{
    private readonly DataContext _context;

    public LogRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<LogPayload>> GetLogsByJobId(Guid jobId, int skip = 10, int take = 10)
    {
        // CHECK ME:
        //  * weird query
        //  * skip take 
        //  * skip take with sort
        return await _context.Logs
            .Where(log => log.JobId == jobId)
            // .Skip(skip)
            // .Take(take)
            .Where(log => _context.Logs
                .OrderBy(row => row.LogId)
                .Select(row => row.LogId)
                .Skip(skip)
                .Take(take)
                .Contains(log.LogId))
            .Select(log => new LogPayload(log))
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Log> AddLog(LogInput input)
    {
        var log = new Log(JobStatus.New, input.JobId);
        await _context.Logs.AddAsync(log);
        return log;
    }
}