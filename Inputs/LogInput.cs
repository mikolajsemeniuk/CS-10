using System.ComponentModel.DataAnnotations;
using server.Enums;

namespace server.Inputs;

public record struct LogInput(
    [Required]
    JobStatus Description,
    [Required]
    Guid JobId
);