namespace Mock.ShippingProvider.API.Endpoints
{
    public static class Shipments
    {
        public static void RegisterShipmentEndpoints(this IEndpointRouteBuilder routes)
        {
            var users = routes.MapGroup("/api/v1/shipments");

            users.MapGet("", () => { });

            users.MapGet("/{id}", (int id) => { });

            users.MapPost("", () => { });

            users.MapPut("/{id}", (int id) => { });

            users.MapDelete("/{id}", (int id) => { });
        }
    }
}
