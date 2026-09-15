using ShiftPlanner.Application.Validation;
using ShiftPlanner.Domain;
using ShiftPlanner.Domain.Enums;
using Xunit;

namespace ShiftPlanner.Application.Tests.Validation;

public class OverlapRuleTests
{
    private readonly OverlapRule _rule = new();

    [Fact]
    public void Validate_WhenNewAssignmentOverlapsExistingShiftSameDay_ReturnsError()
    {
        var employeeId = Guid.NewGuid();
        var periodId = Guid.NewGuid();
        var date = new DateOnly(2026, 9, 22);

        var existingAssignment = new ShiftAssignment
        {
            Id = Guid.NewGuid(),
            SchedulingPeriodId = periodId,
            Date = date,
            Shift = ShiftType.Shift1,
            Station = Station.Floor,
            EmployeeId = employeeId
        };

        var newAssignment = new ShiftAssignment
        {
            Id = Guid.NewGuid(),
            SchedulingPeriodId = periodId,
            Date = date,
            Shift = ShiftType.Shift2,
            Station = Station.Bar,
            EmployeeId = employeeId
        };

        var context = new ValidationContext
        {
            NewAssignment = newAssignment,
            ExistingAssignments = new List<ShiftAssignment> { existingAssignment },
            Availability = new List<AvailabilityEntry>(),
            Employee = new Employee
            {
                Id = employeeId,
                FirstName = "Ana",
                LastName = "Silva",
                Station = Station.Floor,
                MaxWeeklyHours = 40
            }
        };

        var issues = _rule.Validate(context).ToList();

        Assert.Single(issues);
        Assert.Equal(ValidationSeverity.Error, issues[0].Severity);
    }

    [Fact]
    public void Validate_WhenExistingAssignmentIsOnDifferentDay_ReturnsNoIssues()
    {
        var employeeId = Guid.NewGuid();
        var periodId = Guid.NewGuid();

        var existingAssignment = new ShiftAssignment
        {
            Id = Guid.NewGuid(),
            SchedulingPeriodId = periodId,
            Date = new DateOnly(2026, 9, 22),
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
            Employee = new Employee
            {
                Id = employeeId,
                FirstName = "Ana",
                LastName = "Silva",
                Station = Station.Floor,
                MaxWeeklyHours = 40
            }
        };

        var issues = _rule.Validate(context).ToList();

        Assert.Empty(issues);
    }

    [Fact]
    public void Validate_WhenExistingAssignmentBelongsToDifferentEmployee_ReturnsNoIssues()
    {
        var periodId = Guid.NewGuid();
        var date = new DateOnly(2026, 9, 22);

        var existingAssignment = new ShiftAssignment
        {
            Id = Guid.NewGuid(),
            SchedulingPeriodId = periodId,
            Date = date,
            Shift = ShiftType.Shift1,
            Station = Station.Floor,
            EmployeeId = Guid.NewGuid()
        };

        var currentEmployeeId = Guid.NewGuid();

        var newAssignment = new ShiftAssignment
        {
            Id = Guid.NewGuid(),
            SchedulingPeriodId = periodId,
            Date = date,
            Shift = ShiftType.Shift2,
            Station = Station.Bar,
            EmployeeId = currentEmployeeId
        };

        var context = new ValidationContext
        {
            NewAssignment = newAssignment,
            ExistingAssignments = new List<ShiftAssignment> { existingAssignment },
            Availability = new List<AvailabilityEntry>(),
            Employee = new Employee
            {
                Id = currentEmployeeId,
                FirstName = "Bruno",
                LastName = "Costa",
                Station = Station.Bar,
                MaxWeeklyHours = 40
            }
        };

        var issues = _rule.Validate(context).ToList();

        Assert.Empty(issues);
    }
}