using Booking.domain;
using System.Collections.Concurrent;

namespace Booking.app;

public sealed class InMemoryFieldRepository : IFieldRepository
{
    private static readonly SportsField[] Fields =
    [
        new(Guid.Parse("0d417b4a-c3c1-4ba3-a60d-9102d7b52f1a"), "Футбольное поле №1", "football", 2500m, 1, 3),
        new(Guid.Parse("7696e864-ea59-4f48-b6e6-c8d614af17a4"), "Теннисный корт А", "tennis", 3000m, 1, 2),
        new(Guid.Parse("5f0f67d4-a313-49f4-bf44-4942bb8942a0"), "Баскетбольный зал", "basketball", 2800m, 1, 4)
    ];

    public Task<IReadOnlyCollection<SportsField>> GetAllAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult<IReadOnlyCollection<SportsField>>(Fields);
    }

    public Task<SportsField?> GetByIdAsync(Guid fieldId, CancellationToken cancellationToken)
    {
        return Task.FromResult(Fields.FirstOrDefault(x => x.Id == fieldId));
    }
}

public sealed class InMemoryBookingRepository : IBookingRepository
{
    private readonly ConcurrentDictionary<Guid, List<FieldBooking>> _bookingsByField = new();

    public Task<IReadOnlyCollection<FieldBooking>> GetByFieldAsync(Guid fieldId, CancellationToken cancellationToken)
    {
        var bookings = _bookingsByField.TryGetValue(fieldId, out var existing)
            ? existing.ToArray()
            : [];

        return Task.FromResult<IReadOnlyCollection<FieldBooking>>(bookings);
    }

    public Task<FieldBooking> AddAsync(FieldBooking booking, CancellationToken cancellationToken)
    {
        var storage = _bookingsByField.GetOrAdd(booking.FieldId, _ => []);

        lock (storage)
        {
            storage.Add(booking);
        }

        return Task.FromResult(booking);
    }
}
