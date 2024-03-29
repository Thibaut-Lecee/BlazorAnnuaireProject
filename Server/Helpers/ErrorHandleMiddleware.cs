namespace BlazorAnnuaireProject.Helpers;

using System.Net;
using System.Text.Json;

// Ce middleware permet de gérer le type de réponse en fonction du type d'erreur rencontré par l'application
public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception error)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            switch(error)
            {
                case AppException e:
                    // custom application error
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case KeyNotFoundException e:
                    // not found error
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;

                // case forbidden error
                case WebException e:
                    response.StatusCode = (int)HttpStatusCode.Forbidden;
                    break;

                case UnauthorizedAccessException e:
                    // not found error
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    break;
                case Exception e:
                    // unhandled error
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
                    
                default:
                    // unhandled error
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            var result = JsonSerializer.Serialize(new { message = error?.Message });
            await response.WriteAsync(result);
        }
    }
}
