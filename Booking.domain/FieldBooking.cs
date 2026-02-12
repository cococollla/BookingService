namespace Booking.domain;

public sealed class FieldBooking
{
    public FieldBooking(Guid id, Guid fieldId, string customerName, TimeRange range, decimal totalPrice)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Идентификатор бронирования обязателен.", nameof(id));

        if (fieldId == Guid.Empty)
            throw new ArgumentException("Идентификатор поля обязателен.", nameof(fieldId));

        if (string.IsNullOrWhiteSpace(customerName))
            throw new ArgumentException("Имя клиента обязательно.", nameof(customerName));

        if (totalPrice <= 0)
            throw new ArgumentOutOfRangeException(nameof(totalPrice), "Сумма бронирования должна быть положительной.");

        Id = id;
        FieldId = fieldId;
        CustomerName = customerName;
        Range = range;
        TotalPrice = totalPrice;
    }

    public Guid Id { get; }
    public Guid FieldId { get; }
    public string CustomerName { get; }
    public TimeRange Range { get; }
    public decimal TotalPrice { get; }

    public bool ConflictsWith(TimeRange requestedRange) => Range.Overlaps(requestedRange);
}
