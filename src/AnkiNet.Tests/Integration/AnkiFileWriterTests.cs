namespace AnkiNet.Tests.Integration;

public class AnkiFileWriterTests
{
    private readonly string _folder = "db_files";
    private readonly string _fileName = "Output.apkg";

    [Fact]
    public async Task WhenWrite_ThenNoExceptionIsThrown()
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
        collection.CreateNote(deckId, noteTypeId, "Ne le faites pas", @"don't");
        collection.CreateNote(deckId, noteTypeId, "Test", @"""test""");

        //
        // 2. Write to file
        //
        await AnkiFileWriter.WriteToFileAsync(_folder, _fileName, collection);
    }
}