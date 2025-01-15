using Mock.ShippingProvider.API.Attributes;

namespace Mock.ShippingProvider.API.Endpoints
{
    public static class Shipments
    {
        public static void RegisterShipmentEndpoints(this IEndpointRouteBuilder routes)
        {
            var shipments = routes.MapGroup("/api/v1/shipments");

            shipments.MapGet("", () => { return Results.NotFound(); })
                .WithMetadata(new AllowAnonymousApiKeyAttribute());

            shipments.MapGet("/{id}", (int id) => { return Results.BadRequest(); })
                .WithMetadata(new AllowAnonymousApiKeyAttribute());


            shipments.MapPost("", () => { return Results.Unauthorized(); })
                .WithMetadata(new AllowAnonymousApiKeyAttribute());


            shipments.MapPut("/{id}", (int id) =>
            {
                var errors = new Dictionary<string, string[]> { { "HHOHIO", ["HOHHHHHHHHHHH", "hiiiiiii"] } };

                return Results.ValidationProblem(errors);
            })
                .WithMetadata(new AllowAnonymousApiKeyAttribute());
            ;

            shipments.MapDelete("/{id}", (int id) => { });
        }
    }
}
