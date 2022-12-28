# Anki.NET

[![NuGet](https://img.shields.io/nuget/v/Anki.NET.svg)](https://www.nuget.org/packages/Anki.NET)
[![NuGet](https://img.shields.io/nuget/dt/Anki.NET.svg)](https://www.nuget.org/packages/Anki.NET)

Create and export Anki collections, decks, notes and cards from your C# applications.

```csharp
var noteType = new AnkiNoteType(1, "Basic")
{
    Fields = new[] { "Front", "Back" },
    CardTypes = new[]
    {
        new AnkiCardType
        (
            Name: "Card 1",
            Ordinal: 0,
            QuestionFormat: "{{Front}}",
            AnswerFormat: "{{Front}}<hr id=\"answer\">{{Back}}"
        ),
                
    }
};
AnkiCollection collection = new AnkiCollection(noteType);
AnkiDeck deck = collection.AddDeck("My Anki Deck");

collection.AddNote(deck, noteType, "Hello", "Bonjour");

await new AnkiFileWriter().WriteCollectionToFile(collection, "/", "MyCollection.apkg");
```

## Acknowledgement

Anki.NET is a fork form the the archived [AnkiSharp](https://github.com/AnkiTools/AnkiSharp) project from [Clement-Jean](https://github.com/Clement-Jean). Thanks a lot for the hard work!

## Usage

### AnkiCollection

Start by creating an `AnkiCollection`. To add notes to the collection, you need a notes model (`AnkiNoteType`), you can pass in the constructor, like this.
A note can correspond to one or several cards, if their model has several card tempates ('AnkiCardType').

``` csharp
var noteType = new AnkiNoteType(1, "Basic (With hints)")
{
    Fields = new[] { "Front", "Back", "Help" },
    CardTypes = new []
    {
        new AnkiCardType
        (
            "Forwards",
            0,
            "{{Front}}<br/>{{hint:Help}}",
            "{{Front}}<hr id=\"answer\">{{Back}}"
        ),
        new AnkiCardType
        (
            "Backwards",
            1,
            "{{Back}}<br/>{{hint:Help}}",
            "{{Back}}<hr id=\"answer\">{{Front}}"
        )
    }
};

var collection = new AnkiCollection(noteType);
```

### AnkiDeck

``` csharp
var collection = new AnkiCollection(noteType);

var myDeck = collection.AddDeck("French vocabulary");

var defaultDeck = collection.GetDeckById(1);
var myDeckAgain = collection.GetDeckById(myDeck.Id);
```

### AnkiNote

With the above `AnkiNoteType`, each added note will generate 2 different cards.

```csharp
collection.AddNote(defaultDeck, noteType, "Hello", "Bonjour", "");
collection.AddNote(defaultDeck, noteType, "House", "Maison", "Starts with "M");
```

### Set CSS

``` csharp
var noteType = new AnkiNoteType(1, "Basic (with CSS)")
{
    Css = @".card{
        color: red;
    }",
    // ... 
};
```

### Read `AnkiCollection` from `.apkg` file

``` csharp
var collection = await AnkiFileReader.ReadCollection(_path);
```

## Resources

- [Anki APKG format documentation](http://decks.wikia.com/wiki/Anki_APKG_format_documentation)
- [Database Structure](https://github.com/ankidroid/Anki-Android/wiki/Database-Structure)
