namespace WebApi.Controllers;

using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1/[controller]")]
public class AppController : ControllerBase
{
    private readonly IHttpClientFactory clientFactory;
    private readonly ILogger<AppController> logger;
    
    public AppController(
        IHttpClientFactory clientFactory,
        ILogger<AppController> logger)
    {
        this.clientFactory = clientFactory;
        this.logger = logger;
    }

    [HttpGet("run")]
    public async Task<IActionResult> Run(
        [FromQuery]int clientTimeout,
        [FromQuery]int delaySeconds,
        [FromQuery]int statusCode,
        [FromQuery]bool returnData)
    {
        try
        {
            var client = this.clientFactory.CreateClient();
            client.Timeout = TimeSpan.FromSeconds(clientTimeout);

            var httpResponse = await client.GetAsync(
                $"http://localhost:5226/api/v1/Error/endpoint1?delaySeconds={delaySeconds}&statusCode={statusCode}&returnData={returnData}");

            var responseStr = await httpResponse.Content.ReadAsStringAsync();
            
            if (string.IsNullOrWhiteSpace(responseStr))
            {
                return this.StatusCode((int)httpResponse.StatusCode);
            }

            var obj = JsonSerializer.Deserialize<CustomResponse>(responseStr);

            return this.StatusCode(
                (int)httpResponse.StatusCode,
                obj);
        }
        catch (Exception exception)
        {
            this.logger.LogError(
                exception,
                "An error was thrown making the API call");
            
            throw;
        }
    }
}