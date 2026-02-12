using Booking.bridge;
using Booking.domain;

namespace Booking.app;

public interface IFieldRepository
{
    Task<IReadOnlyCollection<SportsField>> GetAllAsync(CancellationToken cancellationToken);
    Task<SportsField?> GetByIdAsync(Guid fieldId, CancellationToken cancellationToken);
}

public interface IBookingRepository
{
    Task<IReadOnlyCollection<FieldBooking>> GetByFieldAsync(Guid fieldId, CancellationToken cancellationToken);
    Task<FieldBooking> AddAsync(FieldBooking booking, CancellationToken cancellationToken);
}

public interface IBookingService
{
    Task<IReadOnlyCollection<SportsFieldResponse>> GetFieldsAsync(CancellationToken cancellationToken);
    Task<IReadOnlyCollection<TimeSlotResponse>> GetAvailableSlotsAsync(Guid fieldId, DateOnly date, CancellationToken cancellationToken);
    Task<(BookingResponse? Booking, BookingValidationError? Error)> CreateBookingAsync(CreateBookingRequest request, CancellationToken cancellationToken);
}
