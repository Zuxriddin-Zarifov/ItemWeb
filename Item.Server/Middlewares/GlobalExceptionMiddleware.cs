namespace Item.Server.Middlewares;
public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context); // Keyingi middleware yoki controller
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Global exception caught: {Message}", ex.Message);
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";

            var response = new
            {
                Message = "Serverda kutilmagan xatolik yuz berdi.",
                Error = ex.Message // faqat developmentda ko‘rsatish kerak
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
