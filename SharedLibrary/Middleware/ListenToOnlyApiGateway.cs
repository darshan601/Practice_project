using Microsoft.AspNetCore.Http;

namespace SharedLibrary.Middleware;

public class ListenToOnlyApiGateway(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        // extract specific header from the request
        var signedHeader = context.Request.Headers["Api-Gateway"];

        // NULL means the request is not coming from the api gateway //503 service unavailable
        if (signedHeader.FirstOrDefault() is null)
        {
            context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
            await context.Response.WriteAsync("Sorry, the Service is unavailable");
            return;
        }
        else
        {
            await next(context);
        }


    }
    
}