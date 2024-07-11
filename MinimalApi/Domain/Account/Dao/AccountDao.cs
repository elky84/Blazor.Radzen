
using AutoMapper;
using Dapper.Json;
using MinimalApi.Domain.Account.Dto;
using MinimalApi.Domain.Account.Type;

namespace MinimalApi.Domain.Account.Dao;

public record AccountDao
{
    public ulong AccountUid { get; set; }
    public string AccountId { get; init; } = string.Empty;
    public string AccountPassword { get; init; } = string.Empty;
    public AccountStatus AccountStatus { get; init; } = AccountStatus.None;
    
    public Json<DetailDao>? Detail { get; set; }

    public DateTime CreatedAt { get; init; } = DateTime.Now;
    public DateTime UpdatedAt { get; init; } = DateTime.Now;
}

public class DetailDao(List<DetailInfoDao> detailInfos)
{
    public List<DetailInfoDao> DetailInfos { get; set; } = detailInfos;
}

// ReSharper disable once ClassNeverInstantiated.Global
public class DetailInfoDao(uint id, byte count)
{
    public uint Id { get; set; } = id;
    public byte Count { get; set; } = count;
}


public class DetailConverter : IValueConverter<DetailDto, Json<DetailDao>>
{
    public Json<DetailDao> Convert(DetailDto sourceMember, ResolutionContext context)
    {
        // BattleDataDto를 BattleDataDao로 매핑하고 Json<BattleDataDao> 생성
        var dao = context.Mapper.Map<DetailDao>(sourceMember);
        return new Json<DetailDao>(dao);
    }
}