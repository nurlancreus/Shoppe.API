namespace Mock.ShippingProvider.API.Endpoints
{
    public static class Shipments
    {
        public static void RegisterShipmentEndpoints(this IEndpointRouteBuilder routes)
        {
            var shipments = routes.MapGroup("/api/v1/shipments");

            shipments.MapGet("", () => { });

            shipments.MapGet("/{id}", (int id) => { });

            shipments.MapPost("", () => { });

            shipments.MapPut("/{id}", (int id) => { });

            shipments.MapDelete("/{id}", (int id) => { });
        }
    }
}
