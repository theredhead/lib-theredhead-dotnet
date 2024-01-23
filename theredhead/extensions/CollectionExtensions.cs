namespace theredhead.extensions;

public static class CollectionExtensions
{
    public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> values) 
    {
        foreach(var value in values) 
        {
            collection.Add(value);
        }
    }

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

    public static IEnumerable<T> Distinct<T>(this IEnumerable<T> collection) 
    {
        var set = new HashSet<T>(collection);
        foreach(var item in set) 
        {
            yield return item;
        }
    }
}