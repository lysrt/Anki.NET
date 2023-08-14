using FluentAssertions;

namespace AnkiNet.Tests.Unit;

public class AnkiCollectionTests
{
    [Fact]
    public void New_AnkiCollection_Has_Default_Deck()
    {
        var collection = new AnkiCollection();

        collection.NoteTypes.Should().BeEmpty();
        collection.Decks.Should().HaveCount(1);

        var defaultDeck = collection.Decks.Single();
        defaultDeck.Name.Should().Be("Default");
        defaultDeck.Id.Should().Be(1);
    }

    [Fact]
    public void New_AnkiNoteType_Without_CardType_Added_To_Collection_Throws()
    {
        var createNoteType = () => _ = new AnkiNoteType("NT", Array.Empty<AnkiCardType>(), new[] {"A", "B" }, "Css");
        createNoteType.Should().ThrowExactly<ArgumentException>();
    }

    [Fact]
    public void New_AnkiNoteType_With_CardType_Added_To_Collection_Is_OK()
    {
        var noteType = new AnkiNoteType("NT", new[] { new AnkiCardType("Name", 0, "Q", "A") }, new[] { "A", "B" }, "Css");

        var collection = new AnkiCollection();
        var createAnkiCollection = () => collection.CreateNoteType(noteType);
        createAnkiCollection.Should().NotThrow();
    }

    [Fact]
    public void AnkiCollection_Cannot_Add_Deck_With_Default_Name()
    {
        var collection = new AnkiCollection();
        var addDeck = () => _ = collection.CreateDeck("Default");
        addDeck.Should().ThrowExactly<ArgumentException>();
    }

    [Fact]
    public void AnkiCollection_Cannot_Add_Deck_With_Default_Id_1()
    {
        var collection = new AnkiCollection();
        var addDeck = () => collection.AddDeck(1, "Some deck");
        addDeck.Should().ThrowExactly<ArgumentException>();
    }

    [Fact]
    public void AnkiCollection_With_Deck_Cannot_Add_Deck_With_Same_Name()
    {
        var collection = new AnkiCollection();
        _ = collection.CreateDeck("New");
        var addDeck = () => _ = collection.CreateDeck("New");
        addDeck.Should().ThrowExactly<ArgumentException>();
    }

    [Fact]
    public void AnkiCollection_With_Deck_Cannot_Add_Deck_With_Same_Id()
    {
        var collection = new AnkiCollection();
        collection.AddDeck(15, "New deck 1");
        var addDeck = () => collection.AddDeck(15, "New deck 2");
        addDeck.Should().ThrowExactly<ArgumentException>();
    }

    [Fact]
    public void AnkiCollection_AddNote_With_Unknown_Deck_Id_Throws()
    {
        var collection = new AnkiCollection();
        var addNote = () => collection.CreateNote(
            50,
            1,
            "A", "B"
        );
        addNote.Should().ThrowExactly<ArgumentException>();
    }

    [Fact]
    public void AnkiCollection_AddNote_With_Unknown_NoteTypeId_Throws()
    {
        var collection = new AnkiCollection();
        const int unknownNoteTypeId = 15;
        var addNote = () => collection.CreateNote(
            1,
            unknownNoteTypeId,
            "A", "B"
        );
        addNote.Should().ThrowExactly<ArgumentException>();
    }

    [Fact]
    public void AnkiCollection_AddNote_To_Deck_With_Known_NoteTypeId_Creates_Cards()
    {
        const long cardTypeOrdinal1 = 23;
        const long cardTypeOrdinal2 = 55;
        var cardTypes = new[]
        {
            new AnkiCardType("CT1", cardTypeOrdinal1, "Q1", "A1"),
            new AnkiCardType("CT2", cardTypeOrdinal2, "Q2", "A2")
        };
        var noteType = new AnkiNoteType("NT", cardTypes, new[] {"A", "B", "C"}, "");
    
        var collection = new AnkiCollection();
        var noteTypeId = collection.CreateNoteType(noteType);

        var defaultDeck = collection.DefaultDeck;
        defaultDeck.Cards.Should().BeEmpty();

        collection.CreateNote(defaultDeck.Id, noteTypeId, "A", "B");

        defaultDeck.Cards.Should().HaveCount(2);
        var card1 = defaultDeck.Cards[0];
        var card2 = defaultDeck.Cards[1];

        card1.Note.NoteTypeId.Should().Be(noteTypeId);
        card1.Note.Fields.Should().Equal("A", "B");
        card1.NoteCardTypeOrdinal.Should().Be(cardTypeOrdinal1);

        card2.Note.NoteTypeId.Should().Be(noteTypeId);
        card2.Note.Fields.Should().Equal("A", "B");
        card2.NoteCardTypeOrdinal.Should().Be(cardTypeOrdinal2);
    }

    [Fact]
    public void CheckAllFeatures()
    {
        // Card types
        var ct1 = new AnkiCardType("ID to EN", 0, "{{ID}} ", "{{ID}}<hr id=\"answer\">{{EN}}");
        var ct2 = new AnkiCardType("EN to ID", 1, "{{EN}}", "{{EN}}<hr id=\"answer\">{{ID}}");

        // Create a note type with 2 models. This will create 2 cards for each new note
        var noteType = new AnkiNoteType("Back and forth", new[] { ct1, ct2 }, new[] { "ID", "EN" }, "css");

        // Create a collection
        var collection = new AnkiCollection();

        // Add the note type to the collection
        var noteTypeId = collection.CreateNoteType(noteType);

        // Create a deck
        var deckId = collection.CreateDeck("Indonesian vocabulary");

        // Create notes, using the note type idx
        collection.CreateNote(deckId, noteTypeId, "Bunga", "Flower");
        collection.CreateNote(deckId, noteTypeId, "Kucing", "Cat");

        // Check the resulting cards
        var allDecks = collection.Decks;
        _ = collection.TryGetDeckByName("Indonesian vocabulary", out var deck1);
        _ = collection.TryGetDeckById(deckId, out var deck2);

        foreach (var c in deck1!.Cards)
        {
            // Read the fields
            var fields = c.Note.Fields;
        }
    }

    [Fact]
    public void AddSeveralNoteTypes_NoIdClash()
    {
        var cardTypes = new[] { new AnkiCardType("CT", 0, "", "") };
        var fields = new[] { "F1", "F2" };
        var css = "";

        var nt1 = new AnkiNoteType("A", cardTypes, fields, css);
        var nt2 = new AnkiNoteType("B", cardTypes, fields, css);
        var nt3 = new AnkiNoteType("C", cardTypes, fields, css);
        var nt4 = new AnkiNoteType("D", cardTypes, fields, css);

        var col = new AnkiCollection();
        col.CreateNoteType(nt1);
        col.CreateNoteType(nt2);
        col.CreateNoteType(nt3);
        col.CreateNoteType(nt4);
    }
}