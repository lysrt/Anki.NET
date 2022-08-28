using System;
using Microsoft.Data.Sqlite;
using System.Text;

namespace AnkiNet.Helpers;

internal class SQLiteHelper
{
    internal static void ExecuteSQLiteCommand(SqliteConnection conn, string toExecute)
    {
        try
        {
            using SqliteCommand command = new SqliteCommand(toExecute, conn);
            command.ExecuteNonQuery();
        }
        catch (Exception)
        {
            throw new Exception("Can't execute query : " + toExecute);
        }
    }

    internal static SqliteDataReader ExecuteSQLiteCommandRead(SqliteConnection conn, string toExecute)
    {
        try
        {
            using SqliteCommand command = new SqliteCommand(toExecute, conn);
            return command.ExecuteReader();
        }
        catch (Exception)
        {
            throw new Exception("Can't execute query : " + toExecute);
        }
    }

    internal static string CreateStringFormat(int from, int to)
    {
        var result = new StringBuilder();

        for (int i = from; i < to; ++i)
        {
            result.Append($"{{{i}}}");

            if (i + 1 < to)
            {
                result.Append(", ");
            }
        }

        return result.ToString();
    }
}