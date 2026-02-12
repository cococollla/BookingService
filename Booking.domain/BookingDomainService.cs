namespace Booking.domain;

public static class BookingDomainService
{
    public static bool HasConflicts(IEnumerable<FieldBooking> existingBookings, TimeRange requestedRange)
    {
        return existingBookings.Any(booking => booking.ConflictsWith(requestedRange));
    }
}
