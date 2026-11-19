using TaskSprintTracker.Dtos;

namespace TaskSprintTracker.Services;

public interface IProjectService
{
    Task<IReadOnlyList<ProjectResponse>> GetAllAsync();
    Task<ProjectResponse?> GetByIdAsync(int id);
    Task<ProjectResponse> CreateAsync(ProjectRequest request);
    Task<ProjectResponse?> UpdateAsync(int id, ProjectRequest request);
    Task<bool> DeleteAsync(int id);
    Task<ProjectSummary?> GetSummaryAsync(int id);
}
