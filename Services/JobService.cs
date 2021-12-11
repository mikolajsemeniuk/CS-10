using server.Data;
using server.Entities;
using server.Enums;
using server.Inputs;
using server.Interfaces;
using server.Repositories;

namespace server.Services;

// ===================================================
// 
//  Services
// ===================================================
// * Combine repositories logic
// * if you don't have to use `Repositories method` change object here
// * Services should return entities
// * Services should modify existing entities
// * Services methods should based on id or actual object
// * Services should not use the same method inside them
// * Operations which require SaveChanges cannot be concatenate together
//
// ===================================================

public class JobService : IJobService
{
    private readonly DataContext _context;

    public JobService(DataContext context) => 
        _context = context;

    public IJobRepository jobRepository => 
        new JobRepository(_context);

    public ILogRepository logRepository => 
        new LogRepository(_context);

    public async Task<bool> Complete()
    {
        return await _context.SaveChangesAsync() > 0;
    }

    public bool HasChange()
    {
        return _context.ChangeTracker.HasChanges();
    }

    public async Task<Job> AddJob(JobInput input)
    {
        var job = await jobRepository.AddJob(input);
        var log = await logRepository.AddLog(new LogInput(JobStatus.New, job.JobId));
        job.Logs = new List<Log> { log };
        return job;
    }

    public async Task<IEnumerable<Job>> ProcessJobs()
    {
        var jobsToProcess = await jobRepository.GetJobsToProcess();
        jobsToProcess.ToList().ForEach(async job =>
        {
            (job.Status, job.UpdatedAt) = (JobStatus.InProgress, DateTime.Now);
            await logRepository.AddLog(new LogInput(JobStatus.InProgress, job.JobId));
        });
        return jobsToProcess;
    }

    public async Task CompleteJobs(IEnumerable<Job> jobsInProgress)
    {
        var jobs = jobsInProgress.Select(async job =>
        {
            if (WasJobSuccessful(job))
            {
                job.Status = await ChangeStatus(JobStatus.Done, job.JobId);
            }
            else
            {
                job.FailureCounter += 1;
                if (job.FailureCounter == 5)
                {
                    job.Status = await ChangeStatus(JobStatus.Closed, job.JobId);
                }
                else
                {
                    job.Status = await ChangeStatus(JobStatus.Failed, job.JobId);
                }
            }
            job.UpdatedAt = DateTime.Now;
        });
        await Task.WhenAll(jobs);
    }

    private async Task<JobStatus> ChangeStatus(JobStatus status, Guid jobId)
    {
        await logRepository.AddLog(new LogInput(status, jobId));
        return status;
    }

    private bool WasJobSuccessful(Job job)
    {
        Random random = new Random();
        // await Task.Delay(1000);
        return random.Next(10) < 4 ? false : true;
    }
}