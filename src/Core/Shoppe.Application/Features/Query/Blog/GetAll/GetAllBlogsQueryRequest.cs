﻿using MediatR;
using Shoppe.Application.Abstractions.Params;
using Shoppe.Application.RequestParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Features.Query.Blog.GetAll
{
    public class GetAllBlogsQueryRequest : PaginationRequestParams, IBlogFilterParams, IRequest<GetAllBlogsQueryResponse>
    {
        public string? CategoryName { get; set; }
        public string? TagName { get; set; }
        public string? SearchQuery { get; set; }
    }
}
