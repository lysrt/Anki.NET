using AnkiNet.CollectionFile.Database.Model;
using Microsoft.Data.Sqlite;

namespace AnkiNet.CollectionFile.Database;

internal class ColRepository : SqliteRepository<col>
{
    public ColRepository(SqliteConnection connection) : base(connection)
    {
    }

    protected override string TableName => "[col]";

    protected override string Columns =>
        "[id], [crt], [mod], [scm], [ver], " +
        "[dty], [usn], [ls], [conf], [models], " +
        "[decks], [dconf], [tags]";

    protected override string GetValues(col i)
    {
        return
            $"{i.id},{i.crt},{i.mod},{i.scm},{i.ver}," +
            $"{i.dty},{i.usn},{i.ls},'{i.conf}','{i.models}'," +
            $"'{i.decks}','{i.dconf}','{i.tags}'";
    }

    protected override col Map(SqliteDataReader reader)
    {
        return new col(
            reader.Get<long>("id"),
            reader.Get<long>("crt"),
            reader.Get<long>("mod"),
            reader.Get<long>("scm"),
            reader.Get<long>("ver"),
            reader.Get<long>("dty"),
            reader.Get<long>("usn"),
            reader.Get<long>("ls"),
            reader.Get<string>("conf"),
            reader.Get<string>("models"),
            reader.Get<string>("decks"),
            reader.Get<string>("dconf"),
            reader.Get<string>("tags")
        );
    }
}