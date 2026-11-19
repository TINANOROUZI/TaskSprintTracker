using Microsoft.AspNetCore.Mvc;
using TaskSprintTracker.Dtos;
using TaskSprintTracker.Services;

namespace TaskSprintTracker.Controllers;

[ApiController]
[Route("api/projects")]
public class ProjectsController(IProjectService projects, ITaskService tasks) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ProjectResponse>>> GetAll() =>
        Ok(await projects.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProjectResponse>> GetById(int id) =>
        await projects.GetByIdAsync(id) is { } p ? Ok(p) : NotFound();

    [HttpPost]
    public async Task<ActionResult<ProjectResponse>> Create(ProjectRequest request)
    {
        var created = await projects.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ProjectResponse>> Update(int id, ProjectRequest request) =>
        await projects.UpdateAsync(id, request) is { } p ? Ok(p) : NotFound();

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id) =>
        await projects.DeleteAsync(id) ? NoContent() : NotFound();

    [HttpGet("{id:int}/summary")]
    public async Task<ActionResult<ProjectSummary>> GetSummary(int id) =>
        await projects.GetSummaryAsync(id) is { } s ? Ok(s) : NotFound();

    [HttpGet("{projectId:int}/tasks")]
    public async Task<ActionResult<IReadOnlyList<TaskResponse>>> GetTasks(
        int projectId, [FromQuery] Models.TaskItemStatus? status, [FromQuery] Models.TaskPriority? priority) =>
        await tasks.GetByProjectAsync(projectId, status, priority) is { } list ? Ok(list) : NotFound();

    [HttpPost("{projectId:int}/tasks")]
    public async Task<ActionResult<TaskResponse>> CreateTask(int projectId, TaskRequest request)
    {
        var created = await tasks.CreateAsync(projectId, request);
        return created is null
            ? NotFound()
            : CreatedAtAction(nameof(TasksController.GetById), "Tasks", new { id = created.Id }, created);
    }
}
