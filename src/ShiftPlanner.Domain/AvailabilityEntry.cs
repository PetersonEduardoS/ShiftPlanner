using ShiftPlanner.Domain.Enums;

namespace ShiftPlanner.Domain;

public class AvailabilityEntry
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public Guid SchedulingPeriodId { get; set; }
    public DateOnly Date { get; set; }
    public ShiftType Shift { get; set; }
    public bool IsAvailable { get; set; }
}