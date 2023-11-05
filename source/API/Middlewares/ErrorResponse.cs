namespace API.Middlewares;

public class ErrorResponse
{
    public string Message { get; init; }
    
    public int Status { get; init; }
    
    public IReadOnlyDictionary<string, string[]> Details { get; init; }
}