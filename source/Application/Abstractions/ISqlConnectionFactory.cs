using Npgsql;

namespace Application.Abstractions;

public interface ISqlConnectionFactory
{
    NpgsqlConnection CreateConnection();
}