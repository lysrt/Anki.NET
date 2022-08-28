using System;
using System.IO;

namespace AnkiNet;

public class ApkgFile
{
    public string Path { get; private set; }

    /// <summary>
    /// Representation of Apkg file
    /// </summary>
    /// <param name="path">Path of apkg file</param>
    public ApkgFile(string path)
    {
        if (!path.Contains(".apkg"))
        {
            throw new Exception("Need apkg file");
        }
        if (!File.Exists(path))
        {
            throw new Exception("Need existing file");
        }

        Path = path;
    }
}
