using ShiftPlanner.Domain;

namespace ShiftPlanner.Application.Validation;

public class ValidationContext
{
    public required ShiftAssignment NewAssignment { get; init; }
    public required IReadOnlyList<ShiftAssignment> ExistingAssignments { get; init; }
    public required IReadOnlyList<AvailabilityEntry> Availability { get; init; }
    public required Employee Employee { get; init; }
}