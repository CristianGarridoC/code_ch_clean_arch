using Application.Abstractions;
using Application.Options;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Infrastructure.Persistence;

public class SqlConnectionFactory : ISqlConnectionFactory
{
    private readonly ConnectionOptions _connectionOptions;

    public SqlConnectionFactory(IOptions<ConnectionOptions> connectionOptions)
    {
        _connectionOptions = connectionOptions.Value;
    }

    public NpgsqlConnection CreateConnection()
    {
        return new NpgsqlConnection(_connectionOptions.Default);
    }
}