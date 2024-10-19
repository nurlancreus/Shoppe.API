using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shoppe.Application.Abstractions.Repositories.DiscountRepos;
using Shoppe.Application.Abstractions.UoW;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Shoppe.Persistence.Concretes.Services.Hosted
{
    public class DiscountExpiryBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public DiscountExpiryBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var discountWriteRepository = scope.ServiceProvider.GetRequiredService<IDiscountWriteRepository>();
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                    var expiredDiscounts = await discountWriteRepository.Table
                        .Where(d => d.IsActive && d.EndDate <= DateTime.UtcNow)
                        .ToListAsync(stoppingToken);

                    if (expiredDiscounts.Count != 0)
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
