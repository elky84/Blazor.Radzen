using MinimalApi.Domain.Account.Type;

namespace MinimalApi.Domain.Account.Dto;

public record AccountDto
{
    public ulong AccountUid { get; init; }
    public string AccountId { get; init; } = string.Empty;
    public string AccountPassword { get; init; } = string.Empty;
    public AccountStatus AccountStatus { get; init; } = AccountStatus.None;
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}