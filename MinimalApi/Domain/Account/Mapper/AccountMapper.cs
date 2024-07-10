using AutoMapper;
using MinimalApi.Domain.Account.Dao;
using MinimalApi.Domain.Account.Dto;

namespace MinimalApi.Domain.Account.Mapper;

public static class AccountMapper
{
    public static void Set(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<AccountDao, AccountDto>();
        cfg.CreateMap<AccountDto, AccountDao>();
    }
}