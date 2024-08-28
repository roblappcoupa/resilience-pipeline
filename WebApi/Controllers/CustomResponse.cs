namespace WebApi.Controllers;

public sealed class CustomResponse
{
    public CustomResponse(
        int delay,
        int statusCode,
        IDictionary<string, string> data = null)
    {
        this.Delay = delay;
        this.StatusCode = statusCode;
        this.Data = data == null
            ? new Dictionary<string, string>()
            : new Dictionary<string, string>(data);
    }

    public int Delay { get; }
        
    public int StatusCode { get; }

    public IDictionary<string, string> Data { get; }
}