using ShiftPlanner.Domain.Enums;

namespace ShiftPlanner.Application.Validation;

public class MinimumRestRule : IValidationRule
{
    public IEnumerable<ValidationIssue> Validate(ValidationContext context)
    {
        var newAssignment = context.NewAssignment;
        var newStart = newAssignment.Date.ToDateTime(newAssignment.Shift.GetStartTime());
        var newEnd = newAssignment.Date.ToDateTime(newAssignment.Shift.GetEndTime());

        var sameEmployeeShifts = context.ExistingAssignments
            .Where(a => a.EmployeeId == newAssignment.EmployeeId && a.Id != newAssignment.Id)
            .Select(a => (
                Start: a.Date.ToDateTime(a.Shift.GetStartTime()),
                End: a.Date.ToDateTime(a.Shift.GetEndTime())));

        foreach (var (start, end) in sameEmployeeShifts)
        {
            if (end <= newStart && !MinimumRestCalculator.HasSufficientRest(end, newStart))
            {
                yield return new ValidationIssue(
                    $"Less than 11 hours of rest before the shift on {newAssignment.Date:yyyy-MM-dd}.",
                    ValidationSeverity.Error);
            }

            if (start >= newEnd && !MinimumRestCalculator.HasSufficientRest(newEnd, start))
            {
                yield return new ValidationIssue(
                    $"Less than 11 hours of rest after the shift on {newAssignment.Date:yyyy-MM-dd}.",
                    ValidationSeverity.Error);
            }
        }
    }
}