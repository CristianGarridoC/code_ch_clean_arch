namespace Domain.Products;

public sealed record Money
{
    public decimal Amount { get; private init; }
    public Currency Currency { get; }

    private Money(decimal amount, Currency currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public static Money Create(decimal amount, Currency currency)
    {
        if (amount < 0)
        {
            throw new ApplicationException("The amount must be greater than or equals to 0");
        }

        return new Money(amount, currency);
    }
    
    public static Money operator +(Money first, Money second)
    {
        if (first.Currency != second.Currency)
        {
            throw new ApplicationException("Currencies have to be equal in the products");
        }

        return first with { Amount = first.Amount + second.Amount };
    }
    
    public static Money operator -(Money first, Money second)
    {
        if (first.Currency != second.Currency)
        {
            throw new ApplicationException("Currencies have to be equal in the products");
        }

        return first with { Amount = first.Amount - second.Amount };
    }
}