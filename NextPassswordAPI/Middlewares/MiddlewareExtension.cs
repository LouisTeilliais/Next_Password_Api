namespace NextPassswordAPI.Middlewares
{
    public static class MiddlewareExtension
    {
        public static IApplicationBuilder UseRequestLogMiddleware(
        this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TokenMiddleware>();
        }
    }
}
