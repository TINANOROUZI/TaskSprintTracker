using Microsoft.EntityFrameworkCore;
using TaskSprintTracker.Data;
using TaskSprintTracker.Dtos;
using TaskSprintTracker.Models;

namespace TaskSprintTracker.Services;

public class ProjectService(AppDbContext db) : IProjectService
{
    public async Task<IReadOnlyList<ProjectResponse>> GetAllAsync() =>
        await db.Projects.AsNoTracking().OrderBy(p => p.Id)
            .Select(p => new ProjectResponse(p.Id, p.Name, p.Description, p.CreatedAt, p.Tasks.Count))
            .ToListAsync();

    public async Task<ProjectResponse?> GetByIdAsync(int id) =>
        await db.Projects.AsNoTracking().Where(p => p.Id == id)
            .Select(p => new ProjectResponse(p.Id, p.Name, p.Description, p.CreatedAt, p.Tasks.Count))
            .FirstOrDefaultAsync();

    public async Task<ProjectResponse> CreateAsync(ProjectRequest request)
    {
        var project = new Project { Name = request.Name.Trim(), Description = request.Description };
        db.Projects.Add(project);
        await db.SaveChangesAsync();
        return new ProjectResponse(project.Id, project.Name, project.Description, project.CreatedAt, 0);
    }

    public async Task<ProjectResponse?> UpdateAsync(int id, ProjectRequest request)
    {
        var project = await db.Projects.FindAsync(id);
        if (project is null) return null;

        project.Name = request.Name.Trim();
        project.Description = request.Description;
        await db.SaveChangesAsync();

        var count = await db.Tasks.CountAsync(t => t.ProjectId == id);
        return new ProjectResponse(project.Id, project.Name, project.Description, project.CreatedAt, count);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var project = await db.Projects.FindAsync(id);
        if (project is null) return false;

        db.Projects.Remove(project);
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<ProjectSummary?> GetSummaryAsync(int id)
    {
        if (!await db.Projects.AnyAsync(p => p.Id == id)) return null;

        var tasks = db.Tasks.AsNoTracking().Where(t => t.ProjectId == id);
        return new ProjectSummary(
            id,
            await tasks.CountAsync(),
            await tasks.CountAsync(t => t.Status == TaskItemStatus.Done),
            await tasks.SumAsync(t => t.EstimatedHours) ?? 0);
    }
}
