using server.Enums;

namespace server.Entities;

public class Log
{
    public Guid LogId { get; set; } = Guid.NewGuid();
    public JobStatus Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public Guid JobId { get; set; }
    public virtual Job Job { get; set; } = null!;

    public Log(JobStatus description, Guid jobId)
    {
        Description = description;
        JobId = jobId;
    }
}