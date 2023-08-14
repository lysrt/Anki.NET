using System.IO.Compression;
using AnkiNet.CollectionFile.Database;
using AnkiNet.CollectionFile.Mapper;
using ZstdSharp;

namespace AnkiNet.CollectionFile;

internal sealed class CollectionFileHandler
{
    private readonly InternalConverter _converter;
    private readonly DatabaseMapper _dbMapper;
    private readonly DatabaseReader _dbReader;

    public CollectionFileHandler()
    {
        _converter = new InternalConverter();
        _dbMapper = new DatabaseMapper();
        _dbReader = new DatabaseReader();
    }

    public async Task<AnkiCollection> ReadCollectionFile(Stream stream)
    {
        var dbFile = GetDbFile(stream);
        var db = await _dbReader.ReadDbAsync(dbFile);
        var collection = _dbMapper.ConvertDbToModels(db);
        return _converter.ConvertCollectionToAnkiCollection(collection);
    }

    private static string GetDbFile(Stream stream)
    {
        using var zipArchive = new ZipArchive(stream);

        var entryNames = zipArchive.Entries.Select(e => e.Name);
        if (entryNames.Contains("collection.anki21b"))
        {
            throw new NotImplementedException("Anki.NET cannot yet open 2.1b files. Please export from Anki App using the 'Support older Anki versions' option.");
            var entry = zipArchive.GetEntry("collection.anki21b")!;

            var tempFileCompressed = Path.GetTempFileName();
            entry.ExtractToFile(tempFileCompressed, true);

            var tempFileDecompressed = "db_files/new.db"; // TODO Revert - Path.GetTempFileName();

            using var input = File.OpenRead(tempFileCompressed);
            using var output = File.OpenWrite(tempFileDecompressed);
            using var decompressionStream = new DecompressionStream(input);
            decompressionStream.CopyTo(output);

            File.Delete(tempFileCompressed);
            return tempFileDecompressed;
        }
        var databaseEntry = zipArchive.GetEntry("collection.anki21");
        if (databaseEntry == null)
        {
            databaseEntry = zipArchive.GetEntry("collection.anki2");
        }
        if (databaseEntry == null)
        {
            throw new InvalidOperationException("No collection SQLite file found in this Anki archive");
        }

        // Open DB file, as we cannot go from Stream to SqliteConnection
        var tempFile = Path.GetTempFileName();
        databaseEntry.ExtractToFile(tempFile, true);
        return tempFile;
    }

    public async Task<string> WriteCollectionFile(string dbFile, AnkiCollection collection)
    {
        var modelCollection = _converter.ConvertAnkiCollectionToCollection(collection);
        var dbExtract = _dbMapper.ConvertModelsToDb(modelCollection);
        await _dbReader.CreateAndPopulateDatabaseTables(dbFile, dbExtract);
        return dbFile;
    }
}