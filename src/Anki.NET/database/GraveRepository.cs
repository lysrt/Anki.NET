using AnkiNet.database.model;
using Microsoft.Data.Sqlite;

namespace AnkiNet.database;

internal class GraveRepository : SqliteRepository<grave>
{
    protected override string TableName => "[graves]";

    protected override string Columns => "[usn], [oid], [type]";

    protected override string GetValues(grave i)
    {
        return $"{i.usn},{i.oid},{i.type}";
    }

    protected override grave Map(SqliteDataReader reader)
    {
        return new grave(
            reader.Get<int>("usn"),
            reader.Get<int>("oid"),
            reader.Get<int>("type")
        );
    }
}
