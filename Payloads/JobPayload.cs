using server.Entities;
using server.Enums;

namespace server.Payloads;

public record JobPayload
{
    public Guid JobId { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
    public DateTime? ProcessDate { get; init; }
    public string Status { get; init; }
    public long FailureCounter { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
    public List<LogPayload> Logs { get; init; }

    public JobPayload(Job job)
    {
        JobId = job.JobId;
        Title = job.Title;
        Description = job.Description;
        ProcessDate = job.ProcessDate;
        Status = job.Status.ToString();
        FailureCounter = job.FailureCounter;
        CreatedAt = job.CreatedAt;
        UpdatedAt = job.UpdatedAt;
        Logs = job.Logs?.Select(log => new LogPayload(log)).ToList() ?? new List<LogPayload>();
    }
}