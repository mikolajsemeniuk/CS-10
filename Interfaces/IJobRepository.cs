using server.Entities;
using server.Inputs;
using server.Payloads;

namespace server.Interfaces;

public interface IJobRepository
{
    Task<IEnumerable<Job>> GetJobsToProcess();
    Task<Job> AddJob(JobInput input);
    Task<Job?> RemoveJob(Guid jobId);

    Task<IEnumerable<JobPayload>> GetJobPayloads(int skip = 0, int take = 10);
    Task<JobPayload?> GetJobPayloadByIdWithLogs(Guid jobId, int take = 10);
}