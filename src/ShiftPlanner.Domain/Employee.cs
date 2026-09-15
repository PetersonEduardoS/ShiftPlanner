using ShiftPlanner.Domain.Enums;

namespace ShiftPlanner.Domain;

public class Employee
{
    public Guid Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public Station Station { get; set; }
    public int MaxWeeklyHours { get; set; }
}