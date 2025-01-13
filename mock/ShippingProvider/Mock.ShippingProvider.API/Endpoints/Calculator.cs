using Mock.ShippingProvider.Application.Interfaces;

namespace Mock.ShippingProvider.API.Endpoints
{
    public static class Calculator
    {
        public static void RegisterCalculatorEndpoints(this IEndpointRouteBuilder routes)
        {
            var calculatorGroup = routes.MapGroup("/api/v1/calculator");

            // Endpoint for retrieving country information
            calculatorGroup.MapGet("country", async (string country, IGeoInfoService geoInfoService) =>
            {
                var res = await geoInfoService.GetCountryGeoInfoByNameAsync(country); // get name of the country

                // Handle logic to retrieve country details
                return Results.Ok(res);
            });

            // Endpoint for performing the calculation with query parameters for country, weight, dimensions, shipping method
            calculatorGroup.MapGet("calculate", (string country, double weight, double dimension, string shippingMethod) =>
            {
                // Handle calculation logic based on the query parameters
                var result = new
                {
                    Country = country,
                    Weight = weight,
                    Dimension = dimension,
                    ShippingMethod = shippingMethod,
                    EstimatedCost = 99.99 // This is just an example
                };
                return Results.Ok(result);
            });
        }
    }
}
