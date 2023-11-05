using Application.Product.Common;
using Domain.Products;

namespace Application.Abstractions;

public interface IProductRepository
{
    Task<ProductResponse?> GetByIdAsync(ProductId id);
    Task<ProductResponse?> GetByNameAndBrandAsync(Name name, Brand brand, ProductId id);
    Task AddAsync(Domain.Products.Product product);
    Task UpdateAsync(Domain.Products.Product product);
    Task DeleteAsync(ProductId id);
    Task<IEnumerable<ProductResponse>> GetAll();
}