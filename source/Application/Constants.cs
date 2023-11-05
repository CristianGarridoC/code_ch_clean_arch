namespace Application;

public static class Constants
{
    public static class Cache
    {
        public const string FailedCacheKey = "Failed to cache key";
        public const string FailedGetKey = "Failed to get the cache key";
        public const string FailedDeleteKey = "Failed to delete cache key";
    }

    public static class Product
    {
        public const string AlreadyExists = "The product name already exists in the database";
        public const string NotFound = "We could not found the product";
    }
    
    public static class ErrorMessages
    {
        public const string ValidationError = "One or more validation failures have occurred";
        public const string UnhandledError = "An error occurred while processing your request";
    }
}