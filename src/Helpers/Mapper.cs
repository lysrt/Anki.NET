
using AnkiNet.Models;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace AnkiNet.Helpers;

internal class Mapper
{
    private static Mapper instance = null;

    private Mapper()
    {
    }

    public static Mapper Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Mapper();
            }
            return instance;
        }
    }
    
    public static List<AnkiSharpDynamic> MapSQLiteReader(SqliteConnection conn, string toExecute)
    {
        var result = new List<AnkiSharpDynamic>();
        var reader = SQLiteHelper.ExecuteSQLiteCommandRead(conn, toExecute);

        while (reader.Read())
        {
            var ankiSharpDynamic = new AnkiSharpDynamic();

            for (int i = 0; i < reader.FieldCount; ++i)
            {
                ankiSharpDynamic[reader.GetName(i)] = reader.GetValue(i);
            }

            result.Add(ankiSharpDynamic);
        }

        reader.Close();
        return result;
    }
}
