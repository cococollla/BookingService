using Booking.bridge;
using Booking.domain;

namespace Booking.app;

public sealed class BookingService(IFieldRepository fieldRepository, IBookingRepository bookingRepository) : IBookingService
{
    private static readonly TimeSpan WorkStart = TimeSpan.FromHours(6);
    private static readonly TimeSpan WorkEnd = TimeSpan.FromHours(23);

    public async Task<IReadOnlyCollection<SportsFieldResponse>> GetFieldsAsync(CancellationToken cancellationToken)
    {
        var fields = await fieldRepository.GetAllAsync(cancellationToken);

        return fields
            .Select(x => new SportsFieldResponse(x.Id, x.Name, x.Type, x.HourlyRate, x.MinBookingHours, x.MaxBookingHours))
            .ToArray();
    }

    public async Task<IReadOnlyCollection<TimeSlotResponse>> GetAvailableSlotsAsync(Guid fieldId, DateOnly date, CancellationToken cancellationToken)
    {
        var field = await fieldRepository.GetByIdAsync(fieldId, cancellationToken);
        if (field is null)
            return [];

        var bookings = await bookingRepository.GetByFieldAsync(fieldId, cancellationToken);

        var dayStart = date.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
        var available = new List<TimeSlotResponse>();

        for (var hour = WorkStart.Hours; hour < WorkEnd.Hours; hour++)
        {
            var slotStart = dayStart.AddHours(hour);
            var slotEnd = slotStart.AddHours(1);
            var range = new TimeRange(slotStart, slotEnd);

            if (BookingDomainService.HasConflicts(bookings, range))
                continue;

            available.Add(new TimeSlotResponse(slotStart, slotEnd));
        }

        return available;
    }

    public async Task<(BookingResponse? Booking, BookingValidationError? Error)> CreateBookingAsync(CreateBookingRequest request, CancellationToken cancellationToken)
    {
        if (request.FieldId == Guid.Empty)
            return (null, new BookingValidationError("field.required", "Поле обязательно для бронирования."));

        if (request.Hours < 1)
            return (null, new BookingValidationError("hours.invalid", "Минимальная длительность аренды — 1 час."));

        if (request.StartUtc.Kind != DateTimeKind.Utc)
            return (null, new BookingValidationError("start.invalid", "Дата бронирования должна быть в UTC."));

        if (string.IsNullOrWhiteSpace(request.CustomerName))
            return (null, new BookingValidationError("customer.required", "Укажите имя клиента."));

        var field = await fieldRepository.GetByIdAsync(request.FieldId, cancellationToken);
        if (field is null)
            return (null, new BookingValidationError("field.not_found", "Выбранная площадка не найдена."));

        if (!field.SupportsDuration(request.Hours))
            return (null, new BookingValidationError("hours.not_supported", $"Для площадки доступно бронирование от {field.MinBookingHours} до {field.MaxBookingHours} часов."));

        var end = request.StartUtc.AddHours(request.Hours);

        if (request.StartUtc.TimeOfDay < WorkStart || end.TimeOfDay > WorkEnd)
            return (null, new BookingValidationError("time.out_of_range", "Бронирование доступно с 06:00 до 23:00 UTC."));

        var requestedRange = new TimeRange(request.StartUtc, end);
        var bookings = await bookingRepository.GetByFieldAsync(request.FieldId, cancellationToken);

        if (BookingDomainService.HasConflicts(bookings, requestedRange))
            return (null, new BookingValidationError("time.not_available", "Выбранный интервал уже занят, выберите свободное время."));

        var entity = new FieldBooking(
            Guid.NewGuid(),
            field.Id,
            request.CustomerName,
            requestedRange,
            field.HourlyRate * request.Hours);

        var saved = await bookingRepository.AddAsync(entity, cancellationToken);

        return (new BookingResponse(
            saved.Id,
            saved.FieldId,
            saved.CustomerName,
            saved.Range.StartUtc,
            saved.Range.EndUtc,
            request.Hours,
            saved.TotalPrice), null);
    }
}
