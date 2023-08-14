using FluentAssertions;

namespace AnkiNet.Tests.Integration;

public class WriteAndReadTests
{
    [Fact]
    public async Task WhenWriteThenRead_ThenCollectionsAreIdentical()
    {
        var expectedCollection = CreateCollection();

        using var stream = new MemoryStream();
        await AnkiFileWriter.WriteToStreamAsync(stream, expectedCollection);

        var actualCollection = await AnkiFileReader.ReadFromStreamAsync(stream);

        actualCollection.Should().BeEquivalentTo(expectedCollection);
    }

    private static AnkiCollection CreateCollection()
    {
        var cardTypes = new[]
            {
                new AnkiCardType
                (
                    "Forward",
                    0,
                    "{{Front}}<br/>{{hint:Help}}",
                    "{{Front}}<hr id=\"answer\">{{Back}}"
                ),
                new AnkiCardType
                (
                    "Backward",
                    1,
                    "{{Back}}<br/>{{hint:Help}}",
                    "{{Back}}<hr id=\"answer\">{{Front}}"
                )
            };

        var css = @".card {
    font-family: arial;
    font-size: 20px;
    text-align: center;
    color: red;
    background-color: blue;
}";
        var newNoteType = new AnkiNoteType(
            "Basic (With hints)", cardTypes, new[] { "Front", "Back", "Help" }, css
         );

        // Create with a custom note type
        var collection = new AnkiCollection();
        var noteTypeId = collection.CreateNoteType(newNoteType);

        //
        // 1. Create everything through the AnkiCollection
        //
        var deckId = collection.CreateDeck("C# Test");
        collection.CreateNote(deckId, noteTypeId, "Bonjour", "Hello", "B... H...");
        collection.CreateNote(deckId, noteTypeId, "Salut", "Hi", "S... Hi...");

        return collection;
    }
}