namespace Ong.Api.Campaigns.Extensions
{
    public static class FilterExtensions
    {
        public static RouteHandlerBuilder RequireApiKey(this RouteHandlerBuilder builder)
        {
            return builder.AddEndpointFilter(async (context, next) =>
            {
                var request = context.HttpContext.Request;
                var config = context.HttpContext.RequestServices
                    .GetRequiredService<IConfiguration>();

                var receivedKey = request.Headers["X-Api-Key"].FirstOrDefault();
                var expectedKey = config["ApiKeys:WorkerApiKey"];

                if (string.IsNullOrEmpty(receivedKey) || receivedKey != expectedKey)
                    return Results.Unauthorized();

                return await next(context);
            });
        }
    }
}
