﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Review.GetReviewByUser
{
    public class GetReviewsByUserQueryRequest : IRequest<GetReviewsByUserQueryResponse>
    {
        public string? UserId { get; set; }
    }
}