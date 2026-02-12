using Booking.domain;

namespace Booking.domain.tests;

public class TimeRangeTests
{
    [Fact]
    public void Overlaps_ReturnsTrue_WhenRangesIntersect()
    {
        var left = new TimeRange(
            new DateTime(2026, 1, 10, 10, 0, 0, DateTimeKind.Utc),
            new DateTime(2026, 1, 10, 12, 0, 0, DateTimeKind.Utc));

        var right = new TimeRange(
            new DateTime(2026, 1, 10, 11, 0, 0, DateTimeKind.Utc),
            new DateTime(2026, 1, 10, 13, 0, 0, DateTimeKind.Utc));

        Assert.True(left.Overlaps(right));
    }

    [Fact]
    public void Overlaps_ReturnsFalse_WhenRangesTouchByBoundary()
    {
        var left = new TimeRange(
            new DateTime(2026, 1, 10, 10, 0, 0, DateTimeKind.Utc),
            new DateTime(2026, 1, 10, 11, 0, 0, DateTimeKind.Utc));

        var right = new TimeRange(
            new DateTime(2026, 1, 10, 11, 0, 0, DateTimeKind.Utc),
            new DateTime(2026, 1, 10, 12, 0, 0, DateTimeKind.Utc));

        Assert.False(left.Overlaps(right));
    }
}
