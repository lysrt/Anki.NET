namespace AnkiNet;

/// <summary>
/// 
/// </summary>
public class AnkiCollection
{
    private const long DefaultDeckId = 1;
    private const string DefaultDeckName = "Default";

    public long Id { get; }
    private readonly Dictionary<long, AnkiNoteType> _noteTypes;
    private readonly Dictionary<long, AnkiDeck> _decks;
    private readonly Dictionary<long, AnkiNote> _notes;
    private readonly Dictionary<long, AnkiCard> _cards;

    public AnkiCollection() : this(DateTimeOffset.UtcNow.Ticks)
    {
    }

    public AnkiCollection(long id)
    {
        Id = id;
        _noteTypes = new Dictionary<long, AnkiNoteType>();
        _decks = new Dictionary<long, AnkiDeck>
        {
            {DefaultDeckId, CreateDefaultDeck()}
        };
        _notes = new Dictionary<long, AnkiNote>();
        _cards = new Dictionary<long, AnkiCard>();
    }

    private static AnkiDeck CreateDefaultDeck() => new(DefaultDeckId, DefaultDeckName);

    public AnkiCollection(AnkiNoteType noteType) : this()
    {
        AddNoteType(noteType);
    }

    /// <summary>
    /// List all the <see cref="AnkiDeck"/>s of the collection.
    /// </summary>
    public AnkiDeck[] Decks => _decks.Values.ToArray();

    /// <summary>
    /// List all the <see cref="AnkiNoteType"/> of the collection.
    /// </summary>
    public AnkiNoteType[] NoteTypes => _noteTypes.Values.ToArray();

    public AnkiDeck? GetDeckById(long id)
    {
        _decks.TryGetValue(id, out var deck);
        return deck;
    }

    /// <summary>
    /// Add a new <see cref="AnkiNoteType"/> to the collection.
    /// Throws an exception if a note type with the same Id already exists.
    /// </summary>
    /// <param name="noteType"><see cref="AnkiNoteType"/> to add to the collection.</param>
    public void AddNoteType(AnkiNoteType noteType)
    {
        if (!noteType.CardTypes.Any())
        {
            throw new ArgumentException($"AnkiNoteType must have at least one CardType");
        }

        _noteTypes.Add(noteType.Id, noteType);
    }

    /// <summary>
    /// Add a new <see cref="AnkiDeck"/> to the collection.
    /// Throws an exception if a deck with the same Id or name already exists.
    /// </summary>
    /// <param name="deck"><see cref="AnkiDeck"/> to add to the collection.</param>
    public AnkiDeck AddDeck(string name)
    {
        if (_decks.Values.Any(d => d.Name == name))
        {
            throw new ArgumentException($"The collection already has a deck with the name {name}");
        }

        var newId = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() * 1000;
        while (_decks.ContainsKey(newId))
        {
            newId++;
        }

        var deck = new AnkiDeck(newId, name);
        _decks.Add(newId, deck);

        return deck;
    }

    public AnkiDeck AddDeck(long id, string name)
    {
        if (_decks.Values.Any(d => d.Name == name))
        {
            throw new ArgumentException($"The collection already has a deck with the name {name}");
        }

        if (_decks.ContainsKey(id))
        {
            throw new ArgumentException($"The collection already has a deck with id {id}");
        }

        var deck = new AnkiDeck(id, name);
        _decks.Add(id, deck);

        return deck;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="deck"></param>
    /// <param name="noteType"></param>
    /// <param name="fields"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public void AddNote(AnkiDeck deck, AnkiNoteType noteType, params string[] fields)
    {
        if (!_decks.ContainsKey(deck.Id))
        {
            throw new InvalidOperationException($"Unknown deck id '{deck.Id} in this collection");
        }

        if (!_decks.Values.Any(d => d.Name == deck.Name))
        {
            throw new InvalidOperationException($"Unknown deck name: '{deck.Name} in this collection");
        }

        if (!_noteTypes.TryGetValue(noteType.Id, out var existingNoteType))
        {
            throw new InvalidOperationException($"Unknown note type '{noteType.Id}' in this collection");
        }

        var newId = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() * 1000;
        while (_notes.ContainsKey(newId))
        {
            newId++;
        }

        var note = new AnkiNote
        {
            Id = newId,
            NoteTypeId = noteType.Id,
            Fields = fields
        };

        _notes.Add(newId, note);

        foreach (var cardType in noteType.CardTypes)
        {
            var newCardId = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() * 1000;
            while (_cards.ContainsKey(newCardId))
            {
                newCardId++;
            }

            var newCard = new AnkiCard
            {
                Id = newCardId,
                NoteCardTypeOrdinal = cardType.Ordinal,
                Note = note
            };

            _cards.Add(newCardId, newCard);
            _decks[deck.Id].Cards.Add(newCard);
        }
    }
}