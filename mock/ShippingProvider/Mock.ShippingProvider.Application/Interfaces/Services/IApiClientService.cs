using Mock.ShippingProvider.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mock.ShippingProvider.Application.Interfaces.Services
{
    public interface IApiClientService
    {
        Task<IEnumerable<ApiClientDTO>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<ApiClientDTO?> GetIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<ApiClientDTO> CreateAsync(CreateApiClientRequestDTO request, CancellationToken cancellationToken = default);
        Task<ApiClientDTO> UpdateAsync(UpdateApiClientRequestDTO request, CancellationToken cancellationToken = default);
        Task<bool> DeactivateAsync(Guid id, CancellationToken cancellationToken = default);
        Task<bool> ActivateAsync(Guid id, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

        Task<bool> UpdateApiKeyAsync(Guid id, CancellationToken cancellationToken = default);
        Task<bool> UpdateSecretKeyAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
