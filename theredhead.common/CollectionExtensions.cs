namespace theredhead.core.Extensions;

/// <summary>
/// Provides extension methods for collections.
/// </summary>
public static class CollectionExtensions
{
    /// <summary>
    /// Adds a range of values to a collection.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collection"></param>
    /// <param name="values"></param>
    public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> values) 
    {
        foreach(var value in values) 
        {
            collection.Add(value);
        }
    }

    /// <summary>
    /// Concatenates two collections into a single collection.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collection"></param>
    /// <param name="other"></param>
    /// <returns></returns>
    public static IEnumerable<T> Concat<T>(this IEnumerable<T> collection, IEnumerable<T> other) 
    {
        foreach(var item in collection) 
        {
            yield return item;
        }
        foreach(var item in other) 
        {
            yield return item;
        }
    }

    /// <summary>
    /// Concatenates multiple collections into a single collection.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collection"></param>
    /// <param name="others"></param>
    /// <returns></returns>
    public static IEnumerable<T> Concat<T>(this IEnumerable<T> collection, params IEnumerable<T>[] others) {
        foreach(var item in collection) 
        {
            yield return item;
        }
        foreach(var other in others) 
        {
            foreach(var item in other)
            {
                yield return item;
            }
        }
    }

    /// <summary>
    /// Returns a new collection that contains the distinct elements of the original collection.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collection"></param>
    /// <returns></returns>
    public static IEnumerable<T> Distinct<T>(this IEnumerable<T> collection) 
    {
        var set = new HashSet<T>(collection);
        foreach(var item in set) 
        {
            yield return item;
        }
    }

    /// <summary>
    /// Sometimes this just reads better than Contains.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="needle"></param>
    /// <param name="haystack"></param>
    /// <returns></returns>
    public static bool In<T>(this T needle, IEnumerable<T> haystack)
    {
        return haystack.Contains(needle);
    }

    /// <summary>
    /// Groups a sequence of values by a key selector.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="source"></param>
    /// <param name="keySelector"></param>
    /// <returns></returns>
    public static Dictionary<TKey, IEnumerable<TValue>> GroupBy<TKey,TValue>(this IEnumerable<TValue> source, Func<TValue, TKey> keySelector) where TKey : notnull
    {
        var result = new Dictionary<TKey, IEnumerable<TValue>>();

        foreach(var item in source)
        {
            var key = keySelector(item);
            if(!result.ContainsKey(key))
            {
                result[key] = new List<TValue>();
            }
            if (result[key] is List<TValue> list)
            {
                list.Add(item);
            }
        }

        return result;
    }
}