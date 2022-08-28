using AnkiNet.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace AnkiNet.Helpers;

internal static class GeneralHelper
{
    internal static Dictionary<string, string> extensionTag = new()
    {
        { ".wav", "[sound:{0}]" },
        { ".gif", "<img src=\"{0}\"/>" }
    };

    internal static string ConcatFields(FieldList flds, AnkiItem item, string separator, MediaInfo info)
    {
        var matchedFields = flds
            .Where(t => item[t.Name] as string != "")
            .Select(t => item[t.Name])
            .ToArray();

        if (info != null)
        {
            int indexOfField = Array.IndexOf(matchedFields, item[info.field]);

            if (indexOfField != -1)
            {
                matchedFields[indexOfField] += string.Format(extensionTag[info.extension], matchedFields[0] + info.extension);
            }
        }
        
        return string.Join(separator, matchedFields);
    }
    
    internal static string ReadResource(string path)
    {
        var a = Assembly.GetExecutingAssembly();
        var resourceStream = a.GetManifestResourceStream(path);

        return new StreamReader(resourceStream).ReadToEnd();
    }

    internal static string CheckSum(string sfld)
    {
        using var sha1 = SHA1.Create();

        //var length = sfld.Length >= 9 ? 8 : sfld.Length;
        var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(sfld));
        var sb = new StringBuilder(hash.Length);

        foreach (byte b in hash)
        {
            sb.Append(b);
        }

        return sb.ToString().Substring(0, 10);
    }
}