namespace AnkiNet;

/// <summary>
/// <see cref="AnkiCardType"/> defines a card's Question and Answer templates.<br/>
/// It must be listed into <seealso cref="AnkiNoteType.CardTypes"/> proprety.
/// </summary>
/// <param name="Name">Name of the card type.</param>
/// <param name="Ordinal">Ordinal (order) to use in <see cref="AnkiNoteType.CardTypes"/></param>
/// <param name="QuestionFormat">Format of the question side of cards using this card type.</param>
/// <param name="AnswerFormat">Format of the answer side of cards using this card type.</param>
public record AnkiCardType(
    string Name,
    long Ordinal,
    string QuestionFormat,
    string AnswerFormat
)
{
    /// <summary>
    /// Name of the card type.
    /// </summary>
    public string Name { get; init; } = Name;

    /// <summary>
    /// Order of the card type, inside <see cref="AnkiNoteType.CardTypes"/>.
    /// </summary>
    public long Ordinal { get; init; } = Ordinal;

    /// <summary>
    /// Question side of the card to be created from this card type.<br/>
    /// Anki template tags can be used.
    /// </summary>
    public string QuestionFormat { get; init; } = QuestionFormat;

    /// <summary>
    /// Answer side of the card to be created from this card type.<br/>
    /// Anki template tags can be used.
    /// </summary>
    public string AnswerFormat { get; init; } = AnswerFormat;
}