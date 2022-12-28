using System.IO.Compression;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using AnkiNet.database;
using AnkiNet.database.model;
using AnkiNet.mapper;
using AnkiNet.model;
using Microsoft.Data.Sqlite;

namespace AnkiNet;

public class AnkiFileWriter
{
    private const string AnkiFileExtension = ".apkg";

    public async Task WriteCollectionToFile(AnkiCollection collection, string outputFolder, string fileName)
    {
        var modelCollection = ConvertAnkiCollectionToModel(collection);

        var dbFilePath = CreateDbFile(outputFolder);
        await CreateAndPopulateDatabaseTables(dbFilePath, modelCollection);

        CreateZipFile(outputFolder, fileName, dbFilePath);
    }

    private static string CreateDbFile(string outputFolder, string name = "temp.db")
    {
        var dbfilePath = Path.Combine(outputFolder, name);

        if (File.Exists(dbfilePath) == true)
        {
            File.Delete(dbfilePath);
        }

        File.Create(dbfilePath).Close();
        return dbfilePath;
    }

    private static Collection ConvertAnkiCollectionToModel(AnkiCollection collection)
    {
        var allCards = collection.Decks.SelectMany(d => d.Cards).ToArray();
        var allNotes = allCards.Select(d => d.Note).Distinct().ToArray();

        var deckIdByCardId = collection.Decks
            .SelectMany(d => d.Cards.ToDictionary(c => c.Id, c => d.Id))
            .ToDictionary(d => d.Key, d => d.Value);

        var result = new Collection(
            collection.Id,
            0, // Creation date
            0, // Last modified date
            0, // Schema modification date
            11, // Version
            0, // Dirty
            0, // Update Sequence Number
            0, // Last sync date
            new model.json.JsonConfiguration
            {
                SortBackwards = false,
                CurrentDeck = 1,
                DueCounts = true,
                SortType = "noteFld",
                CurrentModel = collection.NoteTypes.First().Id,
                TimeLimit = 0,
                NewSpread = 0,
                CollapseTime = 1200,
                EstimateTimes = true,
                AddToCurrent = true,
                NextPosition = 1,
                DayLearnFirst = false,
                SchedulerVersion = 2,
                CreationOffset = -480,
                ActiveDecks = new[] { 1 },
                NewBury = false,
                LastUnburied = 0,
            },
            collection.NoteTypes.ToDictionary(
                nt => nt.Id,
                nt => new model.json.JsonModel
                {
                    Id = nt.Id,
                    Name = nt.Name,
                    ModificationTime = 0,
                    Css = nt.Css ?? string.Empty,
                    DefaultDeckId = null,
                    ModelType = 0,
                    UpdateSequenceNumber = 0,
                    LegacyVersionNumber = null,
                    LatexPost = "\\end{ document }",
                    LatexPre = "\\documentclass[12pt]{article}\n\\special{papersize=3in,5in}\n\\usepackage[utf8]{inputenc}\n\\usepackage{amssymb,amsmath}\n\\pagestyle{empty}\n\\setlength{\\parindent}{0in}\n\\begin{document}\n",
                    LatexSvg = false,
                    BrowserSortField = 0,
                    LastAddedNoteTags = null,
                    CardTemplates = nt.CardTypes.Select(ct => new model.json.JsonCardTemplate
                    {
                        TemplateName = ct.Name,
                        TemplateOrdinal = ct.Ordinal,
                        DeckOverrideId = null,
                        AnswerFormat = ct.AnswerFormat,
                        BrowserAnswerFormat = string.Empty,
                        QuestionFormat = ct.QuestionFormat,
                        BrowserQuestionFormat = string.Empty,
                        BFont = string.Empty,
                        BSize = 0
                    }).ToArray(),
                    Fields = nt.Fields.Select((field, index) => new model.json.JsonField
                    {
                        FieldName = field,
                        FieldNumber = index,
                        IsRightToLeft = false,
                        IsSticky = false,
                        Font = "Arial", // TODO Make this customizable?
                        FontSize = 20, // TODO Make this customizable?
                        Description = string.Empty,
                        Media = null
                    }).ToArray(),
                    RequiredFields = new object[]
                    {
                        0,
                        "any",
                        new object[] {0}
                    }
                }
            ),
            collection.Decks.ToDictionary(
                d => d.Id,
                d => new model.json.JsonDeck
                {
                    Id = d.Id,
                    LastModificationTime = 0,
                    Name = d.Name,
                    UpdateSequenceNumber = 0,
                    NewToday = new[] { 0, 0 },
                    ReviewedToday = new[] { 0, 0 },
                    LearnedToday = new[] { 0, 0 },
                    TimeToday = new[] { 0, 0 },
                    IsCollapsed = false,
                    IsCollapsedInBrowser = false,
                    Description = string.Empty, // TODO Handle deck description?
                    IsDynamic = 0,
                    ConfigurationGroupId = 1,
                    ExtendedNewCardLimit = 0,
                    ExtendedReviewCardLimit = 0,
                }
            ),
            new Dictionary<long, model.json.JsonDeckConfguration>
            {
                {
                    1,
                    new model.json.JsonDeckConfguration
                    {
                        Id = 1,
                        LastModificationTime = 0,
                        Name = "Default",
                        UpdateSequenceNumber = 0,
                        AutoplayQuestionAudio = true,
                        ReplayQuestionAudio = true,
                        ShowTimer = 0,
                        IsDynamic = false,
                        StopTimerAfterSeconds = 0,
                        LapseCardsConfiguration = new model.json.JsonLapseCardsConfiguration
                        {
                            Delays = new[]{ 10f },
                            LapsedIntervalMultiplierPercent = 0,
                            LeechAction = 1,
                            LeechFailsAllowedCount = 8,
                            MinimumInterfalAfterLeech = 1
                        },
                        NewCardsConfiguration = new model.json.JsonNewCardsConfiguration
                        {
                            Bury = false,
                            Delays = new[] { 1f, 10f },
                            InitialEaseFactor = 2500,
                            IntDelays = new[] { 1, 4, 0 },
                            NewCardsPerDay = 20,
                            NewCardsShowOrder = 1,
                            Separate = 0
                        },
                        ReviewCardsConfiguration = new model.json.JsonReviewCardsConfiguration
                        {
                            Bury = false,
                            CardsToReviewPerDay = 200,
                            Ease4 = 1.3f,
                            Fuzz = 0,
                            HardFactor = 1.2f,
                            IntervalMultiplicationFactor = 1,
                            MaximumReviewInterval = 36500,
                            MinSpace = 0
                        }
                    }
                }
            },
            "{}"
        )
        {
            // TODO Get deck id and note id
            Cards = allCards.Select(c => new Card(
                c.Id,
                c.Note.Id, // long NoteId
                deckIdByCardId[c.Id], // long DeckId, // From col table
                c.NoteCardTypeOrdinal, // long Ordinal,
                0, // long ModificationTime, // TODO Convert to DateTime?
                0, // long UpdateSequenceNumber,
                0, // CardLearningType LearningType,
                0, // long Queue,
                0, // long Due,
                0, // long Interval, // Negative = seconds, Positive = days // TODO Convert to TimeSpan?
                0, // long EaseFactor,
                0, // long ReviewsCount,
                0, // long LapsesCount,
                0, // long Left, // '2004' means 2 reps left today and 4 reps till graduation // TODO Split?
                0, // long OriginalDue,
                0, // long OriginalDid,
                0, // long Flags, // Used for colors?
                "" // string Data // Currently unused?
            )).ToArray(),

            // TODO Get note ID and sync it with model id (note type ID)
            Notes = allNotes.Select(n => new Note(
                n.Id,
                Guid.NewGuid().ToString().Substring(0, 10), // string Guid,
                n.NoteTypeId, // long ModelId,
                0, // long ModificationDateTime, // TODO Convert to DateTime?
                0, // long UpdateSequenceNumber,
                "", // string Tags, // Space separated with spaces at beginning and end
                n.Fields, // string[] Fields,
                n.Fields[0], // string SortField, // TODO Check this is correct
                0, // long FieldChecksum, // integer representation of first 8 digits of sha1 hash of the first field
                0, // long Flags, // Unused
                "" // string Data // Unused
            )).ToArray(),
            RevLogs = null,
            Graves = null
        };

        return result;
    }

    private static string ReadResource(string path)
    {
        var a = Assembly.GetExecutingAssembly();
        var resourceStream = a.GetManifestResourceStream(path);
        return new StreamReader(resourceStream).ReadToEnd();
    }

    private async Task CreateAndPopulateDatabaseTables(string dbFilePath, Collection collection)
    {
        SqliteConnection? conn = null;

        try
        {
            SQLitePCL.Batteries.Init();
            //SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());

            conn = new SqliteConnection($"Data Source={dbFilePath};");
            conn.Open();

            var col = ReadResource("AnkiNet.database.sql.ColTable.sql");
            var notes = ReadResource("AnkiNet.database.sql.NotesTable.sql");
            var cards = ReadResource("AnkiNet.database.sql.CardsTable.sql");
            var revLogs = ReadResource("AnkiNet.database.sql.RevLogTable.sql");
            var graves = ReadResource("AnkiNet.database.sql.GravesTable.sql");
            var indexes = ReadResource("AnkiNet.database.sql.Indexes.sql");

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

            var dbExtract = ConvertModelsToDb(collection);

            await new ColRepository().Add(conn, new List<col> { dbExtract.col });
            await new NoteRepository().Add(conn, dbExtract.notes);
            await new CardRepository().Add(conn, dbExtract.cards);
            await new RevLogRepository().Add(conn, dbExtract.revLogs);
            await new GraveRepository().Add(conn, dbExtract.graves);
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

    private static DatabaseExtract ConvertModelsToDb(Collection collection)
    {
        var col = CollectionMapper.ToDb(collection);
        var cards = collection.Cards.Select(c => CardMapper.ToDb(c)).ToList();
        var graves = new List<grave>();
        var notes = collection.Notes.Select(n => NoteMapper.ToDb(n)).ToList();
        var revLogs = new List<revLog>();

        // TODO Add RevLog MetaData
        //if (_revLogMetadatas.Count != 0)
        //{
        //    string insertRevLog = "";

        //    foreach (var revlogMetadata in _revLogMetadatas)
        //    {
        //        insertRevLog = "INSERT INTO revlog VALUES(" + revlogMetadata.id + ", " + revlogMetadata.cid + ", " + revlogMetadata.usn + ", " + revlogMetadata.ease + ", " + revlogMetadata.ivl + ", " + revlogMetadata.lastIvl + ", " + revlogMetadata.factor + ", " + revlogMetadata.time + ", " + revlogMetadata.type + ")";

        //        SQLiteHelper.ExecuteSQLiteCommand(_conn, insertRevLog);
        //    }
        //}

        return new DatabaseExtract(
            col,
            cards,
            graves,
            notes,
            revLogs
        );
    }

    private void CreateZipFile(string outputFolder, string fileName, string dbFilePath)
    {
        if (fileName.EndsWith(AnkiFileExtension))
        {
            fileName = fileName.Replace(AnkiFileExtension, string.Empty);
        }

        var fileNameWithExtension = fileName + AnkiFileExtension;
        var outputFilePath = Path.Combine(outputFolder, fileNameWithExtension);

        if (File.Exists(outputFilePath) == true)
        {
            File.Delete(outputFilePath);
        }

        var tempFolder = Path.Combine(outputFolder, fileName);
        var directory = Directory.CreateDirectory(tempFolder);
        if (directory.Exists)
        {
            directory.Delete(true);
            directory.Create();
        }

        File.Move(dbFilePath, Path.Combine(tempFolder, "collection.anki21"));

        // Create fake media file
        var mediaFilePath = Path.Combine(tempFolder, "media");
        using var stream = File.OpenWrite(mediaFilePath);
        {
            using var writer = new StreamWriter(stream);
            {
                writer.Write("{}");
            }
        }

        //using var zipFile = File.Open(outputFilePath, FileMode.Create);
        //using var archive = new ZipArchive(zipFile, ZipArchiveMode.Create, true);
        //archive.CreateEntryFromFile(dbFilePath, "collection.anki2");
        //archive.CreateEntryFromFile(mediaFilePath, "media");

        ZipFile.CreateFromDirectory(tempFolder, outputFilePath);

        //File.Delete(mediaFilePath);
        //File.Delete(dbFilePath);

        Directory.Delete(tempFolder, true);
    }
}