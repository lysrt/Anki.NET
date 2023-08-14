using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("AnkiNet.Tests")]

namespace AnkiNet;

/// <summary>
/// Represents a collection of Anki templates, decks, notes and cards.
/// </summary>
public class AnkiCollection
{
    private const long DefaultDeckId = 1L;
    private const string DefaultDeckName = "Default";

    private readonly Dictionary<long, AnkiNoteType> _noteTypes;
    private readonly Dictionary<long, AnkiDeck> _decks;
    private readonly Dictionary<long, AnkiNote> _notes;
    private readonly Dictionary<long, AnkiCard> _cards;

    /// <summary>
    /// Create a new Anki collection.
    /// </summary>
    public AnkiCollection()
    {
        _noteTypes = new Dictionary<long, AnkiNoteType>();
        _decks = new Dictionary<long, AnkiDeck>
        {
            {DefaultDeckId, new(DefaultDeckId, DefaultDeckName)}
        };
        _notes = new Dictionary<long, AnkiNote>();
        _cards = new Dictionary<long, AnkiCard>();
    }

    /// <summary>
    /// Lists all decks of the collection.
    /// </summary>
    public AnkiDeck[] Decks => _decks.Values.ToArray();

    /// <summary>
    /// Returns a deep copy of all the <see cref="AnkiNoteType"/> of the collection.
    /// </summary>
    public AnkiNoteType[] NoteTypes => _noteTypes.Values.ToArray();

    /// <summary>
    /// Get the default deck (with id 1) of the collection.
    /// </summary>
    public AnkiDeck DefaultDeck => _decks[DefaultDeckId];

    /// <summary>
    /// Gets the deck associated to the given deck id.
    /// </summary>
    /// <param name="deckId">The id of the deck to get.</param>
    /// <param name="deck">The deck associated with the given id if it exists.</param>
    /// <returns>True if the given deck id exists in the collection.</returns>
    public bool TryGetDeckById(long deckId, [MaybeNullWhen(false)] out AnkiDeck deck)
    {
        return _decks.TryGetValue(deckId, out deck);
    }

    /// <summary>
    /// Gets the deck associated to the given deck name.
    /// </summary>
    /// <param name="deckName">The name of the deck to get.</param>
    /// <param name="deck">The deck associated with the given name if it exists.</param>
    /// <returns>True if the given deck name exists in the collection.</returns>
    public bool TryGetDeckByName(string deckName, [MaybeNullWhen(false)] out AnkiDeck deck)
    {
        deck = _decks.Values.SingleOrDefault(d => d.Name == deckName);
        return deck != null;
    }

    /// <summary>
    /// Gets the note type associated to the given note type id.
    /// </summary>
    /// <param name="noteTypeId">The id of the note type to get.</param>
    /// <param name="noteType">The note type associated with the given id if it exists.</param>
    /// <returns>True if the given note type id exists in the collection.</returns>
    public bool TryGetNoteTypeById(long noteTypeId, [MaybeNullWhen(false)] out AnkiNoteType noteType)
    {
        return _noteTypes.TryGetValue(noteTypeId, out noteType);
    }

    /// <summary>
    /// Gets the note type associated to the given note type name.
    /// </summary>
    /// <param name="noteTypeName">The name of the note type to get.</param>
    /// <param name="noteType">The note type associated with the given name if it exists.</param>
    /// <returns>True if the given note type name exists in the collection.</returns>
    public bool TryGetNoteTypeByName(string noteTypeName, [MaybeNullWhen(false)] out AnkiNoteType noteType)
    {
        noteType = _noteTypes.Values.SingleOrDefault(d => d.Name == noteTypeName);
        return noteType != null;
    }

    /// <summary>
    /// Create and add a new <see cref="AnkiNoteType"/> to this collection, given the input note type.
    /// </summary>
    /// <param name="noteType">Note type to add to the collection.</param>
    /// <returns>The id of the added note type.</returns>
    public long CreateNoteType(AnkiNoteType noteType)
    {
        var newId = IdFactory.Create();
        while (_noteTypes.ContainsKey(newId))
        {
            newId++;
        }

        noteType.Id = newId;
        AddNoteType(noteType);

        return newId;
    }

    /// <summary>
    /// Create and add a new <see cref="AnkiDeck"/> to the collection.
    /// Throws an exception if a deck with the same name already exists.
    /// </summary>
    /// <param name="name">Name of the deck to create.</param>
    /// <returns>The id of the created deck.</returns>
    public long CreateDeck(string name)
    {
        var newId = IdFactory.Create();
        while (_decks.ContainsKey(newId))
        {
            newId++;
        }

        AddDeck(newId, name);
        return newId;
    }

    /// <summary>
    /// Create and add a new note and associated cards to the collection.
    /// </summary>
    /// <param name="deckId">Id of the deck to insert the note and cards to.</param>
    /// <param name="noteTypeId">Id of the note type (template) to use to create the nnote and cards.</param>
    /// <param name="fields">Fields of the note to create.</param>
    /// <exception cref="ArgumentException">If the deckId, noteTypeId do not exist, or if the fields count is more than the lengtho of the <see cref="AnkiNoteType.FieldNames"/></exception>
    public void CreateNote(long deckId, long noteTypeId, params string[] fields)
    {
        if (!_decks.ContainsKey(deckId))
        {
            throw new ArgumentException($"Unknown deck id '{deckId} in this collection");
        }

        if (!_noteTypes.TryGetValue(noteTypeId, out var existingNoteType))
        {
            throw new ArgumentException($"Unknown note type '{noteTypeId}' in this collection");
        }

        if (fields.Length > existingNoteType.FieldNames.Length)
        {
            throw new ArgumentException($"Cannot create a note with more fields ({fields.Length}) than the note type ({existingNoteType.FieldNames.Length})");
        }

        var newNoteId = IdFactory.Create();
        while (_notes.ContainsKey(newNoteId))
        {
            newNoteId++;
        }

        // Create the single note
        var note = new AnkiNote(newNoteId, existingNoteType.Id, fields);
        _notes.Add(newNoteId, note);

        // Create at least one card type
        foreach (var cardType in existingNoteType.CardTypes)
        {
            var newCardId = IdFactory.Create();
            while (_cards.ContainsKey(newCardId))
            {
                newCardId++;
            }

            var newCard = new AnkiCard(newCardId, note, cardType.Ordinal);
            _cards.Add(newCardId, newCard);
            _decks[deckId].AddCard(newCard);
        }
    }

    internal void AddNoteType(AnkiNoteType noteType)
    {
        if (_noteTypes.ContainsKey(noteType.Id))
        {
            throw new ArgumentException($"The collection already has a note type with id {noteType.Id}");
        }

        _noteTypes.Add(noteType.Id, noteType);
    }

    internal void AddDeck(long id, string name)
    {
        if (_decks.ContainsKey(id))
        {
            throw new ArgumentException($"The collection already has a deck with id {id}");
        }

        if (_decks.Values.Any(d => d.Name == name))
        {
            throw new ArgumentException($"The collection already has a deck with the name {name}");
        }

        var deck = new AnkiDeck(id, name);
        _decks.Add(id, deck);
    }

    internal void AddNoteWithCards(long noteId, long deckId, long noteTypeId, string[] fields, (long Ordinal, long Id)[] cardIds)
    {
        var note = new AnkiNote(noteId, noteTypeId, fields);
        _notes.Add(noteId, note);

        foreach (var (ordinal, id) in cardIds)
        {
            var newCard = new AnkiCard(id, note, ordinal);
            _cards.Add(id, newCard);
            _decks[deckId].AddCard(newCard);
        }
    }
}