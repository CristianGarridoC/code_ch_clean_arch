namespace Application.Product.Queries.GetAll;

public record GetAllResponse(IEnumerable<Common.ProductResponse> Products);