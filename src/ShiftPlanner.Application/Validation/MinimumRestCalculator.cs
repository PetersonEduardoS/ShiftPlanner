namespace ShiftPlanner.Application.Validation;

public static class MinimumRestCalculator
{
    public static readonly TimeSpan MinimumRest = TimeSpan.FromHours(11);

    public static bool HasSufficientRest(DateTime previousShiftEnd, DateTime nextShiftStart)
        => nextShiftStart - previousShiftEnd >= MinimumRest;
}