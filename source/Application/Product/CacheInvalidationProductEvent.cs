using MediatR;

namespace Application.Product;

internal sealed record CacheInvalidationProductEvent() : INotification;