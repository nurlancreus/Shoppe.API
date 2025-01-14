
using Mock.ShippingProvider.API.Endpoints;
using Mock.ShippingProvider.Infrastructure.Persistence;

namespace Mock.ShippingProvider.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.RegisterServices();

            var app = builder.Build();

            app.RegisterMiddlewares();

            app.RegisterShipmentEndpoints();
            app.RegisterCalculatorEndpoints();

            app.Run();
        }
    }
}
