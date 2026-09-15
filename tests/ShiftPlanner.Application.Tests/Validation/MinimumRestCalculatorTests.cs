using ShiftPlanner.Application.Validation;
using Xunit;

namespace ShiftPlanner.Application.Tests.Validation;

public class MinimumRestCalculatorTests
{
    [Fact]
    public void HasSufficientRest_WhenGapIsExactlyElevenHours_ReturnsTrue()
    {
        var previousEnd = new DateTime(2026, 9, 22, 23, 0, 0);
        var nextStart = new DateTime(2026, 9, 23, 10, 0, 0);

        var result = MinimumRestCalculator.HasSufficientRest(previousEnd, nextStart);

        Assert.True(result);
    }

    [Fact]
    public void HasSufficientRest_WhenGapIsLessThanElevenHours_ReturnsFalse()
    {
        var previousEnd = new DateTime(2026, 9, 22, 23, 0, 0);
        var nextStart = new DateTime(2026, 9, 23, 9, 0, 0);

        var result = MinimumRestCalculator.HasSufficientRest(previousEnd, nextStart);

        Assert.False(result);
    }

    [Fact]
    public void HasSufficientRest_WhenGapIsMoreThanElevenHours_ReturnsTrue()
    {
        var previousEnd = new DateTime(2026, 9, 22, 23, 0, 0);
        var nextStart = new DateTime(2026, 9, 23, 12, 30, 0);

        var result = MinimumRestCalculator.HasSufficientRest(previousEnd, nextStart);

        Assert.True(result);
    }
}