using MediatR;
using Microsoft.AspNetCore.Mvc;
using Mock.ShippingProvider.API.Attributes;
using Mock.ShippingProvider.Application.Features.ApiClients.Commands.Create;
using Mock.ShippingProvider.Application.Features.ApiClients.Commands.Delete;
using Mock.ShippingProvider.Application.Features.ApiClients.Commands.Toggle;
using Mock.ShippingProvider.Application.Features.ApiClients.Commands.Update;
using Mock.ShippingProvider.Application.Features.ApiClients.Commands.UpdateApiKey;
using Mock.ShippingProvider.Application.Features.ApiClients.Commands.UpdateSecretKey;
using Mock.ShippingProvider.Application.Features.ApiClients.Queries.Get;
using Mock.ShippingProvider.Application.Features.ApiClients.Queries.GetAll;

namespace Mock.ShippingProvider.API.Endpoints
{
    public static class Clients
    {
        public static void RegisterApiClientEndpoints(this IEndpointRouteBuilder routes)
        {
            var clients = routes.MapGroup("/api/v1/clients");

            clients.MapGet("", async (ISender sender, CancellationToken cancellationToken) =>
            {
                var allClients = await sender.Send(new GetAllApiClientsQuery(), cancellationToken);
                return Results.Ok(allClients);
            });

            clients.MapGet("/{id:guid}", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(new GetApiClientByIdQuery { Id = id }, cancellationToken);

                return response.IsSuccess ? Results.Ok(response) : Results.NotFound(response);
            });

            clients
                .MapPost("", async ([FromBody] CreateApiClientCommand request, ISender sender, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);

                return response.IsSuccess ? Results.Created($"/api/v1/clients/{response.Data?.Id}", response) : Results.BadRequest(response);
            })
                .WithMetadata(new AllowAnonymousApiKeyAttribute());

            clients.MapPatch("/{id:guid}", async (Guid id, [FromBody] UpdateApiClientCommand request, ISender sender, CancellationToken cancellationToken) =>
            {
                request.Id = id;
                var response = await sender.Send(request, cancellationToken);

                return response.IsSuccess ? Results.Ok(response) : Results.BadRequest(response);
            });

            clients.MapPatch("/{id:guid}/toggle", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(new ToggleApiClientCommand { Id = id }, cancellationToken);

                return response.IsSuccess ? Results.Ok(response) : Results.NotFound(response);
            });

            clients.MapDelete("/{id:guid}", async (Guid id, ISender service, CancellationToken cancellationToken) =>
            {
                var response = await service.Send(new DeleteApiClientCommand { Id = id }, cancellationToken);

                return response.IsSuccess ? Results.Ok(response) : Results.NotFound(response);
            });


            clients.MapPatch("/{id:guid}/api-key", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(new UpdateApiKeyCommand { Id = id }, cancellationToken);

                return response.IsSuccess ? Results.Ok(response) : Results.NotFound(response);
            });

            clients.MapPatch("/{id:guid}/secret-key", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(new UpdateSecretKeyCommand { Id = id }, cancellationToken);

                return response.IsSuccess ? Results.Ok(response) : Results.NotFound(response);
            });
        }
    }
}
