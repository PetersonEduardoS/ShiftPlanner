# ShiftPlanner

A shift scheduling app for restaurants and bars, built with **.NET 10**, **Blazor Server**, **EF Core** and **SQL Server**.

Managers build the weekly rota from staff availability, and a validation engine blocks assignments that break scheduling rules, such as double booking someone or leaving less than 11 hours of rest between shifts.

> **Status:** work in progress. The domain model and the first two validation rules are done and unit tested. Next up: the remaining rules, persistence and the Blazor UI. See the [Roadmap](#roadmap).

## Why this project

Before moving into software development, I supervised bar and floor teams in Dublin, and building the weekly rota was part of the job. The hard part was never the hours. It was the rules you cannot break: nobody on two shifts at once, enough rest between a late close and an early start, and no one going over their weekly hours.

ShiftPlanner turns those rules into code that is tested first, so the UI can rely on them instead of re-implementing them.

## Features

| Feature | Status |
|---|---|
| Domain model (employees, stations, shifts, scheduling periods, availability) | ✅ Done |
| Validation: no overlapping shifts for the same employee | ✅ Done |
| Validation: minimum 11 hours of rest between shifts | ✅ Done |
| Validation: employee must be available for the shift | ⏳ Planned |
| Validation: maximum weekly hours per employee | ⏳ Planned |
| Persistence with EF Core and SQL Server | ⏳ Planned |
| Blazor UI: manager rota calendar and employee availability | ⏳ Planned |

## Scheduling model

- **Stations:** `Floor` and `Bar`
- **Shifts:** `Shift1` (11:30 to 19:00) and `Shift2` (17:00 to 23:00)
- **Scheduling period:** a date range for one rota, with status `Open` (being built) or `Published` (shared with staff)
- **Availability:** each employee marks, per date and shift, whether they can work
- **Employee:** belongs to a station and has a configurable `MaxWeeklyHours`

## Validation engine

Every rule implements one small interface, so new rules can be added without touching the existing ones:

```csharp
public interface IValidationRule
{
    IEnumerable<ValidationIssue> Validate(ValidationContext context);
}
```

A rule receives a `ValidationContext` with the new assignment, the employee's existing assignments, their availability and the employee record. It returns zero or more `ValidationIssue`s, each with a message and a severity.

| Rule | What it checks |
|---|---|
| `OverlapRule` | The employee is not already assigned to a shift that overlaps the new one |
| `MinimumRestRule` | At least 11 hours between the end of one shift and the start of the next, before and after the new assignment |

The 11 hour minimum reflects the daily rest period in Ireland's Organisation of Working Time Act 1997 (EU Working Time Directive). The calculation lives in `MinimumRestCalculator`, so it can be tested on its own.

## Architecture

```
src/
  ShiftPlanner.Domain/          Entities and enums. No dependencies.
  ShiftPlanner.Application/     Validation engine and business rules. Depends on Domain.
  ShiftPlanner.Infrastructure/  EF Core and SQL Server (planned). Depends on Domain.
  ShiftPlanner.Web/             Blazor Server UI (interactive server rendering).
tests/
  ShiftPlanner.Application.Tests/   xUnit tests for the validation rules.
```

Business rules live in `Application`, not in the UI or the database layer. That keeps them fast to test and reusable from any front end.

## Tech stack

- .NET 10, C#
- Blazor Server (Razor Components with interactive server rendering)
- Entity Framework Core with SQL Server (planned)
- xUnit for unit tests

## Getting started

**Prerequisites:** [.NET 10 SDK](https://dotnet.microsoft.com/download)

```bash
git clone https://github.com/PetersonEduardoS/ShiftPlanner.git
cd ShiftPlanner

dotnet build
dotnet test
dotnet run --project src/ShiftPlanner.Web
```

## Roadmap

**Version 1 (in progress)**

1. `MaxWeeklyHoursRule` with tests
2. `AvailabilityRule` with tests
3. EF Core with SQL Server: DbContext, migrations and seed data
4. Blazor UI: manager rota calendar and employee availability screen
5. GitHub Actions CI running the test suite on every push

**Version 2 (planned)**

- MCP server built with the official C# SDK (`ModelContextProtocol`), so an AI assistant can query the rota and check it against the same validation engine. Example questions: "Who can cover Saturday night at the bar?" or "Does this week's rota break any rule?"
- Shift swap requests between employees, approved by a manager
- Real-time rota updates with SignalR

## Author

**Peterson Eduardo Sampaio Silva**, Junior .NET / Backend Developer based in Dublin

- LinkedIn: [linkedin.com/in/peterson-eduardo-silva](https://www.linkedin.com/in/peterson-eduardo-silva/)
- GitHub: [github.com/PetersonEduardoS](https://github.com/PetersonEduardoS)
- Portfolio: [petersoneduardos.github.io](https://petersoneduardos.github.io/)
