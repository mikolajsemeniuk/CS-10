using server.Entities;
using server.Inputs;
using server.Payloads;

namespace server.Interfaces;

public interface ILogRepository
{
    Task<Log> AddLog(LogInput input);

    Task<IEnumerable<LogPayload>> GetLogsByJobId(Guid jobId, int skip = 10, int take = 10);
}