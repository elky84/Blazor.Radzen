using MinimalApi.Domain.Common;

namespace MinimalApi.Domain.Account.Dto;

public record SignInResDto : ResponseHeader
{
    public AccountDto? Account { get; init; }
}