namespace Domain.Products;

public sealed record Name
{
    private Name(string value)
    {
        Value = value;
    }
    
    public string Value { get; }

    public static Name Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ApplicationException("The product name can not be null or empty");
        }

        return new Name(value);
    }
}