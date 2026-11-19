using System.ComponentModel.DataAnnotations;
using TaskSprintTracker.Models;

namespace TaskSprintTracker.Dtos;

public class TaskRequest
{
    [Required, StringLength(200, MinimumLength = 1)]
    public string Title { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Description { get; set; }

    [EnumDataType(typeof(TaskItemStatus))]
    public TaskItemStatus Status { get; set; } = TaskItemStatus.Todo;

    [EnumDataType(typeof(TaskPriority))]
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;

    public DateTime? DueDate { get; set; }

    [Range(0, 1000)]
    public double? EstimatedHours { get; set; }
}

public record TaskResponse(
    int Id, string Title, string? Description,
    TaskItemStatus Status, TaskPriority Priority,
    DateTime? DueDate, double? EstimatedHours, int ProjectId);
