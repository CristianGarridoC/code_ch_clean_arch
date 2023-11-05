namespace Domain.Products;

public sealed class Product
{
    public ProductId Id { get; }
    public Brand Brand { get; }
    public Name Name { get; }
    public Money Price { get; }

    private Product(ProductId id, Name name, Brand brand, Money price)
    {
        Id = id;
        Name = name;
        Brand = brand;
        Price = price;
    }

    public static Product Create(Name name, Brand brand, Money price)
    {
        return new Product(new ProductId(Guid.NewGuid()), name, brand, price);
    }
    
    public static Product Create(ProductId id, Name name, Brand brand, Money price)
    {
        return new Product(id, name, brand, price);
    }
}