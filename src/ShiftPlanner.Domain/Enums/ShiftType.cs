namespace ShiftPlanner.Domain.Enums;

public enum ShiftType
{
    Shift1,
    Shift2
}

public static class ShiftTypeExtensions
{
    public static TimeOnly GetStartTime(this ShiftType shiftType) => shiftType switch
    {
        ShiftType.Shift1 => new TimeOnly(11, 30),
        ShiftType.Shift2 => new TimeOnly(17, 0),
        _ => throw new ArgumentOutOfRangeException(nameof(shiftType))
    };

    public static TimeOnly GetEndTime(this ShiftType shiftType) => shiftType switch
    {
        ShiftType.Shift1 => new TimeOnly(19, 0),
        ShiftType.Shift2 => new TimeOnly(23, 0),
        _ => throw new ArgumentOutOfRangeException(nameof(shiftType))
    };
}