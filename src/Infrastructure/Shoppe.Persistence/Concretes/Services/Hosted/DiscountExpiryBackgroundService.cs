using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shoppe.Application.Abstractions.Repositories.DiscountRepos;
using Shoppe.Application.Abstractions.UoW;
using Shoppe.Persistence.Context;

namespace Shoppe.Persistence.Concretes.Services.Hosted
{
    public class DiscountExpiryBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration; 

        public DiscountExpiryBackgroundService(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            bool isDbAvailable = ShoppeDbContext.CheckDatabaseAvailability(_configuration);

            if (!isDbAvailable) return;

            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var discountWriteRepository = scope.ServiceProvider.GetRequiredService<IDiscountWriteRepository>();
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                    var expiredDiscounts = await discountWriteRepository.Table
                        .Where(d => d.IsActive && d.EndDate <= DateTime.UtcNow)
                        .ToListAsync(stoppingToken);

                    if (expiredDiscounts.Count > 0)
                    {
                        foreach (var discount in expiredDiscounts)
                        {
                            discount.IsActive = false;
                            discountWriteRepository.Update(discount);
                        }

                        await unitOfWork.SaveChangesAsync(stoppingToken);
                    }
                }

                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
    }
}
