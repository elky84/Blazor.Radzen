namespace MinimalApi.Domain.Common;

public record ResponseHeader
{
    public int StatusCode { get; set; }
    public string? Message { get; set; } = string.Empty;
}