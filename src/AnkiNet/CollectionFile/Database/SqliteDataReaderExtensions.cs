using AnkiNet.CollectionFile.Database.Model;
using Microsoft.Data.Sqlite;

namespace AnkiNet.CollectionFile.Database;

internal static class SqliteDataReaderExtensions
{
    public static T Get<T>(this SqliteDataReader reader, string columnName)
    {
        if (reader.IsDBNull(reader.GetOrdinal(columnName)))
        {
            throw new InvalidOperationException($"Null column: '{columnName}'");
        }

        try
        {
            return (T)reader[columnName];
        }
        catch (Exception e)
        {
            throw new InvalidCastException($"Cannot get column '{columnName}'", e);
        }
    }

    public static T? GetNullable<T>(this SqliteDataReader reader, string columnName) where T:class
    {
        try
        {
            return reader[columnName] as T ?? null;
        }
        catch (Exception e)
        {
            throw new InvalidCastException($"Cannot get column {columnName}", e);
        }
    }
}