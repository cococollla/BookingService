using Booking.app;
using Booking.bridge;
using Common.Api.Results;
using Microsoft.AspNetCore.Mvc;
using static Common.Api.ResultsExtensions;

namespace BookingService.Controllers.Booking;

/// <summary>
/// Работа со спортивными площадками и бронированием.
/// </summary>
[ApiController]
[Route("api/booking")]
[Tags("booking")]
public class FieldsController(IBookingService bookingService, IFieldRepository fieldRepository) : ControllerBase
{
    /// <summary>
    /// Получить список доступных площадок.
    /// </summary>
    [HttpGet("fields")]
    public async Task<Ok<IReadOnlyCollection<SportsFieldResponse>>> GetFields(CancellationToken cancellationToken)
    {
        return ok(await bookingService.GetFieldsAsync(cancellationToken));
    }

    /// <summary>
    /// Получить свободные слоты по площадке и дате.
    /// </summary>
    [HttpGet("fields/{fieldId:guid}/slots")]
    public async Task<Results<Ok<IReadOnlyCollection<TimeSlotResponse>>, TNotFound>> GetAvailableSlots(
        Guid fieldId,
        [FromQuery] DateOnly date,
        CancellationToken cancellationToken)
    {
        var field = await fieldRepository.GetByIdAsync(fieldId, cancellationToken);
        if (field is null)
            return notFound("Площадка не найдена.");

        var slots = await bookingService.GetAvailableSlotsAsync(fieldId, date, cancellationToken);
        return ok(slots);
    }

    /// <summary>
    /// Создать бронирование.
    /// </summary>
    [HttpPost("bookings")]
    public async Task<Results<Ok<BookingResponse>, TConflict>> CreateBooking(
        [FromBody] CreateBookingRequest request,
        CancellationToken cancellationToken)
    {
        var result = await bookingService.CreateBookingAsync(request, cancellationToken);
        if (result.Error is not null)
            return conflict(result.Error.Message);

        return ok(result.Booking!);
    }
}
