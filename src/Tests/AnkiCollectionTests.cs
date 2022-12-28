using AnkiNet;
using FluentAssertions;

namespace Tests;

public class AnkiCollectionTests
{
    [Test]
    public void New_AnkiCollection_Has_Default_Deck()
    {
        var collection = new AnkiCollection();

        collection.NoteTypes.Should().BeEmpty();
        collection.Decks.Should().HaveCount(1);

        var defaultDeck = collection.Decks.Single();
        defaultDeck.Name.Should().Be("Default");
        defaultDeck.Id.Should().Be(1);
    }

    [Test]
    public void New_AnkiNoteType_Without_CardType_Added_To_Collection_Throws()
    {
        var noteType = new AnkiNoteType(1, "NT")
        {
            Css = "Css",
            Fields = new[] { "A", "B" }
        };

        var createAnkiCollection = () => _ = new AnkiCollection(noteType);
        createAnkiCollection.Should().ThrowExactly<ArgumentException>();
    }

    [Test]
    public void New_AnkiNoteType_With_CardType_Added_To_Collection_Is_OK()
    {
        var noteType = new AnkiNoteType(1, "NT")
        {
            Css = "Css",
            Fields = new[] { "A", "B" },
            CardTypes = new[]
            {
                new AnkiCardType("Name", 0, "Q", "A")
            }
        };

        var createAnkiCollection = () => _ = new AnkiCollection(noteType);
        createAnkiCollection.Should().NotThrow();
    }

    [Test]
    public void AnkiCollection_Cannot_Add_Deck_With_Default_Name()
    {
        var collection = new AnkiCollection();
        var addDeck = () => _ = collection.AddDeck("Default");
        addDeck.Should().ThrowExactly<ArgumentException>();
    }

    [Test]
    public void AnkiCollection_Cannot_Add_Deck_With_Default_Id_1()
    {
        var collection = new AnkiCollection();
        var addDeck = () => _ = collection.AddDeck(1, "Some deck");
        addDeck.Should().ThrowExactly<ArgumentException>();
    }

    [Test]
    public void AnkiCollection_With_Deck_Cannot_Add_Deck_With_Same_Name()
    {
        var collection = new AnkiCollection();
        collection.AddDeck("New");
        var addDeck = () => _ = collection.AddDeck("New");
        addDeck.Should().ThrowExactly<ArgumentException>();
    }

    [Test]
    public void AnkiCollection_With_Deck_Cannot_Add_Deck_With_Same_Id()
    {
        var collection = new AnkiCollection();
        collection.AddDeck(15, "New deck 1");
        var addDeck = () => _ = collection.AddDeck(15, "New deck 2");
        addDeck.Should().ThrowExactly<ArgumentException>();
    }

    [Test]
    public void AnkiCollection_AddNote_With_Unknown_Deck_Id_Throws()
    {
        var collection = new AnkiCollection();
        var addNote = () => collection.AddNote(
            new AnkiDeck(50, "Deck"),
            new AnkiNoteType(100, "Note type"),
            "A", "B"
        );
        addNote.Should().ThrowExactly<InvalidOperationException>();
    }

    [Test]
    public void AnkiCollection_AddNote_With_Unknown_Deck_Name_Throws()
    {
        var collection = new AnkiCollection();
        var addNote = () => collection.AddNote(
            new AnkiDeck(1, "Unknown deck"),
            new AnkiNoteType(100, "Note type"),
            "A", "B"
        );
        addNote.Should().ThrowExactly<InvalidOperationException>();
    }

    [Test]
    public void AnkiCollection_AddNote_With_Unknown_NoteTypeId_Throws()
    {
        var collection = new AnkiCollection();
        const int unknownNoteTypeId = 15;
        var addNote = () => collection.AddNote(
            new AnkiDeck(1, "Default"),
            new AnkiNoteType(unknownNoteTypeId, "Note Type"),
            "A", "B"
        );
        addNote.Should().ThrowExactly<InvalidOperationException>();
    }

    [Test]
    public void AnkiCollection_AddNote_To_Deck_With_Known_NoteTypeId_Creates_Cards()
    {
        const long noteTypeId = 150;
        const long cardTypeOrdinal1 = 23;
        const long cardTypeOrdinal2 = 55;
        var noteType = new AnkiNoteType(noteTypeId, "NT")
        {
            CardTypes = new[] {
                new AnkiCardType("CT1", cardTypeOrdinal1, "Q1", "A1"),
                new AnkiCardType("CT2", cardTypeOrdinal2, "Q2", "A2")
            }
        };
    
        var collection = new AnkiCollection(noteType);

        var defaultDeck = collection.GetDeckById(1)!;
        defaultDeck.Cards.Should().BeEmpty();

        collection.AddNote(defaultDeck, noteType, "A", "B");

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
}