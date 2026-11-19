using TaskSprintTracker.Dtos;
using TaskSprintTracker.Models;

namespace TaskSprintTracker.Services;

public interface ITaskService
{
    Task<IReadOnlyList<TaskResponse>?> GetByProjectAsync(int projectId, TaskItemStatus? status, TaskPriority? priority);
    Task<TaskResponse?> GetByIdAsync(int id);
    Task<TaskResponse?> CreateAsync(int projectId, TaskRequest request);
    Task<TaskResponse?> UpdateAsync(int id, TaskRequest request);
    Task<bool> DeleteAsync(int id);
}
