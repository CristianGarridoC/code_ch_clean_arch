namespace Domain.Products;

public sealed record Brand
{
    private Brand(string value)
    {
        Value = value;
    }
    
    public string Value { get; }

    public static Brand Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ApplicationException("The product brand can not be null or empty");
        }

        return new Brand(value);
    }
}