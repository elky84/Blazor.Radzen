using Dapper;
using MySqlConnector;
using System.Data;

namespace MinimalApi.Common.Database;

public class DbContext : IDisposable
{

    public DbContext(IConfiguration configuration)
    {
        ConnectionString = configuration.GetConnectionString("DB");
        Connection = new MySqlConnection(ConnectionString);
        Open();
    }

    private string? ConnectionString { get; }

    private MySqlConnection Connection { get; }

    private MySqlTransaction? Transaction { get; set; }

    public async void Dispose()
    {
        if (Transaction != null)
        {
            await Transaction.DisposeAsync();
        }

        await Connection.DisposeAsync();
    }

    public void Open()
    {
        if (Connection?.State != ConnectionState.Open)
        {
            Connection?.Open();
        }
    }

    public async Task BeginTransaction()
    {
        Transaction = await Connection.BeginTransactionAsync();
    }


    public async Task Commit()
    {
        if (Transaction != null)
        {
            await Transaction.CommitAsync();
        }
        Transaction = null;
    }

    public async Task Rollback()
    {
        if (Transaction != null)
        {
            await Transaction.RollbackAsync();
        }

        Transaction = null;
    }

    public async Task<IEnumerable<T>> QueryAsync<T>(string sql,
        object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null)
    {
        transaction = transaction ?? Transaction;
        if (transaction != null)
        {
            return Connection.Query<T>(sql, param, transaction, true, commandTimeout, commandType);
        }

        await using var mySqlConnection = new MySqlConnection(ConnectionString);
        return await mySqlConnection.QueryAsync<T>(sql, param, transaction, commandTimeout, commandType);
    }

    public async Task<T?> QueryFirstOrDefaultAsync<T>(string sql,
        object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null)
    {
        transaction ??= Transaction;
        if (transaction != null)
        {
            return Connection.QueryFirstOrDefault<T>(sql, param, transaction, commandTimeout, commandType);
        }

        await using var mySqlConnection = new MySqlConnection(ConnectionString);
        return await mySqlConnection.QueryFirstOrDefaultAsync<T>(sql, param, transaction, commandTimeout, commandType);
    }

    public async Task<T?> ExecuteScalarAsync<T>(string sql,
        object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null)
    {
        transaction ??= Transaction;
        if (transaction != null)
        {
            return Connection.ExecuteScalar<T>(sql, param, transaction, commandTimeout, commandType);
        }

        await using var mySqlConnection = new MySqlConnection(ConnectionString);
        return await mySqlConnection.ExecuteScalarAsync<T>(sql, param, transaction, commandTimeout, commandType);
    }

    public async Task<int> ExecuteAsync(string sql,
        object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null)
    {
        transaction ??= Transaction;
        if (transaction != null)
        {
            return Connection.Execute(sql, param, transaction, commandTimeout, commandType);
        }

        await using var mySqlConnection = new MySqlConnection(ConnectionString);
        return await mySqlConnection.ExecuteAsync(sql, param, transaction, commandTimeout, commandType);
    }
}