namespace Hospital_Management_System.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var start = DateTime.Now;

            await _next(context);

            var time = DateTime.Now - start;

            Console.WriteLine($"{context.Request.Method} {context.Request.Path} {time.TotalMilliseconds}ms");
        }
    }
}
