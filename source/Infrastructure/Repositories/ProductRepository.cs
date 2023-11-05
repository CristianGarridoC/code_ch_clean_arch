using Application.Abstractions;
using Application.Product.Common;
using Dapper;
using Domain.Products;

namespace Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public ProductRepository(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<ProductResponse?> GetByIdAsync(ProductId id)
    {
        await using var connection = _connectionFactory.CreateConnection();
        const string sql = "SELECT id, name, brand, price from products where id = @Id";
        var result = await connection.QueryFirstOrDefaultAsync(sql, new { Id = id.Value.ToString() });
        return result is null ? 
            null : 
            new ProductResponse(result.id, result.name, result.brand, result.price, Currency.USD.Code);
    }

    public async Task<ProductResponse?> GetByNameAndBrandAsync(Name name, Brand brand, ProductId id)
    {
        await using var connection = _connectionFactory.CreateConnection();
        const string sql = "SELECT id, name, brand, price from products where lower(name) = @Name and lower(brand) = @Brand and id <> @Id ";
        var result = await connection.QueryFirstOrDefaultAsync(sql, new
        {
            Id = id.Value.ToString(),
            Name = name.Value.ToLower().Trim(),
            Brand = brand.Value.ToLower().Trim()
        });
        return result is null ? 
            null : 
            new ProductResponse(result.id, result.name, result.brand, result.price, Currency.USD.Code);
    }

    public async Task AddAsync(Domain.Products.Product product)
    {
        await using var connection = _connectionFactory.CreateConnection();
        const string sql = """
            INSERT INTO products (id, name, brand, price)
            VALUES(@Id, @Name, @Brand, @Price)
        """;
        await connection.ExecuteAsync(sql, new
        {
            Id = product.Id.Value.ToString(),
            Name = product.Name.Value.Trim(),
            Brand = product.Brand.Value.Trim(),
            Price = product.Price.Amount
        });
    }

    public async Task UpdateAsync(Domain.Products.Product product)
    {
        await using var connection = _connectionFactory.CreateConnection();
        const string sql = """
            UPDATE products
            SET name = @Name,
                brand = @Brand,
                price = @Price
            WHERE id = @Id
        """;
        await connection.ExecuteAsync(sql, new
        {
            Id = product.Id.Value.ToString(),
            Name = product.Name.Value.Trim(),
            Brand = product.Brand.Value.Trim(),
            Price = product.Price.Amount
        });
    }

    public async Task DeleteAsync(ProductId id)
    {
        await using var connection = _connectionFactory.CreateConnection();
        const string sql = "DELETE FROM products WHERE id = @Id";
        await connection.ExecuteAsync(sql, new
        {
            Id = id.Value.ToString()
        });
    }

    public async Task<IEnumerable<ProductResponse>> GetAll()
    {
        await using var connection = _connectionFactory.CreateConnection();
        const string sql = "SELECT id, name, brand, price from products";
        var result = await connection.QueryAsync(sql);
        return !result.Any() ?
            Enumerable.Empty<ProductResponse>() : 
            result.Select(item => new ProductResponse(item.id, item.name, item.brand, item.price, Currency.USD.Code));
    }
}