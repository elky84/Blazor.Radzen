using AutoMapper;
using Dapper.Json;
using MinimalApi.Domain.Account.Dao;
using MinimalApi.Domain.Account.Dto;
using MinimalApi.Domain.Account.Repository;
using MinimalApi.Domain.Account.Type;

namespace MinimalApi.Endpoint.Api;

public static class SignIn
{
    public static async Task<SignInResDto> Handle(SignInReqDto dto, AccountRepository repository, IMapper mapper)
    {
        var accountDao = await repository.GetById(dto.AccountId);
        if (accountDao == null)
        {
            var newAccount = new AccountDao
            {
                AccountId = dto.AccountId,
                AccountPassword = dto.AccountPassword,
                AccountStatus = AccountStatus.Normal,
                Detail = new DetailDao([new DetailInfoDao(5, 10)]),
                CreatedAt = DateTime.Now
            };

            accountDao = mapper.Map<AccountDao>(await repository.InsertAsync(newAccount));
        }
        else
        {
            if (accountDao.AccountPassword != dto.AccountPassword)
                throw new Exception("Invalid password");
        }

        return new SignInResDto
        {
            Account = mapper.Map<AccountDto>(accountDao)
        };
    }
}