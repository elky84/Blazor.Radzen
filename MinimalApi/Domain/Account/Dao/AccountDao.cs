
using MinimalApi.Domain.Account.Type;

namespace MinimalApi.Domain.Account.Dao;

public record AccountDao
{
    public ulong AccountUid { get; set; }
    public string AccountId { get; init; } = string.Empty;
    public string AccountPassword { get; init; } = string.Empty;
    public AccountStatus AccountStatus { get; init; } = AccountStatus.None;
    public DateTime CreatedAt { get; init; } = DateTime.Now;
    public DateTime UpdatedAt { get; init; } = DateTime.Now;
}