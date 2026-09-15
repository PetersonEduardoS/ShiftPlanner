namespace ShiftPlanner.Application.Validation;

public interface IValidationRule
{
    IEnumerable<ValidationIssue> Validate(ValidationContext context);
}