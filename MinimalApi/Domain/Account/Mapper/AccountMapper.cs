using AutoMapper;
using MinimalApi.Domain.Account.Dao;
using MinimalApi.Domain.Account.Dto;

namespace MinimalApi.Domain.Account.Mapper
{
    public class AccountMapper : Profile
    {
        public AccountMapper()
        {
            CreateMap<AccountDao, AccountDto>()
                .ForMember(x => x.Detail, opt => opt.MapFrom(x => x.Detail != null ? x.Detail.Value ?? null : null));

            CreateMap<AccountDto, AccountDao>()
                .ForMember(x => x.Detail, opt => opt.ConvertUsing(new DetailConverter()));

            CreateMap<DetailDao, DetailDto>();
            CreateMap<DetailDto, DetailDao>();

            CreateMap<DetailInfoDao, DetailInfoDto>();
            CreateMap<DetailInfoDto, DetailInfoDao>();
        }
    }
}
