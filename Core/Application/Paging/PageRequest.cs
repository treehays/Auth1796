using System.ComponentModel;

namespace Auth1796.Core.Application.Paging;

public record PageRequest
{
    public int PageSize { get; init; } = 20;
    public int Page { get; init; } = 1;
    public string? SortBy { get; init; }
    [DefaultValue("asc")]
    public string? SortDirection { get; init; }
    public string? Keyword { get; init; }
    public bool IsAscending { get; init; } = true;
    public bool UsePaging { get; set; } = true;
}
