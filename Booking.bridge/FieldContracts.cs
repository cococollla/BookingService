namespace Booking.bridge;

public sealed record SportsFieldResponse(
    Guid Id,
    string Name,
    string Type,
    decimal HourlyRate,
    int MinBookingHours,
    int MaxBookingHours);

public sealed record TimeSlotResponse(DateTime StartUtc, DateTime EndUtc);
