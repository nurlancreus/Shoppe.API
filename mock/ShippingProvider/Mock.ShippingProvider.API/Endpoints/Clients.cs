using Microsoft.AspNetCore.Mvc;
using Mock.ShippingProvider.Application.DTOs;
using Mock.ShippingProvider.Application.Interfaces.Services;

namespace Mock.ShippingProvider.API.Endpoints
{
    public static class Clients
    {
        public static void RegisterApiClientEndpoints(this IEndpointRouteBuilder routes)
        {
            var clients = routes.MapGroup("/api/v1/clients");

            clients.MapGet("", async (IApiClientService service, CancellationToken cancellationToken) =>
            {
                var allClients = await service.GetAllAsync(cancellationToken);
                return Results.Ok(allClients);
            });

            clients.MapGet("/{id:guid}", async (Guid id, IApiClientService service, CancellationToken cancellationToken) =>
            {
                var client = await service.GetIdAsync(id, cancellationToken);
                return client is not null ? Results.Ok(client) : Results.NotFound();
            });

            clients.MapPost("", async ([FromBody] CreateApiClientRequestDTO request, IApiClientService service, CancellationToken cancellationToken) =>
            {
                var client = await service.CreateAsync(request, cancellationToken);
                return Results.Created($"/api/v1/clients/{client.Id}", client);
            });

            clients.MapPatch("", async ([FromBody] UpdateApiClientRequestDTO request, IApiClientService service, CancellationToken cancellationToken) =>
            {
                var updatedClient = await service.UpdateAsync(request, cancellationToken);
                return updatedClient is not null ? Results.Ok(updatedClient) : Results.NotFound();
            });

            clients.MapPatch("/{id:guid}/activate", async (Guid id, IApiClientService service, CancellationToken cancellationToken) =>
            {
                var result = await service.ActivateAsync(id, cancellationToken);
                return result ? Results.Ok() : Results.NotFound();
            });

            clients.MapPatch("/{id:guid}/deactivate", async (Guid id, IApiClientService service, CancellationToken cancellationToken) =>
            {
                var result = await service.DeactivateAsync(id, cancellationToken);
                return result ? Results.Ok() : Results.NotFound();
            });

            clients.MapDelete("/{id:guid}", async (Guid id, IApiClientService service, CancellationToken cancellationToken) =>
            {
                var result = await service.DeleteAsync(id, cancellationToken);
                return result ? Results.NoContent() : Results.NotFound();
            });

          
            clients.MapPatch("/{id:guid}/api-key", async (Guid id, IApiClientService service, CancellationToken cancellationToken) =>
            {
                var result = await service.UpdateApiKeyAsync(id, cancellationToken);
                return result ? Results.NoContent() : Results.NotFound();
            });

            clients.MapPatch("/{id:guid}/secret-key", async (Guid id, IApiClientService service, CancellationToken cancellationToken) =>
            {
                var result = await service.UpdateSecretKeyAsync(id, cancellationToken);
                return result ? Results.NoContent() : Results.NotFound();
            });
        }
    }
}
