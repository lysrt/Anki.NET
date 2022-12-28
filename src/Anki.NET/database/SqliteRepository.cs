using Microsoft.Data.Sqlite;

namespace AnkiNet.database;

internal abstract class SqliteRepository<T>
{
    protected abstract string TableName { get; }
    protected abstract string Columns { get; }
    protected abstract string GetValues(T item);

    protected abstract T Map(SqliteDataReader reader);

    public async Task<List<T>> ReadAll(SqliteConnection connection)
    {
        var result = new List<T>();

        var readAllSqlQuery = $"SELECT {Columns} FROM {TableName}";

        try
        {
            using var command = new SqliteCommand(readAllSqlQuery, connection);
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

    public async Task Add(SqliteConnection connection, List<T> items)
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
            using var command = new SqliteCommand(writeSqlQuery, connection);
            var i = await command.ExecuteNonQueryAsync();
        }
        catch (Exception e)
        {
            throw new IOException($"Cannot Add {typeof(T).Name}", e);
        }
    }
}