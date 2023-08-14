namespace AnkiNet.MediaFile;

internal static class MediaFileHandler
{
    /// <summary>
    /// Media file is not handled by Anki.NET, this writes an empty file.
    /// </summary>
    /// <param name="mediaFilePath">File to write media to.</param>
    /// <param name="collection"><see cref="AnkiCollection"/> to read media from.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous write operation.</returns>
    public static async Task WriteMediaFile(string mediaFilePath, AnkiCollection _)
	{
        using var stream = File.OpenWrite(mediaFilePath);
        using var writer = new StreamWriter(stream);
        await writer.WriteAsync("{}");
    }
}