namespace AnkiNet;

/// <summary>
/// Represents an Anki deck, a collection of <see cref="AnkiCard"/>s.
/// </summary>
public readonly record struct AnkiDeck
{
    private readonly List<AnkiCard> _cards;

    /// <summary>
    /// Id of the deck.
    /// </summary>
    public long Id { get; }

    /// <summary>
    /// Name of the deck.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Returns a deep copy of this deck's cards.
    /// </summary>
    public AnkiCard[] Cards => _cards.ToArray();

    internal AnkiDeck(long id, string name)
    {
        Id = id;
        Name = name;
        _cards = new List<AnkiCard>();
    }

    internal void AddCard(AnkiCard card)
    {
        _cards.Add(card);
    }
}