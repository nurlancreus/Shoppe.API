using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shoppe.Application.DTOs.Location
{
    public record GetCountryDTO
    {
        [JsonPropertyName("name")]
        public NameInfo Name { get; set; } = null!;

        [JsonPropertyName("flags")]
        public FlagInfo Flag { get; set; } = null!;

        [JsonPropertyName("cca2")]
        public string Code { get; set; } = string.Empty;

        public class FlagInfo
        {
            [JsonPropertyName("svg")]
            public string SvgUrl { get; set; } = string.Empty;

            [JsonPropertyName("alt")]
            public string AltText { get; set; } = string.Empty;
        }

        public class NameInfo
        {
            [JsonPropertyName("common")]
            public string Name { get; set; } = string.Empty;
        }
    }
}
