using ShiftPlanner.Domain.Enums;

namespace ShiftPlanner.Application.Validation;

public class OverlapRule : IValidationRule
{
    public IEnumerable<ValidationIssue> Validate(ValidationContext context)
    {
        var newAssignment = context.NewAssignment;
        var newStart = newAssignment.Date.ToDateTime(newAssignment.Shift.GetStartTime());
        var newEnd = newAssignment.Date.ToDateTime(newAssignment.Shift.GetEndTime());

        var hasOverlap = context.ExistingAssignments
            .Where(a => a.EmployeeId == newAssignment.EmployeeId && a.Id != newAssignment.Id)
            .Any(a =>
            {
                var existingStart = a.Date.ToDateTime(a.Shift.GetStartTime());
                var existingEnd = a.Date.ToDateTime(a.Shift.GetEndTime());
                return newStart < existingEnd && existingStart < newEnd;
            });

        if (hasOverlap)
        {
            yield return new ValidationIssue(
                $"Employee already has an overlapping shift assignment on {newAssignment.Date:yyyy-MM-dd}.",
                ValidationSeverity.Error);
        }
    }
}