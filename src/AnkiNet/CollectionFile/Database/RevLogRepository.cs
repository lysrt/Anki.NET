using AnkiNet.CollectionFile.Database.Model;
using Microsoft.Data.Sqlite;

namespace AnkiNet.CollectionFile.Database;

internal class RevLogRepository : SqliteRepository<revLog>
{
    public RevLogRepository(SqliteConnection connection) : base(connection)
    {
    }

    protected override string TableName => "[revlog]";

    protected override string Columns =>
        "[id], [cid], [usn], [ease], [ivl], " +
        "[lastIvl], [factor], [time], [type]";

    protected override string GetValues(revLog i)
    {
        return
            $"{i.id},{i.cid},{i.usn},{i.ease},{i.ivl}," +
            $"{i.lastIvl},{i.factor},{i.time},{i.type}";
    }

    protected override revLog Map(SqliteDataReader reader)
    {
        return new revLog(
            reader.Get<long>("id"),
            reader.Get<long>("cid"),
            reader.Get<long>("usn"),
            reader.Get<long>("ease"),
            reader.Get<long>("ivl"),
            reader.Get<long>("lastIvl"),
            reader.Get<long>("factor"),
            reader.Get<long>("time"),
            reader.Get<long>("type")
        );
    }
}