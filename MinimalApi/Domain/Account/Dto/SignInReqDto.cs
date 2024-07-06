namespace MinimalApi.Domain.Account.Dto;

public record SignInReqDto
{
    public string AccountId { get; init; } = string.Empty;
    
    public string AccountPassword { get; init; } = string.Empty;
}