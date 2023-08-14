using System.IO.Compression;
using AnkiNet.CollectionFile;
using AnkiNet.MediaFile;

namespace AnkiNet;

/// <summary>
/// Provides methods to write an <see cref="AnkiCollection"/> to a .apkg file.
/// </summary>
public static class AnkiFileWriter
{
    private const string AnkiFileExtension = ".apkg";

    /// <summary>
    /// Writes an <see cref="AnkiCollection"/> to the given folder and file name.
    /// </summary>
    /// <param name="outputFolder">The folder to write the file to.</param>
    /// <param name="fileName">The name of the file (with or without .apkg extension).</param>
    /// <param name="collection">An <see cref="AnkiCollection"/> to write to the file.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous write operation.</returns>
    public static async Task WriteToFileAsync(string outputFolder, string fileName, AnkiCollection collection)
    {
        if (!fileName.EndsWith(AnkiFileExtension, StringComparison.InvariantCultureIgnoreCase))
        {
            fileName += AnkiFileExtension;
        }

        var outputFilePath = Path.Combine(outputFolder, fileName);

        await WriteToFileAsync(outputFilePath, collection);
    }

    /// <summary>
    /// Writes an <see cref="AnkiCollection"/> to the given file name.
    /// </summary>
    /// <param name="fileName">The path of the file to write to.</param>
    /// <param name="collection">An <see cref="AnkiCollection"/> to write to the file.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous write operation.</returns>
    public static async Task WriteToFileAsync(string filePath, AnkiCollection collection)
    {
        using var stream = new FileStream(filePath, FileMode.Create);
        await WriteToStreamAsync(stream, collection);
    }

    /// <summary>
    /// Writes an <see cref="AnkiCollection"/> to the given <see cref="Stream"/>.
    /// </summary>
    /// <param name="stream">The stream to write to.</param>
    /// <param name="collection">The <see cref="AnkiCollection"/> to read from.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous write operation.</returns>
    public static async Task WriteToStreamAsync(Stream stream, AnkiCollection collection)
    {
        var collectionFilePath = Path.GetTempFileName();
        await new CollectionFileHandler().WriteCollectionFile(collectionFilePath, collection);

        var mediaFilePath = Path.GetTempFileName();
        await MediaFileHandler.WriteMediaFile(mediaFilePath, collection);

        using var archive = new ZipArchive(stream, ZipArchiveMode.Create, true);
        archive.CreateEntryFromFile(collectionFilePath, "collection.anki21");
        archive.CreateEntryFromFile(mediaFilePath, "media");

        File.Delete(collectionFilePath);
        File.Delete(mediaFilePath);
    }
}