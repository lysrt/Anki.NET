using AnkiNet.CollectionFile;

namespace AnkiNet;

/// <summary>
/// Provides methods to read an <see cref="AnkiCollection"/> from a .apkg file.
/// </summary>
public static class AnkiFileReader
{
    /// <summary>
    /// Reads an <see cref="AnkiCollection"/> from the given file path.
    /// </summary>
    /// <param name="filePath">The path of the file to read.</param>
    /// <returns>A value <see cref="Task"/> representing the asynchronous read operation,
    /// containing an <see cref="AnkiCollection"/> with the file contents.</returns>
    public static async Task<AnkiCollection> ReadFromFileAsync(string filePath)
    {
        using var stream = new FileStream(filePath, FileMode.Open);
        return await ReadFromStreamAsync(stream);
    }

    /// <summary>
    /// Reads an <see cref="AnkiCollection"/> from the given <see cref="Stream"/>.
    /// </summary>
    /// <param name="stream">The stream to read from.</param>
    /// <returns>A value <see cref="Task"/> representing the asynchronous read operation,
    /// containing an <see cref="AnkiCollection"/> with the file contents.</returns>
    public static async Task<AnkiCollection> ReadFromStreamAsync(Stream stream)
    {
        return await new CollectionFileHandler().ReadCollectionFile(stream);
    }
}