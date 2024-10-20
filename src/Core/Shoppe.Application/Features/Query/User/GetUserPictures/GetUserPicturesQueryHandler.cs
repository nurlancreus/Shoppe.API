using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.User.GetUserPictures
{
    public class GetUserPicturesQueryHandler : IRequestHandler<GetUserPicturesQueryRequest, GetUserPicturesQueryResponse>
    {
        public Task<GetUserPicturesQueryResponse> Handle(GetUserPicturesQueryRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
