using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace AnkiNet.Helpers;

internal static class StrokeOrderHelper
{
    internal static string baseUrl = "https://raw.githubusercontent.com/nmarley/chinese-char-animations/master/images/";

    internal static void DownloadImage(string path, string text)
    {
        var code = string.Format("U+{0:x4}", (int)text[0]).Replace("U+", "");
        var url = Path.Combine(baseUrl, code + ".gif");

        using var client = new HttpClient();
        client.DownloadFileTaskAsync(new Uri(url), path).Wait();
    }

    private static async Task DownloadFileTaskAsync(this HttpClient client, Uri uri, string FileName)
    {
        using var s = await client.GetStreamAsync(uri);
        using var fs = new FileStream(FileName, FileMode.CreateNew);

        await s.CopyToAsync(fs);
    }
}
