namespace Booking.domain;

public sealed class SportsField
{
    public SportsField(Guid id, string name, string type, decimal hourlyRate, int minBookingHours, int maxBookingHours)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Идентификатор поля обязателен.", nameof(id));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Название поля обязательно.", nameof(name));

        if (string.IsNullOrWhiteSpace(type))
            throw new ArgumentException("Тип поля обязателен.", nameof(type));

        if (hourlyRate <= 0)
            throw new ArgumentOutOfRangeException(nameof(hourlyRate), "Стоимость должна быть положительной.");

        if (minBookingHours < 1)
            throw new ArgumentOutOfRangeException(nameof(minBookingHours), "Минимальная длительность бронирования — 1 час.");

        if (maxBookingHours < minBookingHours)
            throw new ArgumentOutOfRangeException(nameof(maxBookingHours), "Максимальная длительность должна быть больше минимальной.");

        Id = id;
        Name = name;
        Type = type;
        HourlyRate = hourlyRate;
        MinBookingHours = minBookingHours;
        MaxBookingHours = maxBookingHours;
    }

    public Guid Id { get; }
    public string Name { get; }
    public string Type { get; }
    public decimal HourlyRate { get; }
    public int MinBookingHours { get; }
    public int MaxBookingHours { get; }

    public bool SupportsDuration(int hours) => hours >= MinBookingHours && hours <= MaxBookingHours;
}
