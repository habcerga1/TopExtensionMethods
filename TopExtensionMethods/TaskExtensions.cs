namespace TopExtensionMethods;

public static class TaskExtensions
{
    public static async Task<Tout> TryAsync<Tout>(this Task<Tout> task, Action<Exception> errorHandler = null) {
        try {
            return await task;
        }
        catch (Exception ex) {
            if (errorHandler is not null) errorHandler(ex);
            throw ex;
        }
    }
    
    public static async Task TryAsync(this Task task, Action<Exception> errorHandler = null) {
        try {
            await task;
        }
        catch (Exception ex) {
            if (errorHandler is not null) errorHandler(ex);
        }
    }
    
    public static async Task<IEnumerable<T>> WhenAllAsync<T>(this IEnumerable<Task<T>> tasks) {
        if (tasks is null)
            throw new ArgumentNullException(nameof(tasks));

        return await Task
            .WhenAll(tasks)
            .ConfigureAwait(false);
    }

    public static Task WhenAllAsync(this IEnumerable<Task> tasks) {
        if (tasks is null)
            throw new ArgumentNullException(nameof(tasks));

        return Task
            .WhenAll(tasks);
    }
    
    public static async Task<IEnumerable<T>> WhenAllSequentialAsync<T>(this IEnumerable<Task<T>> tasks) {
        if (tasks is null)
            throw new ArgumentNullException(nameof(tasks));

        var results = new List<T>();
        foreach (var task in tasks)
            results.Add(await task.ConfigureAwait(false));
        return results;
    }

    public static async Task WhenAllSequentialAsync(this IEnumerable<Task> tasks) {
        if (tasks is null)
            throw new ArgumentNullException(nameof(tasks));

        foreach (var task in tasks)
            await task.ConfigureAwait(false);
    }
    
    public static async Task<T> DoAsync<T>(this Task<T> task, Func<T, Task> tapAsync) {
        if (task is null)
            throw new ArgumentNullException(nameof(task));

        if (tapAsync is null)
            throw new ArgumentNullException(nameof(tapAsync));

        var res = await task;
        await tapAsync(res);
        return res;
    }

    public static async Task<T> DoAsync<T>(this Task<T> task, Action<T> tap) {
        if (task is null)
            throw new ArgumentNullException(nameof(task));

        if (tap is null)
            throw new ArgumentNullException(nameof(tap));

        var res = await task;
        tap(res);
        return res;
    }
    
}