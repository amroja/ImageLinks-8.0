using System.ComponentModel.DataAnnotations;

namespace ImageLinks_.Application.Common.Models
{
    public record PageRequest
    {
        [Range(0, int.MaxValue)]
        public int PageNumber { get; set; }
        [Range(1, int.MaxValue)]
        public int PageSize { get; set; }
        public List<Filters>? Filters { get; set; }
        public List<SortList>? SortList { get; set; }
        public string? SearchValue { get; set; }
    }

    public class Filters
    {
        public string? Field { get; set; }
        public object? Value { get; set; }
        public string? Operator { get; set; }
        public string? JunctionOperator { get; set; }
        public bool? IsCaseSensitive { get; set; }
        public string? ConnectionString { get; set; }
    }

    public class SortList
    {
        public string? Property { get; set; }
        public string? Direction { get; set; }
    }
}
