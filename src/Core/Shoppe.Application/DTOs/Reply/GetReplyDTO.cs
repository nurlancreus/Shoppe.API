﻿using Shoppe.Application.DTOs.Files;
using Shoppe.Application.DTOs.Reaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.DTOs.Reply
{
    public record GetReplyDTO
    {
        public Guid Id { get; set; }
        public string AuthorId { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public GetImageFileDTO? ProfilePhoto { get; set; }
        public List<GetReplyDTO> Replies { get; set; } = [];
        public List<GetReactionDTO> Reactions { get; set; } = [];
        public string? Body { get; set; }
        public string Type { get; set; } = string.Empty;
        public byte Depth {  get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
