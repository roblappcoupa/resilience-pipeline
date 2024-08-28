namespace WebApi.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1/[controller]")]
public class ErrorController : ControllerBase
{
    private readonly ILogger<ErrorController> logger;
    
    public ErrorController(ILogger<ErrorController> logger)
    {
        this.logger = logger;
    }

    [HttpGet("endpoint1")]
    public async Task<IActionResult> Endpoint1(
        [FromQuery]int delaySeconds,
        [FromQuery]int statusCode,
        [FromQuery]bool returnData)
    {
        this.logger.LogInformation("Delaying for {Seconds} seconds", delaySeconds);

        await Task.Delay(TimeSpan.FromSeconds(delaySeconds));

        object data = returnData
            ? new CustomResponse(
                delaySeconds,
                statusCode,
                new Dictionary<string, string>
                {
                    { "SomeKey", "SomeValue" }
                })
            : null;

        return this.StatusCode(statusCode, data);
    }

}