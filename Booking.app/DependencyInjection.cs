using Microsoft.Extensions.DependencyInjection;

namespace Booking.app;

public static class DependencyInjection
{
    public static IServiceCollection AddBookingModule(this IServiceCollection services)
    {
        services.AddSingleton<IFieldRepository, InMemoryFieldRepository>();
        services.AddSingleton<IBookingRepository, InMemoryBookingRepository>();
        services.AddScoped<IBookingService, BookingService>();

        return services;
    }
}
