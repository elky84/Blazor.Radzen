using MinimalApi.Domain.Account.Type;

namespace MinimalApi.Domain.Account.Dto;

public record AccountDto
{
    public ulong AccountUid { get; init; }
    public string AccountId { get; init; } = string.Empty;
    public string AccountPassword { get; init; } = string.Empty;
    public AccountStatus AccountStatus { get; init; } = AccountStatus.None;
    
    public DetailDto? Detail { get; set; }
    
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}

public class DetailDto(List<DetailInfoDto> detailInfos)
{
    public List<DetailInfoDto> DetailInfos { get; set; } = detailInfos;
}

// ReSharper disable once ClassNeverInstantiated.Global
public class DetailInfoDto(uint id, byte count)
{
    public uint Id { get; set; } = id;
    public byte Count { get; set; } = count;
}