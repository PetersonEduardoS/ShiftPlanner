using ShiftPlanner.Domain.Enums;

namespace ShiftPlanner.Application.Validation;

public class WeeklyHoursRule : IValidationRule
{
    public IEnumerable<ValidationIssue> Validate(ValidationContext context)
    {
        var newAssignment = context.NewAssignment;
        var weekStart = GetWeekStart(newAssignment.Date);
        var weekEnd = weekStart.AddDays(6);

        var hoursAlreadyAssigned = context.ExistingAssignments
            .Where(a => a.EmployeeId == newAssignment.EmployeeId && a.Id != newAssignment.Id)
            .Where(a => a.Date >= weekStart && a.Date <= weekEnd)
            .Sum(a => GetShiftDuration(a.Shift).TotalHours);

        var totalHours = hoursAlreadyAssigned + GetShiftDuration(newAssignment.Shift).TotalHours;

        if (totalHours > context.Employee.MaxWeeklyHours)
        {
            yield return new ValidationIssue(
                $"Assigning this shift would bring the employee to {totalHours}h this week, above the {context.Employee.MaxWeeklyHours}h limit.",
                ValidationSeverity.Error);
        }
    }

    private static DateOnly GetWeekStart(DateOnly date)
    {
        var daysSinceMonday = ((int)date.DayOfWeek - (int)DayOfWeek.Monday + 7) % 7;
        return date.AddDays(-daysSinceMonday);
    }

    private static TimeSpan GetShiftDuration(ShiftType shift) => shift.GetEndTime() - shift.GetStartTime();
}