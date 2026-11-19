using Microsoft.AspNetCore.Mvc;
using TaskSprintTracker.Dtos;
using TaskSprintTracker.Services;

namespace TaskSprintTracker.Controllers;

[ApiController]
[Route("api/tasks")]
public class TasksController(ITaskService tasks) : ControllerBase
{
    [HttpGet("{id:int}")]
    public async Task<ActionResult<TaskResponse>> GetById(int id) =>
        await tasks.GetByIdAsync(id) is { } t ? Ok(t) : NotFound();

    [HttpPut("{id:int}")]
    public async Task<ActionResult<TaskResponse>> Update(int id, TaskRequest request) =>
        await tasks.UpdateAsync(id, request) is { } t ? Ok(t) : NotFound();

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id) =>
        await tasks.DeleteAsync(id) ? NoContent() : NotFound();
}
