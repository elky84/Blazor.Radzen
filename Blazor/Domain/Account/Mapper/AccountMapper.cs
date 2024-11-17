using AutoMapper;
using MinimalApi.Domain.Account.Dao;
using MinimalApi.Domain.Account.Dto;

namespace MinimalApi.Domain.Account.Mapper;

public class AccountMapper : Profile
{
    public AccountMapper()
    {
        CreateMap<AccountDao, AccountDto>();
        CreateMap<AccountDto, AccountDao>();

        CreateMap<DetailDao, DetailDto>();
        CreateMap<DetailDto, DetailDao>();

        CreateMap<DetailInfoDao, DetailInfoDto>();
        CreateMap<DetailInfoDto, DetailInfoDao>();
    }
}