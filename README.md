# TaskSprintTracker

A small REST API for tracking projects and their tasks, built with ASP.NET Core (.NET 8), EF Core and SQLite. Tasks have a status, priority, due date and an optional estimate in hours, and each project exposes a quick summary endpoint.

## Layout
```
Controllers/   routes and status codes
Services/      business logic behind interfaces
Data/          EF Core DbContext
Models/        entities and enums
Dtos/          request/response types and validation
```

## Run it
```bash
DOTNET_ROLL_FORWARD=Major dotnet run
```
The SQLite file (`tracker.db`) is created on first run, and Swagger is at `/swagger`. `EnsureCreated` doesn't alter existing tables, so delete `tracker.db` if it was created before `EstimatedHours` was added.

## Endpoints
| Method | Route | |
|--------|-------|-|
| GET/POST | `/api/projects` | list / create |
| GET/PUT/DELETE | `/api/projects/{id}` | read / update / delete (cascades to tasks) |
| GET | `/api/projects/{id}/summary` | task count, done count, total estimated hours |
| GET/POST | `/api/projects/{id}/tasks` | list (`?status=&priority=`) / create |
| GET/PUT/DELETE | `/api/tasks/{id}` | read / update / delete |

## Try it
```bash
curl -X POST localhost:5000/api/projects \
  -H "Content-Type: application/json" \
  -d '{"name":"Sprint 1"}'

curl -X POST localhost:5000/api/projects/1/tasks \
  -H "Content-Type: application/json" \
  -d '{"title":"Set up CI","priority":"High","estimatedHours":3.5}'

curl localhost:5000/api/projects/1/summary
# {"projectId":1,"totalTasks":1,"completedTasks":0,"totalEstimatedHours":3.5}
```
Use the port that `dotnet run` prints.
# TaskSprintTracker
