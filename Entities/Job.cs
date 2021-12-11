using server.Enums;

namespace server.Entities;

public class Job
{
    public Guid JobId { get; set; } = Guid.NewGuid();
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime? ProcessDate { get; set; }
    public JobStatus Status { get; set; } = JobStatus.New;
    public long FailureCounter { get; set; } = 0;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }
    public virtual IEnumerable<Log> Logs { get; set; } = null!;

    public Job(string title, string description, DateTime? processDate = null)
    {
        Title = title;
        Description = description;
        ProcessDate = processDate;
    }
}