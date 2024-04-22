namespace theredhead.core;

public static class Sanity
{
    /// <summary>
    /// Enforce a condition
    /// </summary>
    /// <param name="condition"></param>
    /// <param name="message"></param>
    /// <exception cref="SanityException"></exception>
    public static void Enforce(bool condition, string message)
    {
        if (!condition)
        {
            throw new SanityException(message);
        }
    }

    public static void Enforce(bool condition, string message, Exception innerException)
    {
        if (!condition)
        {
            throw new SanityException(message, innerException);
        }
    }

    public static void Enforce(bool condition, Func<string> message)
    {
        if (!condition)
        {
            throw new SanityException(message());
        }
    }

    public static void Enforce(bool condition, Func<string> message, Exception innerException)
    {
        if (!condition)
        {
            throw new SanityException(message(), innerException);
        }
    }


    public static void EnforceNotNull(object? obj, string message)
    {
        Enforce(obj != null, message);
    }

    public static void EnforceNotNull(object? obj, string message, Exception innerException)
    {
        Enforce(obj != null, message, innerException);
    }

    public static void EnforceNotNull(object? obj, Func<string> message)
    {
        Enforce(obj != null, message);
    }

    public static void EnforceNotNull(object? obj, Func<string> message, Exception innerException)
    {
        Enforce(obj != null, message, innerException);
    }


    public static void EnforceNotEmpty(string? str, string message)
    {
        Enforce(!string.IsNullOrWhiteSpace(str), message);
    }

    public static void EnforceNotEmpty(string? str, string message, Exception innerException)
    {
        Enforce(!string.IsNullOrWhiteSpace(str), message, innerException);
    }

    public static void EnforceNotEmpty(string? str, Func<string> message)
    {
        Enforce(!string.IsNullOrWhiteSpace(str), message);
    }

    public static void EnforceNotEmpty(string? str, Func<string> message, Exception innerException)
    {
        Enforce(!string.IsNullOrWhiteSpace(str), message, innerException);
    }

    
    public static void EnforceNotEmpty<T>(IEnumerable<T>? collection, string message)
    {
        Enforce(collection != null && collection.Any(), message);
    }

    public static void EnforceNotEmpty<T>(IEnumerable<T>? collection, string message, Exception innerException)
    {
        Enforce(collection != null && collection.Any(), message, innerException);
    }

    public static void EnforceNotEmpty<T>(IEnumerable<T>? collection, Func<string> message)
    {
        Enforce(collection != null && collection.Any(), message);
    }

    public static void EnforceNotEmpty<T>(IEnumerable<T>? collection, Func<string> message, Exception innerException)
    {
        Enforce(collection != null && collection.Any(), message, innerException);
    }


    public static void EnforceNotWhiteSpace(string? str, string message)
    {
        Enforce(!string.IsNullOrWhiteSpace(str), message);
    }

    public static void EnforceNotWhiteSpace(string? str, string message, Exception innerException)
    {
        Enforce(!string.IsNullOrWhiteSpace(str), message, innerException);
    }

    public static void EnforceNotWhiteSpace(string? str, Func<string> message)
    {
        Enforce(!string.IsNullOrWhiteSpace(str), message);
    }

    public static void EnforceNotWhiteSpace(string? str, Func<string> message, Exception innerException)
    {
        Enforce(!string.IsNullOrWhiteSpace(str), message, innerException);
    }


    public static void EnforceNotDefault<T>(T obj, string message)
    {
        Enforce(!EqualityComparer<T>.Default.Equals(obj, default), message);
    }

    public static void EnforceNotDefault<T>(T obj, string message, Exception innerException)
    {
        Enforce(!EqualityComparer<T>.Default.Equals(obj, default), message, innerException);
    }

    public static void EnforceNotDefault<T>(T obj, Func<string> message)
    {
        Enforce(!EqualityComparer<T>.Default.Equals(obj, default), message);
    }

    public static void EnforceNotDefault<T>(T obj, Func<string> message, Exception innerException)
    {
        Enforce(!EqualityComparer<T>.Default.Equals(obj, default), message, innerException);
    }


    public static void EnforceNotDefault<T>(T? obj, string message) where T : struct
    {
        Enforce(obj.HasValue, message);
    }

    public static void EnforceNotDefault<T>(T? obj, string message, Exception innerException) where T : struct
    {
        Enforce(obj.HasValue, message, innerException);
    }

    public static void EnforceNotDefault<T>(T? obj, Func<string> message) where T : struct
    {
        Enforce(obj.HasValue, message);
    }

    public static void EnforceNotDefault<T>(T? obj, Func<string> message, Exception innerException) where T : struct
    {
        Enforce(obj.HasValue, message, innerException);
    }

}
public class SanityException : Exception
{
    public SanityException(string message) : base(message)
    {
    }
    public SanityException(string message, Exception innerException) : base(message, innerException)
    {
    }
}