using MinimalApi.Domain.Account.Dao;

namespace MinimalApi.Common.Database;

public class ChangedBags
{
    public readonly ChangedBag<AccountDao> AccountBag = new(dao => dao.AccountUid);
}