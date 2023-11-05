using Application;
using Application.Abstractions;
using Application.Product.Common;
using Application.Product.Queries.GetAll;
using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NSubstitute;

namespace UnitTests.Application.Products.Queries;

public class GetAllQueryHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly ICacheService _cacheService;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ILogger<GetAllQueryHandler> _logger;
    private readonly GetAllQueryHandler _sut;

    public GetAllQueryHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _cacheService = Substitute.For<ICacheService>();
        _dateTimeProvider = Substitute.For<IDateTimeProvider>();
        _logger = Substitute.For<ILogger<GetAllQueryHandler>>();
        _sut = new GetAllQueryHandler(_productRepository, _cacheService, _dateTimeProvider, _logger);
    }
    
    [Fact]
    public async Task Handle_ShouldReturnDbList_WhenCacheIsEmpty()
    {
        var products = new List<ProductResponse>
        {
            new(
                Guid.NewGuid().ToString(),
                "Test",
                "brand",
                10,
                "USD"
            )
        };
        // Arrange
        _cacheService.GetRecordAsync(Constants.Product.CacheKey).Returns(string.Empty);
        _productRepository.GetAll().Returns(products);
        _dateTimeProvider.DateTimeOffsetNow.Returns(DateTimeOffset.Now);
        
        // Act
        var result = await _sut.Handle(new GetAllRequest(), CancellationToken.None);
        
        // Assert
        await _cacheService.Received(1).GetRecordAsync(Constants.Product.CacheKey);
        await _cacheService.Received(1).SetRecordAsync(Constants.Product.CacheKey, Arg.Any<string>(), Arg.Any<DistributedCacheEntryOptions>());
        result.Should().NotBeNull();
        result.Products.Should().NotBeEmpty();
    }
    
    [Fact]
    public async Task Handle_ShouldReturnCacheList_WhenCacheIsNotEmpty()
    {
        var products = new List<ProductResponse>
        {
            new(
                Guid.NewGuid().ToString(),
                "Test",
                "brand",
                10,
                "USD"
            )
        };
        // Arrange
        _cacheService.GetRecordAsync(Constants.Product.CacheKey).Returns(JsonConvert.SerializeObject(products));
        _productRepository.GetAll().Returns(products);
        _dateTimeProvider.DateTimeOffsetNow.Returns(DateTimeOffset.Now);
        
        // Act
        var result = await _sut.Handle(new GetAllRequest(), CancellationToken.None);
        
        // Assert
        await _cacheService.Received(1).GetRecordAsync(Constants.Product.CacheKey);
        await _cacheService.DidNotReceive().SetRecordAsync(Constants.Product.CacheKey, Arg.Any<string>(), Arg.Any<DistributedCacheEntryOptions>());
        result.Should().NotBeNull();
        result.Products.Should().NotBeEmpty();
        result.Products.Should().BeEquivalentTo(products);
    }
}