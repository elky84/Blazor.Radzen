using AutoMapper;
using MinimalApi.Domain.Account.Dao;
using MinimalApi.Domain.Account.Dto;

namespace MinimalApi.Domain.Account.Mapper;

public static class AccountMapper
{
    public static void Set(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<AccountDao, AccountDto>()
            .ForMember(x => x.Detail, opt => opt.MapFrom(x => x.Detail != null ? x.Detail.Value ?? null : null));

        cfg.CreateMap<AccountDto, AccountDao>()
            .ForMember(x => x.Detail, opt => opt.ConvertUsing(new DetailConverter()));

        cfg.CreateMap<DetailDao, DetailDto>();
        cfg.CreateMap<DetailDto, DetailDao>();
        
        cfg.CreateMap<DetailInfoDao, DetailInfoDto>();
        cfg.CreateMap<DetailInfoDto, DetailInfoDao>();

    }
}