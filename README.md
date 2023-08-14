# Anki.NET

[![NuGet](https://img.shields.io/nuget/v/Anki.NET.svg)](https://www.nuget.org/packages/Anki.NET)
[![NuGet](https://img.shields.io/nuget/dt/Anki.NET.svg)](https://www.nuget.org/packages/Anki.NET)

Create and export Anki collections, decks, notes and cards from your C# applications.

```csharp
var noteType = new AnkiNoteType(
    name: "Basic",
    cardTypes: new[] {
        new AnkiCardType(
            Name: "Card 1",
            Ordinal: 0,
            QuestionFormat: "{{Front}}",
            AnswerFormat: "{{Front}}<hr id=\"answer\">{{Back}}"
        )
    },
    fieldNames: new[] { "Front", "Back" }
);

var collection = new AnkiCollection();

var noteTypeId = collection.CreateNoteType(noteType);
var deckId = collection.CreateDeck("My Anki Deck");

collection.CreateNote(deckId, noteTypeId, "Hello", "Bonjour");

await AnkiFileWriter.WriteToFileAsync("MyCollection.apkg", collection);
```

## Acknowledgement

Anki.NET is a fork form the the archived [AnkiSharp](https://github.com/AnkiTools/AnkiSharp) project from [Clement-Jean](https://github.com/Clement-Jean). Thanks a lot for the hard work!

## Usage

### AnkiCollection

Start by creating an `AnkiCollection`. To add notes to the collection, you need a notes model (`AnkiNoteType`), you can pass in the constructor, like this.
A note can correspond to one or several cards, if their model has several card tempates ('AnkiCardType').

``` csharp
var cardTypes = new[]
{
    new AnkiCardType(
        "Forwards",
        0,
        "{{Front}}<br/>{{hint:Help}}",
        "{{Front}}<hr id=\"answer\">{{Back}}"
    ),
    new AnkiCardType(
        "Backwards",
        1,
        "{{Back}}<br/>{{hint:Help}}",
        "{{Back}}<hr id=\"answer\">{{Front}}"
    )
};

var noteType = new AnkiNoteType(
    "Basic (With hints)",
    cardTypes,
    new[] { "Front", "Back", "Help" }
);
        
var collection = new AnkiCollection();
var noteTypeId = collection.CreateNoteType(noteType);
```

### AnkiDeck

``` csharp
var collection = new AnkiCollection();

var deckId = collection.CreateDeck("French vocabulary");

bool deckExists = collection.TryGetDeckById(deckId, out var deck);
```

### AnkiNote

With the above `AnkiNoteType` (which has two card types), each added note will generate 2 different cards.

```csharp
collection.CreateNote(deckId, notetypeId, "Hello", "Bonjour", "");
collection.CreateNote(deckId, noteTypeId, "House", "Maison", "Starts with \"M\"");
```

### Set CSS

``` csharp
var noteType = new AnkiNoteType(
    name: "Basic",
    cardTypes: cardTypes,
    fieldNames: names,
    css: @".card{
        color: red;
    }",
);
```

### Read `AnkiCollection` from `.apkg` file

``` csharp
AnkiCollection collection = await AnkiFileReader.ReadFromFileAsync("collection.apkg");
```

### Write `AnkiCollection` to `.apkg` file

```csharp
var collection = new AnkiCollection();
await AnkiFileWriter.WriteToFileAsync("MyCollection.apkg", collection);
```

## Resources

- [Database Structure](https://github.com/ankidroid/Anki-Android/wiki/Database-Structure)
- [Anki Scripting](https://www.juliensobczak.com/write/2016/12/26/anki-scripting.html)