namespace AnkiNet;

/// <summary>
/// Represents an Anki note, which can be mapped to one or more <see cref="AnkiCard"/>s.
/// </summary>
/// <param name="Id">Id of the note.</param>
/// <param name="NoteTypeId">Id of the <see cref="AnkiNoteType"/> used as a template for this note.</param>
/// <param name="Fields">Values of the fields of this note, matching (or having less fields than) <see cref="AnkiNoteType.FieldNames"/>.</param>
public readonly record struct AnkiNote(long Id, long NoteTypeId, params string[] Fields);