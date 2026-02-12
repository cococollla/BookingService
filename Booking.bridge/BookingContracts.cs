namespace Booking.bridge;

public sealed record CreateBookingRequest(
    Guid FieldId,
    DateTime StartUtc,
    int Hours,
    string CustomerName);

public sealed record BookingResponse(
    Guid Id,
    Guid FieldId,
    string CustomerName,
    DateTime StartUtc,
    DateTime EndUtc,
    int Hours,
    decimal TotalPrice);

public sealed record BookingValidationError(string Code, string Message);
