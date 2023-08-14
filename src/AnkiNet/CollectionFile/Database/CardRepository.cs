using AnkiNet.CollectionFile.Database.Model;
using Microsoft.Data.Sqlite;

namespace AnkiNet.CollectionFile.Database;

internal class CardRepository : SqliteRepository<card>
{
    public CardRepository(SqliteConnection connection) : base(connection)
    {
    }

    protected override string TableName => "[cards]";

    protected override string Columns =>
        "[id], [nid], [did], [ord], [mod], " +
        "[usn], [type], [queue], [due], [ivl], " +
        "[factor], [reps], [lapses], [left], [odue]," +
        "[odid], [flags], [data]";

    protected override string GetValues(card i)
    {
        return
            $"{i.id},{i.nid},{i.did},{i.ord},{i.mod}," +
            $"{i.usn},{i.type},{i.queue},{i.due},{i.ivl}," +
            $"{i.factor},{i.reps},{i.lapses},{i.left},{i.odue}," +
            $"{i.odid},{i.flags},'{i.data}'";
    }

    protected override card Map(SqliteDataReader reader)
    {
        return new card(
            reader.Get<long>("id"),
            reader.Get<long>("nid"),
            reader.Get<long>("did"),
            reader.Get<long>("ord"),
            reader.Get<long>("mod"),
            reader.Get<long>("usn"),
            reader.Get<long>("type"),
            reader.Get<long>("queue"),
            reader.Get<long>("due"),
            reader.Get<long>("ivl"),
            reader.Get<long>("factor"),
            reader.Get<long>("reps"),
            reader.Get<long>("lapses"),
            reader.Get<long>("left"),
            reader.Get<long>("odue"),
            reader.Get<long>("odid"),
            reader.Get<long>("flags"),
            reader.Get<string>("data")
        );
    }
}