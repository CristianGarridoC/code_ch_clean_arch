using Application;
using Application.Abstractions;
using Application.Exceptions;
using Application.Product.Commands.Create;
using Application.Product.Common;
using AutoFixture;
using Domain.Products;
using FluentAssertions;
using NSubstitute;

namespace UnitTests.Application.Products.Commands;

public class CreateCommandHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly Fixture _fixture;
    private readonly CreateCommandHandler _sut;

    public CreateCommandHandlerTests()
    {
        _fixture = new Fixture();
        _productRepository = Substitute.For<IProductRepository>();
        _sut = new CreateCommandHandler(_productRepository);
    }

    [Fact]
    public async Task Handle_ShouldReturnProductId_WhenRequestIsSuccessful()
    {
        var request = new CreateRequest("Test", "Brand-test", 100);

        var result = await _sut.Handle(request, CancellationToken.None);

        result.Should().NotBeNull();
        await _productRepository.Received(1).AddAsync(Arg.Any<Domain.Products.Product>());
    }
    
    [Fact]
    public async Task Handle_ShouldThrowApplicationException_WhenNameIsEmpty()
    {
        var request = new CreateRequest("", "Brand-test", 100);

        var result = async () => await _sut.Handle(request, CancellationToken.None);

        await result.Should().ThrowAsync<ApplicationException>();
        await _productRepository.Received(0).AddAsync(Arg.Any<Domain.Products.Product>());
    }
    
    [Fact]
    public async Task Handle_ShouldThrowApplicationException_WhenBrandIsEmpty()
    {
        var request = new CreateRequest("Test", "", 100);

        var result = async () => await _sut.Handle(request, CancellationToken.None);

        await result.Should().ThrowAsync<ApplicationException>();
        await _productRepository.Received(0).AddAsync(Arg.Any<Domain.Products.Product>());
    }
    
    [Fact]
    public async Task Handle_ShouldThrowApplicationException_WhenPriceIsInvalid()
    {
        var request = new CreateRequest("Test", "brand", 0);

        var result = async () => await _sut.Handle(request, CancellationToken.None);

        await result.Should().ThrowAsync<ApplicationException>();
        await _productRepository.Received(0).AddAsync(Arg.Any<Domain.Products.Product>());
    }
    
    [Fact]
    public async Task Handle_ShouldThrowAlreadyExistsException_WhenRequestIsInvalid()
    {
        var request = new CreateRequest("Test", "Brand-test", 100);
        var product = _fixture.Create<ProductResponse>();
        _productRepository
            .GetByNameAndBrandAsync(Arg.Any<Name>(), Arg.Any<Brand>(), Arg.Any<ProductId>())
            .Returns(product);

        var result = async () => await _sut.Handle(request, CancellationToken.None);

        await result.Should().ThrowAsync<AlreadyExistsException>().WithMessage(Constants.Product.AlreadyExists);
        await _productRepository.Received(0).AddAsync(Arg.Any<Domain.Products.Product>());
    }
}