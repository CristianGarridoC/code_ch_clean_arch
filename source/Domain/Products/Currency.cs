namespace Domain.Products;

public sealed record Currency
{
    public static Currency USD = new("USD");
    public string Code { get; }
    private Currency(string code)
    {
        Code = code;
    }

    public static Currency FromCode(string code)
    {
        return All
            .FirstOrDefault(x => string.Equals(
                x.Code,
                code.Trim(),
                StringComparison.InvariantCultureIgnoreCase)
            ) ?? throw new ApplicationException("We could not find the code provided");
    }
    
    public static IReadOnlyCollection<Currency> All => new[] { USD };
};