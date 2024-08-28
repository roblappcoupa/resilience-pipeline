namespace Console;

internal static class DummyService
{
    public static async Task<T> Execute<T>(
        T result,
        Exception exceptionToThrow,
        TimeSpan? delay = null)
    {
        if (delay != null)
        {
            await Task.Delay(delay.Value);
        }
        
        if (exceptionToThrow == null)
        {
            return result;
        }

        throw exceptionToThrow;
    }
}