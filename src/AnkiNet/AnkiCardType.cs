namespace AnkiNet;

/// <summary>
/// Defines a card's Question (front) and Answer (back) templates.<br/>
/// It is used in <seealso cref="AnkiNoteType.CardTypes"/> proprety.
/// </summary>
/// <param name="Name">Name of the card type.</param>
/// <param name="Ordinal">Order of the card type, in the list of <see cref="AnkiNoteType.CardTypes"/>.</param>
/// <param name="QuestionFormat">HTML format of the question side of cards created using this card type. Anki template tags can be used.</param>
/// <param name="AnswerFormat">HTML format of the answer side of cards created using this card type. Anki template tags can be used.</param>
public record struct AnkiCardType(
    string Name,
    long Ordinal,
    string QuestionFormat,
    string AnswerFormat
);