using System.ComponentModel.DataAnnotations;

namespace TaskSprintTracker.Dtos;

public class ProjectRequest
{
    [Required, StringLength(100, MinimumLength = 1)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }
}

public record ProjectResponse(int Id, string Name, string? Description, DateTime CreatedAt, int TaskCount);

public record ProjectSummary(int ProjectId, int TotalTasks, int CompletedTasks, double TotalEstimatedHours);
