namespace AnkiNet;

/// <summary>
/// 
/// </summary>
/// <remarks>This is called "model" in the Anki database.</remarks>
public record AnkiNoteType
{
    public long Id { get; }
    public string Name { get;  }
    public string? Css { get; init; }
    public string[] Fields { get; init; }
    public AnkiCardType[] CardTypes { get; init; }

    public AnkiNoteType(long id, string name)
    {
        Id = id;
        Name = name;
        Fields = Array.Empty<string>();
        CardTypes = Array.Empty<AnkiCardType>();
    }
}
