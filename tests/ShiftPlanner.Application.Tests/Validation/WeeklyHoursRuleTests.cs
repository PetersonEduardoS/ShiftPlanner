using ShiftPlanner.Application.Validation;
using ShiftPlanner.Domain;
using ShiftPlanner.Domain.Enums;
using Xunit;

namespace ShiftPlanner.Application.Tests.Validation;

public class WeeklyHoursRuleTests
{
    private readonly WeeklyHoursRule _rule = new();

    private static Employee CreateEmployee(Guid id, int maxWeeklyHours) => new()
    {
        Id = id,
        FirstName = "Ana",
        LastName = "Silva",
        Station = Station.Floor,
        MaxWeeklyHours = maxWeeklyHours
    };

    [Fact]
    public void Validate_WhenNewAssignmentKeepsWithinWeeklyLimit_ReturnsNoIssues()
    {
        var employeeId = Guid.NewGuid();
        var periodId = Guid.NewGuid();

        var existingAssignment = new ShiftAssignment
        {
            Id = Guid.NewGuid(),
            SchedulingPeriodId = periodId,
            Date = new DateOnly(2026, 9, 21),
            Shift = ShiftType.Shift1,
            Station = Station.Floor,
            EmployeeId = employeeId
        };

        var newAssignment = new ShiftAssignment
        {
            Id = Guid.NewGuid(),
            SchedulingPeriodId = periodId,
            Date = new DateOnly(2026, 9, 22),
            Shift = ShiftType.Shift2,
            Station = Station.Bar,
            EmployeeId = employeeId
        };

        var context = new ValidationContext
        {
            NewAssignment = newAssignment,
            ExistingAssignments = new List<ShiftAssignment> { existingAssignment },
            Availability = new List<AvailabilityEntry>(),
            Employee = CreateEmployee(employeeId, maxWeeklyHours: 40)
        };

        var issues = _rule.Validate(context).ToList();

        Assert.Empty(issues);
    }

    [Fact]
    public void Validate_WhenNewAssignmentExceedsWeeklyLimit_ReturnsError()
    {
        var employeeId = Guid.NewGuid();
        var periodId = Guid.NewGuid();

        var existingAssignment = new ShiftAssignment
        {
            Id = Guid.NewGuid(),
            SchedulingPeriodId = periodId,
            Date = new DateOnly(2026, 9, 21),
            Shift = ShiftType.Shift1,
            Station = Station.Floor,
            EmployeeId = employeeId
        };

        var newAssignment = new ShiftAssignment
        {
            Id = Guid.NewGuid(),
            SchedulingPeriodId = periodId,
            Date = new DateOnly(2026, 9, 23),
            Shift = ShiftType.Shift1,
            Station = Station.Floor,
            EmployeeId = employeeId
        };

        var context = new ValidationContext
        {
            NewAssignment = newAssignment,
            ExistingAssignments = new List<ShiftAssignment> { existingAssignment },
            Availability = new List<AvailabilityEntry>(),
            Employee = CreateEmployee(employeeId, maxWeeklyHours: 10)
        };

        var issues = _rule.Validate(context).ToList();

        Assert.Single(issues);
        Assert.Equal(ValidationSeverity.Error, issues[0].Severity);
    }

    [Fact]
    public void Validate_WhenExistingAssignmentIsInDifferentWeek_IsNotCounted()
    {
        var employeeId = Guid.NewGuid();
        var periodId = Guid.NewGuid();

        var existingAssignment = new ShiftAssignment
        {
            Id = Guid.NewGuid(),
            SchedulingPeriodId = periodId,
            Date = new DateOnly(2026, 9, 20),
            Shift = ShiftType.Shift1,
            Station = Station.Floor,
            EmployeeId = employeeId
        };

        var newAssignment = new ShiftAssignment
        {
            Id = Guid.NewGuid(),
            SchedulingPeriodId = periodId,
            Date = new DateOnly(2026, 9, 21),
            Shift = ShiftType.Shift1,
            Station = Station.Floor,
            EmployeeId = employeeId
        };

        var context = new ValidationContext
        {
            NewAssignment = newAssignment,
            ExistingAssignments = new List<ShiftAssignment> { existingAssignment },
            Availability = new List<AvailabilityEntry>(),
            Employee = CreateEmployee(employeeId, maxWeeklyHours: 10)
        };

        var issues = _rule.Validate(context).ToList();

        Assert.Empty(issues);
    }

    [Fact]
    public void Validate_WhenExistingAssignmentBelongsToDifferentEmployee_ReturnsNoIssues()
    {
        var periodId = Guid.NewGuid();

        var existingAssignment = new ShiftAssignment
        {
            Id = Guid.NewGuid(),
            SchedulingPeriodId = periodId,
            Date = new DateOnly(2026, 9, 21),
            Shift = ShiftType.Shift1,
            Station = Station.Floor,
            EmployeeId = Guid.NewGuid()
        };

        var currentEmployeeId = Guid.NewGuid();

        var newAssignment = new ShiftAssignment
        {
            Id = Guid.NewGuid(),
            SchedulingPeriodId = periodId,
            Date = new DateOnly(2026, 9, 22),
            Shift = ShiftType.Shift1,
            Station = Station.Floor,
            EmployeeId = currentEmployeeId
        };

        var context = new ValidationContext
        {
            NewAssignment = newAssignment,
            ExistingAssignments = new List<ShiftAssignment> { existingAssignment },
            Availability = new List<AvailabilityEntry>(),
            Employee = CreateEmployee(currentEmployeeId, maxWeeklyHours: 10)
        };

        var issues = _rule.Validate(context).ToList();

        Assert.Empty(issues);
    }
}