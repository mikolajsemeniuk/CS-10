namespace server.Enums;

[Flags]
public enum JobStatus
{
    New = 0,
    InProgress = 1,
    Failed = 2,
    Done = 4,
    Closed = 8
}