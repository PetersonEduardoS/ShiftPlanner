namespace ShiftPlanner.Application.Validation;

public record ValidationIssue(string Message, ValidationSeverity Severity);