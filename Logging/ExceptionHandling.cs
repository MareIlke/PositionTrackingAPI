
    using System.Net;

    public class ExceptionHandling {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandling> _logger;

        public ExceptionHandling(RequestDelegate next, ILogger<ExceptionHandling> logger){
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context){
            try {
                await _next(context);  
            }
            catch (Exception ex) {
                _logger.LogError(ex, "An unhandled exception occurred during the request processing.");
                await HandleException(context, ex);
            }
        }

        private Task HandleException(HttpContext context, Exception exception) {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
        
            var errorResponse = new {
                message = "An unexpected error occurred. Please try again later.",
                error = exception.Message
            };
            
            return context.Response.WriteAsJsonAsync(errorResponse);
        }
    }