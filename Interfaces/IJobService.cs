using server.Entities;
using server.Inputs;

namespace server.Interfaces;

public interface IJobService
{
    IJobRepository jobRepository { get; }
    ILogRepository logRepository { get; }
    Task<bool> Complete();
    bool HasChange();

    Task<Job> AddJob(JobInput input);

    Task<IEnumerable<Job>> ProcessJobs();

    Task CompleteJobs(IEnumerable<Job> jobsInProgress);
}