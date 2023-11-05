using Application.Abstractions;
using Application.Product;
using Application.Product.Commands.Delete;
using Application.Product.Common;
using AutoFixture;
using Domain.Products;
using MediatR;
using NSubstitute;

namespace UnitTests.Application.Products.Commands;

public class DeleteCommandHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly IPublisher _publisher;
    private readonly Fixture _fixture;
    private readonly DeleteCommandHandler _sut;

    public DeleteCommandHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _fixture = new Fixture();
        _publisher = Substitute.For<IPublisher>();
        _sut = new DeleteCommandHandler(_productRepository, _publisher);
    }
    
    [Fact]
    public async Task Handle_ShouldCallDeleteAsync()
    {
        // Arrange
        var command = _fixture.Create<DeleteRequest>();
        var product = _fixture.Create<ProductResponse>();
        _productRepository
            .GetByIdAsync(Arg.Any<ProductId>())
            .Returns(product);
        
        // Act
        await _sut.Handle(command, CancellationToken.None);
        
        // Assert
        await _productRepository.Received(1).DeleteAsync(new ProductId(command.Id));
        await _publisher.Received(1).Publish(Arg.Any<CacheInvalidationProductEvent>());
    }
}