namespace AnkiNet;

/// <summary>
/// Defines an Anki note (or model) used as a template to create one or several cards.
/// </summary>
/// <remarks>This is called "model" in the Anki database.</remarks>
public record struct AnkiNoteType
{
    /// <summary>
    /// Id of the note type.
    /// </summary>
    public long Id { get; internal set; }

    /// <summary>
    /// Name of the note type.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Card types (templates) of the note type.
    /// </summary>
    public AnkiCardType[] CardTypes { get; }

    /// <summary>
    /// Field names of the note type, used in the <see cref="AnkiCardType"/> templates.
    /// </summary>
    public string[] FieldNames { get; }

    /// <summary>
    /// CSS to apply on the <see cref="AnkiCardType"/> templates.
    /// </summary>
    public string? Css { get; }

    /// <summary>
    /// Create a new note type, with undefined Id, to pass to <see cref="AnkiCollection.CreateNoteType(AnkiNoteType)"/>.
    /// </summary>
    /// <param name="name">Name of the note type.</param>
    /// <param name="cardTypes">Card types (templates) of the note type.</param>
    /// <param name="fieldNames">Field names of the note type, used in the <see cref="AnkiCardType"/> templates.</param>
    /// <param name="css">CSS to apply on the <see cref="AnkiCardType"/> templates</param>
    public AnkiNoteType(string name, AnkiCardType[] cardTypes, string[] fieldNames, string? css = null) : this(-1, name, cardTypes, fieldNames, css)
    {
    }

    internal AnkiNoteType(long id, string name, AnkiCardType[] cardTypes, string[] fieldNames, string? css)
    {
        if (cardTypes.Length < 1)
        {
            throw new ArgumentException("AnkiNoteType needs at least one AnkiCardType");
        }

        Id = id;
        Name = name;
        CardTypes = cardTypes;
        FieldNames = fieldNames;
        Css = css;
    }
}
