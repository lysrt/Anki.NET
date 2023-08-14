using FluentAssertions;

namespace AnkiNet.Tests.Integration;

public class AnkiFileReaderTests
{
    private readonly string _path = "db_files/Japanese.apkg";

    public AnkiFileReaderTests()
    {
    }

    [Fact]
    public async Task WhenRead_ThenNoExceptionIsThrown()
    {
        var collection = await AnkiFileReader.ReadFromFileAsync(_path);

        var expectedDecks = new[]
        {
            new AnkiDeck(1, "Default"),
            new AnkiDeck(1661780077840, "Japanese::YouTube::1. Premiers mots en kanji"),
            new AnkiDeck(1663496509878, "Japanese"),
            new AnkiDeck(1663496546962, "Japanese::YouTube"),
            new AnkiDeck(1661656229326, "Japanese::YouTube::3. Le matériel domestique"),
            new AnkiDeck(1663496558232, "Japanese::Class")
        };

        collection.Decks.Should().BeEquivalentTo(
            expectedDecks,
            o => o.Excluding(d => d.Cards)
        );

        var css = @".card {
    font-family: arial;
    font-size: 20px;
    text-align: center;
    color: black;
    background-color: white;
}
";
        var css2 = @".card {
font-family: arial;
font-size: 20px;
text-align: center;
color: black;
background-color: white;
}";

        var css3 = @".card {
font-family: arial;
font-size: 20px;
text-align: center;
color: black;
background-color: blue;
}";

        var expectedNoteTypes = new[]
        {
            new AnkiNoteType(1663496639418L, "Basic", new[]
                {
                    new AnkiCardType("Card 1", 0, "{{Front}}", @"{{FrontSide}}

<hr id=answer>

{{Back}}")
                }, new[] {"Front", "Back"}, css),
            new AnkiNoteType(1661780059778L, "Basic-6d0e1", new[]
                {
                    new AnkiCardType("Forward", 0, @"{{Front}}
", @"{{FrontSide}}
<hr id=answer />
{{Back}}")
                }, new[] {"Front", "Back"}, css2),
            new AnkiNoteType(1661656212286L, "Basic-64627", new[]
                {
                    new AnkiCardType("Forward", 0, @"{{Front}}
<div style='font-family: ""Ayuthaya""; font-size: 15px;'>{{Help}}</div>
", @"{{FrontSide}}
<hr id=answer />
{{Back}}")
                }, new[] {"Front", "Back", "Help"}, css3)
        };

        collection.NoteTypes.Should().BeEquivalentTo(
            expectedNoteTypes
        );

        var noteTypeId = 1661780059778L;
        var expectedJapaneseDeckCards = new[]
        {
            new AnkiCard(1661780059803L, new AnkiNote(1661780059797L, noteTypeId, "人【ひと】", "person; someone; somebody"), 0),
            new AnkiCard(1661780059806L, new AnkiNote(1661780059804L, noteTypeId, "男【おとこ】", "man; male"), 0),
            new AnkiCard(1661780059808L, new AnkiNote(1661780059807L, noteTypeId, "女【おんな】", "female; woman; female sex"), 0),
            new AnkiCard(1661780059810L, new AnkiNote(1661780059809L, noteTypeId, "子【こ】", "child; kid; teenager; youngster; young (non-adult) person"), 0),
            new AnkiCard(1661780059813L, new AnkiNote(1661780059811L, noteTypeId, "日【ひ】", "day; days"), 0),
            new AnkiCard(1661780059815L, new AnkiNote(1661780059814L, noteTypeId, "月【つき】", "Moon"), 0),
            new AnkiCard(1661780059817L, new AnkiNote(1661780059816L, noteTypeId, "時【とき】", "time; hour; moment"), 0),
            new AnkiCard(1661780059820L, new AnkiNote(1661780059819L, noteTypeId, "水【みず】", "water (esp. cool, fresh water, e.g. drinking water)"), 0),
            new AnkiCard(1661780059822L, new AnkiNote(1661780059821L, noteTypeId, "火【ひ】", "fire; flame; blaze"), 0),
            new AnkiCard(1661780059824L, new AnkiNote(1661780059823L, noteTypeId, "土【つち】", "earth; soil; dirt; clay; mud"), 0),
            new AnkiCard(1661780059825L, new AnkiNote(1661780059825L, noteTypeId, "風【かぜ】", "wind; breeze; draught; draft"), 0),
            new AnkiCard(1661780059827L, new AnkiNote(1661780059826L, noteTypeId, "空【そら】", "sky; the air; the heavens"), 0),
            new AnkiCard(1661780059829L, new AnkiNote(1661780059828L, noteTypeId, "山【やま】", "mountain; hill"), 0),
            new AnkiCard(1661780059831L, new AnkiNote(1661780059830L, noteTypeId, "川【かわ】", "river; stream"), 0),
            new AnkiCard(1661780059834L, new AnkiNote(1661780059832L, noteTypeId, "木【き】", "tree; shrub; bush"), 0),
            new AnkiCard(1661780059836L, new AnkiNote(1661780059835L, noteTypeId, "花【はな】", "flower; blossom; bloom; petal"), 0),
            new AnkiCard(1661780059838L, new AnkiNote(1661780059837L, noteTypeId, "雨【あめ】", "rain"), 0),
            new AnkiCard(1661780059840L, new AnkiNote(1661780059839L, noteTypeId, "雪【ゆき】", "snow; snowfall"), 0),
            new AnkiCard(1661780059841L, new AnkiNote(1661780059840L, noteTypeId, "金【かね】", "money"), 0),
            new AnkiCard(1661780059843L, new AnkiNote(1661780059843L, noteTypeId, "刀【かたな】", "sword (esp. Japanese single-edged); katana"), 0),
        };

        collection.Decks[1].Cards.Should().BeEquivalentTo(
            expectedJapaneseDeckCards
        );
    }

    [Fact]
    public async Task ReadCollection21_NoError()
    {
        var collection = await AnkiFileReader.ReadFromFileAsync("db_files/collection21.apkg");

        collection.Decks.Should().HaveCount(3);
        collection.Decks.First().Id.Should().Be(1);
        collection.Decks.First().Cards.Should().BeEmpty();

        collection.Decks[1].Id.Should().Be(1691848838057L);
        collection.Decks[1].Cards.Should().HaveCount(16);
        var card = collection.Decks[1].Cards.First();

        card.Note.Fields.Should().Equal("Bunga", "Flower");
    }

    [Fact]
    public async Task ReadCollection21b_NotImplementedException()
    {
        var action = async () => _ = await AnkiFileReader.ReadFromFileAsync("db_files/collection21b.apkg");
        await action.Should().ThrowExactlyAsync<NotImplementedException>();

        /*
         * If no exception is thrown, a deck with single card like below will be read in collection21 database file.
         * 
        var collection = await AnkiFileReader.ReadFromFileAsync("db_files/collection21b.apkg");

        collection.Id.Should().Be(1);

        collection.Decks.Should().HaveCount(1);
        collection.Decks.First().Id.Should().Be(1);
        collection.Decks.First().Cards.Should().HaveCount(1);

        var card = collection.Decks.Single().Cards.Single();
        card.Note.Fields.Should().Equal("Please update to the latest Anki version, then import the .colpkg/.apkg file again.", "");
        */
    }
}