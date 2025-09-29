namespace theredhead.common;

public static class TaskExtensions
{
    public static async Task<T> RunAfter<T>(this Task<T> task, TimeSpan time)
    {
        await Task.Delay(time);
        return await task;
    }
}