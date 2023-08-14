namespace AnkiNet;

/// <summary>
/// Represents an Anki card, defined from an <see cref="AnkiNote"/>.
/// </summary>
/// <param name="Id">Id of the card.</param>
/// <param name="Note"><see cref="AnkiNote"/> used to create this card.</param>
/// <param name="NoteCardTypeOrdinal">Index of the card template used to create this card in the <see cref="AnkiNoteType.CardTypes"/> template.</param>
public readonly record struct AnkiCard(long Id, AnkiNote Note, long NoteCardTypeOrdinal);