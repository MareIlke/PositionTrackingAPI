
    public class RequestLogging {
        
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLogging> _logger;

        public RequestLogging(RequestDelegate next, ILogger<RequestLogging> logger){
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context) {
            _logger.LogInformation("Incoming request: {Method} {Path} at {Timestamp}", 
                context.Request.Method, context.Request.Path, DateTime.UtcNow);

            try {
                await _next(context);
            
                _logger.LogInformation("Outgoing response: {StatusCode} at {Timestamp}", 
                    context.Response.StatusCode, DateTime.UtcNow);
            }
            catch (Exception ex) {
            
                _logger.LogError(ex, "An error occurred while processing the request: {Method} {Path} at {Timestamp}", 
                    context.Request.Method, context.Request.Path, DateTime.UtcNow);
                throw; 
            }
        }
    }