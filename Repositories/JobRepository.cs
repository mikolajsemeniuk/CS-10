using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Entities;
using server.Enums;
using server.Inputs;
using server.Interfaces;
using server.Payloads;

namespace server.Repositories;

// ===================================================
// 
//  Repositories
// ===================================================
// * Prepare data for Controllers (optimise them and convert to dto)
// * Prepare read for services (in entity model) (based on id or actual object)
// * Preapre write for services (in entity model) (based on id or actual object)
// * Repositories should not use the same method inside them
// * Define methods here if you deal with:
//   - ToList/ToListAsync
//   - Find/FindAsync
//   - Single/SingleAsync
//   - SingleOrDefault/SingleOrDefaultAsync
//   - First/FirstAsync
//   - FirstOrDefault/FirstOrDefaultAsync
//   - Add/AddAsync/context.Entry(model).State = EntityState.Added;
//   - Update/context.Entry(model).State = EntityState.Modified;
//   - Remove
//
// ===================================================

public class JobRepository : IJobRepository
{
    private readonly DataContext _context;

    public JobRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Job>> GetJobsToProcess()
    {
        return await _context.Jobs
                .Where(job => job.Status == JobStatus.New && job.ProcessDate == null ||
                       job.Status == JobStatus.New && job.ProcessDate != null && job.ProcessDate <= DateTime.Now ||
                       job.Status == JobStatus.Failed)
                .ToListAsync();
    }

    public async Task<Job> AddJob(JobInput input)
    {
        var job = new Job(input.Title, input.Description, input.ProcessDate);
        await _context.Jobs.AddAsync(job);
        return job;
    }

    public async Task<Job?> RemoveJob(Guid jobId)
    {
        var job = await _context.Jobs.FindAsync(jobId);
        if (job is null) return job;
        _context.Remove(job);
        return job;
    }

    public async Task<IEnumerable<JobPayload>> GetJobPayloads(int skip = 0, int take = 10)
    {
        // CHECK ME:
        //  * weird query
        //  * skip take 
        //  * skip take with sort
        return await _context.Jobs
            .Skip(skip)
            .Take(take)
            // .Where(job => _context.Jobs
            //     .OrderBy(row => row.JobId)
            //     .Select(row => row.JobId)
            //     .Skip(skip)
            //     .Take(take)
            //     .Contains(job.JobId))
            .Select(job => new JobPayload(job))
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<JobPayload?> GetJobPayloadByIdWithLogs(Guid jobId, int take = 10)
    {
        return await _context.Jobs
            .Where(job => job.JobId == jobId)
            .Include(job => job.Logs.OrderBy(log => log.CreatedAt).Take(take))
            .Select(job => new JobPayload(job))
            .AsNoTrackingWithIdentityResolution()
            .FirstOrDefaultAsync();
    }
}