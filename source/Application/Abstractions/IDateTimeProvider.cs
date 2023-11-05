namespace Application.Abstractions;

public interface IDateTimeProvider
{
    DateTime Now { get; }
    DateTimeOffset DateTimeOffsetNow { get; }
}