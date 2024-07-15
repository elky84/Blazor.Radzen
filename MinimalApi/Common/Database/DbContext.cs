using MySqlConnector;
using System.Data;

namespace MinimalApi.Common.Database;

public class DbContext : IDisposable
{
    private readonly MySqlConnection _connection;
    private MySqlTransaction? _transaction;

    public DbContext(IConfiguration configuration)
    {
        _connection = new MySqlConnection(configuration.GetConnectionString("DB"));
        Open();
    }

    public MySqlConnection Connection => _connection;
    public MySqlTransaction? Transaction => _transaction;

    public void Open()
    {
        if(_connection?.State != ConnectionState.Open)
            _connection?.Open();
    }
    
    public async Task BeginTransaction()
    {
        _transaction = await _connection.BeginTransactionAsync();
    }

    
    public async Task Commit()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
        }
        _transaction = null;
    }

    public async Task Rollback()
    {
        if(_transaction != null)
            await _transaction.RollbackAsync();

        _transaction = null;
    }

    public async void Dispose()
    {
        if(_transaction != null)
            await _transaction.DisposeAsync();

        await _connection.DisposeAsync();
    }
}