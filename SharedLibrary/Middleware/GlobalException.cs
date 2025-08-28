using System.Diagnostics;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SharedLibrary.Middleware;

public class GlobalException(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        string message = "Sorry, Internal Server Error Occured. Kindly try again";

        int statusCode = (int)HttpStatusCode.InternalServerError;

        string title = "Error";

        try
        {
            await next(context);
            
            // check if response is too many requests //429 status code
            if (context.Response.StatusCode == StatusCodes.Status429TooManyRequests)
            {
                title = "Warning";
                message = "Too Many Requests made";
                statusCode = StatusCodes.Status429TooManyRequests;
                await ModifyHeader(context, title, message, statusCode);
            }
            
            // check if response is Unauthorized //401 status code
            if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
            {
                title = "Alert";
                message = "You are not authorized to access this resource";
                statusCode = StatusCodes.Status401Unauthorized;
                await ModifyHeader(context, title, message, statusCode);
            }
            
            // check if response is forbidden //403 status code
            if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
            {
                title = "Out of Access";
                message = "You are not allowed/required to access";
                statusCode = StatusCodes.Status403Forbidden;
                await ModifyHeader(context, title, message, statusCode);
            }
        }
        catch (Exception e)
        {
            // log original exception /File, Debugger, Console
            
            // check if exception is timeout 408 request timeout
            if (e is TaskCanceledException || e is TimeoutException)
            {
                title = "Out of TIme";
                message = "Request Timeout.....Try Again ....";
                statusCode = StatusCodes.Status408RequestTimeout;
            }
            
            // if exceptions caught
            // if none of the exception, then do the default
            await ModifyHeader(context, title, message, statusCode);

        }
    }

    private async Task ModifyHeader(HttpContext context, string title, string message, int statuscode)
    {
        // display scary free message to client
        context.Response.ContentType = "Application/json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(new ProblemDetails()
        {
            Detail = message,
            Status = statuscode,
            Title = title
        }), CancellationToken.None);

        return;

    }
}