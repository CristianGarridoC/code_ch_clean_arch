using Application.Abstractions;
using Application.Exceptions;
using Application.Product.Common;
using Application.Product.Queries.GetById;
using Domain.Products;
using FluentAssertions;
using NSubstitute;

namespace UnitTests.Application.Products.Queries;

public class GetByIdQueryHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly GetByIdQueryHandler _sut;
    
    public GetByIdQueryHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _sut = new GetByIdQueryHandler(_productRepository);
    }
    
    [Fact]
    public async Task Handle_WhenProductNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var request = new GetByIdRequest(Guid.NewGuid());
        _productRepository.GetByIdAsync(Arg.Any<ProductId>()).Returns((ProductResponse)null);
        
        // Act
        Func<Task> act = async () => await _sut.Handle(request, CancellationToken.None);
        
        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
    
    [Fact]
    public async Task Handle_WhenProductFound_ReturnsProduct()
    {
        // Arrange
        var request = new GetByIdRequest(Guid.NewGuid());
        var product = new ProductResponse(Guid.NewGuid().ToString(), "Test", "Brand", 10, "EUR");
        _productRepository.GetByIdAsync(Arg.Any<ProductId>()).Returns(product);
        
        // Act
        var result = await _sut.Handle(request, CancellationToken.None);
        
        // Assert
        result.Should().BeEquivalentTo(product);
    }
}