using Shoppe.Domain.Enums;
using System;
using System.Collections.Generic;

public static class ParsingHelpers
{
    public static List<SortOption> ParseSortByQuery(string? sortByQuery)
    {
        if (string.IsNullOrWhiteSpace(sortByQuery))
            return new List<SortOption>();

        var sortOptions = new List<SortOption>();
        var criteria = sortByQuery.Split(';', StringSplitOptions.RemoveEmptyEntries);

        foreach (var criterion in criteria)
        {
            var parts = criterion.Split('-');
            if (parts.Length != 2)
                throw new ArgumentException("Invalid sorting criteria format.");

            var field = parts[0].ToLower();
            var direction = parts[1].ToLower();

            var sortOption = field switch
            {
                "price" => direction == "asc" ? SortOption.PriceAsc : SortOption.PriceDesc,
                "createdat" => direction == "asc" ? SortOption.CreatedAtAsc : SortOption.CreatedAtDesc,
                _ => throw new ArgumentException($"Invalid sorting field '{field}'"),
            };

            sortOptions.Add(sortOption);
        }

        return sortOptions;
    }
}
