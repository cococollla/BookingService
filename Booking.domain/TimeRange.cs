namespace Booking.domain;

public readonly record struct TimeRange
{
    public DateTime StartUtc { get; }
    public DateTime EndUtc { get; }

    public TimeRange(DateTime startUtc, DateTime endUtc)
    {
        if (startUtc.Kind != DateTimeKind.Utc || endUtc.Kind != DateTimeKind.Utc)
            throw new ArgumentException("Дата и время должны быть в UTC.");

        if (endUtc <= startUtc)
            throw new ArgumentException("Время окончания должно быть больше времени начала.");

        StartUtc = startUtc;
        EndUtc = endUtc;
    }

    public bool Overlaps(TimeRange other)
    {
        return StartUtc < other.EndUtc && other.StartUtc < EndUtc;
    }
}
