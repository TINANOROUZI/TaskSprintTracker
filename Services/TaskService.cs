using Microsoft.EntityFrameworkCore;
using TaskSprintTracker.Data;
using TaskSprintTracker.Dtos;
using TaskSprintTracker.Models;

namespace TaskSprintTracker.Services;

public class TaskService(AppDbContext db) : ITaskService
{
    public async Task<IReadOnlyList<TaskResponse>?> GetByProjectAsync(
        int projectId, TaskItemStatus? status, TaskPriority? priority)
    {
        if (!await db.Projects.AnyAsync(p => p.Id == projectId)) return null;

        var query = db.Tasks.AsNoTracking().Where(t => t.ProjectId == projectId);
        if (status is not null) query = query.Where(t => t.Status == status);
        if (priority is not null) query = query.Where(t => t.Priority == priority);

        var tasks = await query.OrderBy(t => t.Id).ToListAsync();
        return tasks.Select(ToResponse).ToList();
    }

    public async Task<TaskResponse?> GetByIdAsync(int id)
    {
        var task = await db.Tasks.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
        return task is null ? null : ToResponse(task);
    }

    public async Task<TaskResponse?> CreateAsync(int projectId, TaskRequest request)
    {
        if (!await db.Projects.AnyAsync(p => p.Id == projectId)) return null;

        var task = new TaskItem { ProjectId = projectId };
        Apply(task, request);
        db.Tasks.Add(task);
        await db.SaveChangesAsync();
        return ToResponse(task);
    }

    public async Task<TaskResponse?> UpdateAsync(int id, TaskRequest request)
    {
        var task = await db.Tasks.FindAsync(id);
        if (task is null) return null;

        Apply(task, request);
        await db.SaveChangesAsync();
        return ToResponse(task);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var task = await db.Tasks.FindAsync(id);
        if (task is null) return false;

        db.Tasks.Remove(task);
        await db.SaveChangesAsync();
        return true;
    }

    private static void Apply(TaskItem task, TaskRequest r)
    {
        task.Title = r.Title.Trim();
        task.Description = r.Description;
        task.Status = r.Status;
        task.Priority = r.Priority;
        task.DueDate = r.DueDate;
        task.EstimatedHours = r.EstimatedHours;
    }

    private static TaskResponse ToResponse(TaskItem t) =>
        new(t.Id, t.Title, t.Description, t.Status, t.Priority, t.DueDate, t.EstimatedHours, t.ProjectId);
}
