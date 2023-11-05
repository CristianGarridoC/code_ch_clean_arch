using Application;
using Application.Abstractions;
using Application.Exceptions;
using Application.Product.Commands.Edit;
using Application.Product.Common;
using AutoFixture;
using Domain.Products;
using FluentAssertions;
using NSubstitute;

namespace UnitTests.Application.Products.Commands;

public class EditCommandHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly Fixture _fixture;
    private readonly EditCommandHandler _sut;

    public EditCommandHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _fixture = new Fixture();
        _sut = new EditCommandHandler(_productRepository);
    }
    
    [Fact]
    public async Task Handle_ShouldReturnUnit_WhenRequestIsSuccessful()
    {
        var request = new EditRequest(Guid.NewGuid(), "Test", "Brand-test", 100);
        var product = _fixture.Create<ProductResponse>();
        _productRepository
            .GetByIdAsync(Arg.Any<ProductId>())
            .Returns(product);

        await _sut.Handle(request, CancellationToken.None);
        
        await _productRepository.Received(1).UpdateAsync(Arg.Any<Domain.Products.Product>());
    }
    
    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenProductIdDoesNotExist()
    {
        var request = new EditRequest(Guid.NewGuid(), "Test", "Brand-test", 100);

        var result = async () => await _sut.Handle(request, CancellationToken.None);

        await result.Should().ThrowAsync<NotFoundException>().WithMessage(Constants.Product.NotFound);
        await _productRepository.Received(0).UpdateAsync(Arg.Any<Domain.Products.Product>());
    }
    
    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenProductNameExists()
    {
        var request = new EditRequest(Guid.NewGuid(), "Test", "Brand-test", 100);
        var product = _fixture.Create<ProductResponse>();
        _productRepository
            .GetByIdAsync(Arg.Any<ProductId>())
            .Returns(product);
        _productRepository
            .GetByNameAndBrandAsync(Arg.Any<Name>(), Arg.Any<Brand>(), Arg.Any<ProductId>())
            .Returns(product);

        var result = async () => await _sut.Handle(request, CancellationToken.None);

        await result.Should().ThrowAsync<AlreadyExistsException>().WithMessage(Constants.Product.AlreadyExists);
        await _productRepository.Received(0).UpdateAsync(Arg.Any<Domain.Products.Product>());
    }
}