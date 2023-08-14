using Microsoft.Data.Sqlite;

namespace AnkiNet.CollectionFile.Database;

internal abstract class SqliteRepository<T>
{
    private readonly SqliteConnection _connection;

    protected abstract string TableName { get; }
    protected abstract string Columns { get; }
    protected abstract string GetValues(T item);

    protected abstract T Map(SqliteDataReader reader);

    protected SqliteRepository(SqliteConnection connection)
    {
        _connection = connection;
    }

    public async Task<List<T>> ReadAll()
    {
        var result = new List<T>();

        var readAllSqlQuery = $"SELECT {Columns} FROM {TableName}";

        try
        {
            using var command = new SqliteCommand(readAllSqlQuery, _connection);
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var item = Map(reader);
                result.Add(item);
            }
        }
        catch (Exception e)
        {
            throw new IOException($"Cannot ReadAll {typeof(T).Name}", e);
        }

        return result;
    }

    public async Task Add(List<T> items)
    {
        if (!items.Any())
        {
            return;
        }

        var writeSqlQuery = $@"
            INSERT INTO {TableName}
            ({Columns})
            VALUES ";

        var values = items.Select(i => $"({GetValues(i)})");
        writeSqlQuery += string.Join(',', values);

        try
        {
            using var command = new SqliteCommand(writeSqlQuery, _connection);
            var i = await command.ExecuteNonQueryAsync();
        }
        catch (Exception e)
        {
            throw new IOException($"Cannot Add {typeof(T).Name}", e);
        }
    }
}