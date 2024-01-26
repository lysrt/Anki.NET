using AnkiNet.CollectionFile.Database.Model;
using Microsoft.Data.Sqlite;

namespace AnkiNet.CollectionFile.Database;

internal class NoteRepository : SqliteRepository<note>
{
    public NoteRepository(SqliteConnection connection) : base(connection)
    {
    }

    protected override string TableName => "[notes]";

    protected override string Columns =>
        "[id], [guid], [mid], " +
        "[mod], [usn], [tags], " +
        "[flds], [sfld], [csum], " +
        "[flags], [data]";

    private static string SanitizeHtmlInput(string value)
    {
        return value.Replace(@"""", @"\""").Replace("'", @"''");
    }
    
    protected override string GetValues(note i)
    {
        return
            $"{i.id},'{i.guid}',{i.mid}," +
            $"{i.mod},{i.usn},'{i.tags}'," +
            $"'{SanitizeHtmlInput(i.flds)}','{SanitizeHtmlInput(i.sfld)}',{i.csum}," +
            $"{i.flags},'{i.data}'";
    }

    protected override note Map(SqliteDataReader reader)
    {
        /*
         * Regarding column 'sfld'.
         * See: https://anki.tenderapp.com/discussions/ankidesktop/32752-bug-in-ankis-database-schema-sfld-an-integer
         * | The sort field is an integer so that when users are sorting on a field that contains only numbers,
         * | they are sorted in numeric instead of lexical order.
         */
        return new note(
            reader.Get<long>("id"),
            reader.Get<string>("guid"),
            reader.Get<long>("mid"),
            reader.Get<long>("mod"),
            reader.Get<long>("usn"),
            reader.Get<string>("tags"),
            reader.Get<string>("flds"),
            reader.Get<string>("sfld"), // 'sfld' is an integer column but contains strings
            reader.Get<long>("csum"),
            reader.Get<long>("flags"),
            reader.Get<string>("data")
        );
    }
}