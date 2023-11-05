using Application.Abstractions;

namespace Infrastructure.Provider;

internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime Now => DateTime.Now;
    public DateTimeOffset DateTimeOffsetNow => DateTimeOffset.Now;
}