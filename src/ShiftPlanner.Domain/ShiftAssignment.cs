using ShiftPlanner.Domain.Enums;

namespace ShiftPlanner.Domain;

public class ShiftAssignment
{
    public Guid Id { get; set; }
    public Guid SchedulingPeriodId { get; set; }
    public DateOnly Date { get; set; }
    public ShiftType Shift { get; set; }
    public Station Station { get; set; }
    public Guid EmployeeId { get; set; }
}