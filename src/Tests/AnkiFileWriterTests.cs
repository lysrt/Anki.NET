using AnkiNet;

namespace Tests;

public class AnkiFileWriterTests
{
    private readonly string _folder = "db_files";
    private readonly string _fileName = "Output";

    [Test]
    public async Task WhenWrite_ThenNoExceptionIsThrown()
    {
        var newNoteType = new AnkiNoteType(555, "Basic (With hints)")
        {
            Css = @".card {
    font-family: arial;
    font-size: 20px;
    text-align: center;
    color: red;
    background-color: blue;
}",
            Fields = new[] { "Front", "Back", "Help" },
            CardTypes = new []
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
            }
        };

        // Create with a custom note type
        var collection = new AnkiCollection(newNoteType);

        //
        // 1. Create everything through the AnkiCollection
        //
        var deck = collection.AddDeck("C# Test");
        collection.AddNote(deck, newNoteType, "Bonjour", "Hello", "B... H...");
        collection.AddNote(deck, newNoteType, "Salut", "Hi", "S... Hi...");

        //
        // 2. Write to file
        //
        await new AnkiFileWriter().WriteCollectionToFile(collection, _folder, _fileName);
    }
}