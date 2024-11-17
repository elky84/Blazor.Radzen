using AutoMapper;
using MinimalApi.Common.Database;
using MinimalApi.Domain.Account.Dao;

namespace MinimalApi.Domain.Account.Repository;

public class AccountRepository
{
    private readonly ChangedBag<AccountDao> _changedBag;
    private readonly DbContext _dbContext;

    private readonly IMapper _mapper;

    public AccountRepository(IMapper mapper,
        DbContext dbContext,
        ChangedBags changedBags)
    {
        _mapper = mapper;
        _dbContext = dbContext;
        _dbContext.Open();

        _changedBag = changedBags.AccountBag;
    }

    public async Task<IEnumerable<AccountDao>> All()
    {
        return await _dbContext.QueryAsync<AccountDao>("SELECT * FROM Account");
    }

    public async Task<AccountDao?> GetByUid(long accountUid)
    {
        return await _dbContext.QueryFirstOrDefaultAsync<AccountDao>(
            $"SELECT * FROM Account WHERE AccountUid = {accountUid}");
    }

    public async Task<AccountDao?> GetById(string accountId)
    {
        return await _dbContext.QueryFirstOrDefaultAsync<AccountDao>(
            $"SELECT * FROM Account WHERE AccountId = '{accountId}'");
    }

    public async Task<AccountDao> InsertAsync(AccountDao accountDao)
    {
        const string query = """
                             INSERT INTO Account(
                                    AccountId
                                  , AccountPassword
                                  , AccountStatus
                                  , Detail
                                  , UpdatedAt
                                  , CreatedAt)
                             VALUES (
                                    @AccountId
                                  , @AccountPassword
                                  , @AccountStatus
                                  , @Detail
                                  , @UpdatedAt
                                  , @CreatedAt);

                             SELECT LAST_INSERT_ID();
                             """;

        var id = await _dbContext.ExecuteScalarAsync<ulong>(query);
        accountDao.AccountUid = id;
        _changedBag.Upsert(accountDao);
        return accountDao;
    }

    public async Task<int> UpdateAsync(AccountDao accountDao)
    {
        const string query = """
                             UPDATE Account
                                SET AccountId = @AccountId
                                  , AccountPassword = @AccountPassword
                                  , AccountStatus = @AccountStatus
                                  , Detail = @Detail
                                  , UpdatedAt = @UpdatedAt
                              WHERE AccountUid = @AccountUid;
                             """;
        _changedBag.Upsert(accountDao);
        return await _dbContext.ExecuteAsync(query, accountDao);
    }

    public async Task<AccountDao> UpsertAsync(AccountDao accountDao)
    {
        var affected = await UpdateAsync(accountDao);
        if (affected == 0)
        {
            return await InsertAsync(accountDao);
        }

        return _mapper.Map<AccountDao>(accountDao);
    }

    public async Task DeleteAsync(long accountUid)
    {
        await _dbContext.ExecuteAsync($"DELETE FROM Account WHERE AccountUid = {accountUid}");
    }
}