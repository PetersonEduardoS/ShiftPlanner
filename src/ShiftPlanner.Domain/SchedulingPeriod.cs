using ShiftPlanner.Domain.Enums;

namespace ShiftPlanner.Domain;

public class SchedulingPeriod
{
    public Guid Id { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public SchedulingPeriodStatus Status { get; set; }
}