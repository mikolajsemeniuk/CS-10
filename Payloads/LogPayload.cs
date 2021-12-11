using System.Text.Json.Serialization;
using server.Entities;
using server.Enums;

namespace server.Payloads;

public record LogPayload
{
    public Guid LogId { get; init; }
    public string Description { get; init; }
    public DateTime CreatedAt { get; init; }
    [JsonIgnore]
    public Guid JobId { get; init; }

    public LogPayload(Log log)
    {
        LogId = log.LogId;
        Description = log.Description.ToString();
        CreatedAt = log.CreatedAt;
        JobId = log.JobId;
    }
}