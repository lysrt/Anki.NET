using System.Reflection;
using Microsoft.Data.Sqlite;
using AnkiNet.CollectionFile.Database.Model;

namespace AnkiNet.CollectionFile.Database;

internal sealed class DatabaseReader
{
    public DatabaseReader()
    {
    }

    public async Task<DatabaseExtract> ReadDbAsync(string dbFile)
    {
        SQLitePCL.Batteries.Init();

        using var conn = new SqliteConnection($"Data Source={dbFile};");
        conn.Open();

        var col = (await new ColRepository(conn).ReadAll()).Single();
        var cards = await new CardRepository(conn).ReadAll();
        var graves = await new GraveRepository(conn).ReadAll();
        var notes = await new NoteRepository(conn).ReadAll();
        var revLogs = await new RevLogRepository(conn).ReadAll();

        return new DatabaseExtract(col, cards, graves, notes, revLogs);
    }

    public async Task CreateAndPopulateDatabaseTables(string dbFile, DatabaseExtract dbExtract)
    {
        SqliteConnection? conn = null;

        try
        {
            SQLitePCL.Batteries.Init();

            conn = new SqliteConnection($"Data Source={dbFile};");
            conn.Open();
            
            var col = ReadResource("AnkiNet.CollectionFile.Database.Sql.ColTable.sql");
            var notes = ReadResource("AnkiNet.CollectionFile.Database.Sql.NotesTable.sql");
            var cards = ReadResource("AnkiNet.CollectionFile.Database.Sql.CardsTable.sql");
            var revLogs = ReadResource("AnkiNet.CollectionFile.Database.Sql.RevLogTable.sql");
            var graves = ReadResource("AnkiNet.CollectionFile.Database.Sql.GravesTable.sql");
            var indexes = ReadResource("AnkiNet.CollectionFile.Database.Sql.Indexes.sql");

            using var colCommand = new SqliteCommand(col, conn);
            colCommand.ExecuteNonQuery();
            using var notesCommand = new SqliteCommand(notes, conn);
            notesCommand.ExecuteNonQuery();
            using var cardsCommand = new SqliteCommand(cards, conn);
            cardsCommand.ExecuteNonQuery();
            using var revLogsCommand = new SqliteCommand(revLogs, conn);
            revLogsCommand.ExecuteNonQuery();
            using var gravesCommand = new SqliteCommand(graves, conn);
            gravesCommand.ExecuteNonQuery();
            using var indexesCommand = new SqliteCommand(indexes, conn);
            indexesCommand.ExecuteNonQuery();

            await new ColRepository(conn).Add(new List<col> { dbExtract.col });
            await new NoteRepository(conn).Add(dbExtract.notes);
            await new CardRepository(conn).Add(dbExtract.cards);
            await new RevLogRepository(conn).Add(dbExtract.revLogs);
            await new GraveRepository(conn).Add(dbExtract.graves);
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            conn?.Close();
            conn?.Dispose();
            SqliteConnection.ClearAllPools();
        }
    }

    private static string ReadResource(string path)
    {
        var a = Assembly.GetExecutingAssembly();
        var resourceStream = a.GetManifestResourceStream(path);
        if (resourceStream == null)
        {
            throw new FileNotFoundException($"Cannot find Embedded Resource '{path}' in assembly '{a.GetName().Name}'");
        }

        return new StreamReader(resourceStream).ReadToEnd();
    }
}