using MediatR;
using Shoppe.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.User.GetUserPictures
{
    public class GetUserPicturesQueryHandler : IRequestHandler<GetUserPicturesQueryRequest, GetUserPicturesQueryResponse>
    {
        private readonly IUserService _userService;

        public GetUserPicturesQueryHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<GetUserPicturesQueryResponse> Handle(GetUserPicturesQueryRequest request, CancellationToken cancellationToken)
        {
            var pictures = await _userService.GetImagesAsync(request.UserId!,cancellationToken);

            return new GetUserPicturesQueryResponse
            {
                IsSuccess = true,
                Data = pictures
            };
        }
    }
}
