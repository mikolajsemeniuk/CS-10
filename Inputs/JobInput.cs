using System.ComponentModel.DataAnnotations;

namespace server.Inputs;

public record struct JobInput(
    [Required]
    [StringLength(50, MinimumLength = 4, ErrorMessage = "maximum {1} and minimum {2} characters allowed")]
    string Title,
    [Required]
    string Description,
    DateTime? ProcessDate
);