using AutoMapper;
using Dapper;
using MinimalApi.Domain.Account.Dao;
using MinimalApi.Domain.Account.Dto;
using MySqlConnector;
using System.Data;

namespace MinimalApi.Domain.Account.Repository;

public class AccountRepository
{
    private readonly MySqlConnection _connection;

    private readonly IMapper _mapper;

    public AccountRepository(IMapper mapper, MySqlConnection mySqlConnection)
    {
        _mapper = mapper;
        _connection = mySqlConnection;
        if(_connection.State != ConnectionState.Open)
            _connection.Open();
    }

    public async Task<IEnumerable<AccountDao>> All()
    {
        return await _connection.QueryAsync<AccountDao>("SELECT * FROM Account");
    }

    public async Task<AccountDao?> GetByUid(long accountUid)
    {
        return await _connection.QueryFirstOrDefaultAsync<AccountDao>(
            $"SELECT * FROM Account WHERE AccountUid = {accountUid}");
    }

    public async Task<AccountDao?> GetById(string accountId)
    {
        return await _connection.QueryFirstOrDefaultAsync<AccountDao>(
            $"SELECT * FROM Account WHERE AccountId = '{accountId}'");
    }

    public async Task<AccountDao> InsertAsync(AccountDto accountDto)
    {
        const string query = """
                             INSERT INTO Account(
                                    AccountId
                                  , AccountPassword
                                  , AccountStatus
                                  , UpdatedAt
                                  , CreatedAt)
                             VALUES (
                                    @AccountId
                                  , @AccountPassword
                                  , @AccountStatus
                                  , @UpdatedAt
                                  , @CreatedAt);

                             SELECT LAST_INSERT_ID();
                             """;

        var id = await _connection.ExecuteScalarAsync<ulong>(query, accountDto);
        var accountDao = _mapper.Map<AccountDao>(accountDto);
        accountDao.AccountUid = id;
        return accountDao;
    }

    public async Task<int> UpdateAsync(AccountDto accountDto)
    {
        const string query = """
                             UPDATE Account
                                SET AccountId = @AccountId
                                  , AccountPassword = @AccountPassword
                                  , AccountStatus = @AccountStatus
                                  , UpdatedAt = @UpdatedAt
                              WHERE AccountUid = @AccountUid;
                             """;
        return await _connection.ExecuteAsync(query, accountDto);
    }

    public async Task<AccountDao> UpsertAsync(AccountDto accountDto)
    {
        var affected = await UpdateAsync(accountDto);
        if (affected == 0)
            return await InsertAsync(accountDto);

        return _mapper.Map<AccountDao>(accountDto);
    }

    public async Task DeleteAsync(long accountUid)
    {
        await _connection.ExecuteAsync($"DELETE FROM Account WHERE AccountUid = {accountUid}");
    }
}