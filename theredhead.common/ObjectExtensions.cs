namespace theredhead.core;

public static class ObjectExtensions
{
    public static T Also<T>(this T subject, Action<T> action)
    {
        action.Invoke(subject);
        return subject;
    }
}
