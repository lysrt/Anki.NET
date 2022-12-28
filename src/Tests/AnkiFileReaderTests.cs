using AnkiNet;
using FluentAssertions;

namespace Tests;

public class AnkiFileReaderTests
{
    [SetUp]
    public void Setup()
    {
    }

    private readonly string _path = "db_files/Japanese.apkg";

    [Test]
    public async Task WhenRead_ThenNoExceptionIsThrown()
    {
        var collection = await AnkiFileReader.ReadCollection(_path);

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

        var expectedNoteTypes = new[]
        {
            new AnkiNoteType(1663496639418L, "Basic")
            {
                Css = @".card {
    font-family: arial;
    font-size: 20px;
    text-align: center;
    color: black;
    background-color: white;
}
",
                Fields = new[] {"Front", "Back"},
                CardTypes = new[]
                {
                    new AnkiCardType("Card 1", 0, "{{Front}}", @"{{FrontSide}}

<hr id=answer>

{{Back}}")
                }
            },
            new AnkiNoteType(1661780059778L, "Basic-6d0e1")
            {
                Css = @".card {
font-family: arial;
font-size: 20px;
text-align: center;
color: black;
background-color: white;
}",
                Fields = new[] {"Front", "Back"},
                CardTypes = new[]
                {
                    new AnkiCardType("Forward", 0, @"{{Front}}
", @"{{FrontSide}}
<hr id=answer />
{{Back}}")
                }
            },
            new AnkiNoteType(1661656212286L, "Basic-64627")
            {
                Css = @".card {
font-family: arial;
font-size: 20px;
text-align: center;
color: black;
background-color: blue;
}",
                Fields = new[] {"Front", "Back", "Help"},
                CardTypes = new[]
                {
                    new AnkiCardType("Forward", 0, @"{{Front}}
<div style='font-family: ""Ayuthaya""; font-size: 15px;'>{{Help}}</div>
", @"{{FrontSide}}
<hr id=answer />
{{Back}}")
                }
            }
        };

        collection.NoteTypes.Should().BeEquivalentTo(
            expectedNoteTypes
        );

        var expectedJapaneseDeckCards = new[]
        {
            new AnkiCard { Id = 1661780059803L, NoteCardTypeOrdinal = 0, Note = new AnkiNote { Id = 1661780059797L, NoteTypeId = 1661780059778L, Fields = new[] { "人【ひと】", "person; someone; somebody" }}},
            new AnkiCard { Id = 1661780059806L, NoteCardTypeOrdinal = 0, Note = new AnkiNote { Id = 1661780059804L, NoteTypeId = 1661780059778L, Fields = new[] { "男【おとこ】", "man; male" }}},
            new AnkiCard { Id = 1661780059808L, NoteCardTypeOrdinal = 0, Note = new AnkiNote { Id = 1661780059807L, NoteTypeId = 1661780059778L, Fields = new[] { "女【おんな】", "female; woman; female sex" }}},
            new AnkiCard { Id = 1661780059810L, NoteCardTypeOrdinal = 0, Note = new AnkiNote { Id = 1661780059809L, NoteTypeId = 1661780059778L, Fields = new[] { "子【こ】", "child; kid; teenager; youngster; young (non-adult) person" }}},
            new AnkiCard { Id = 1661780059813L, NoteCardTypeOrdinal = 0, Note = new AnkiNote { Id = 1661780059811L, NoteTypeId = 1661780059778L, Fields = new[] { "日【ひ】", "day; days" }}},
            new AnkiCard { Id = 1661780059815L, NoteCardTypeOrdinal = 0, Note = new AnkiNote { Id = 1661780059814L, NoteTypeId = 1661780059778L, Fields = new[] { "月【つき】", "Moon" }}},
            new AnkiCard { Id = 1661780059817L, NoteCardTypeOrdinal = 0, Note = new AnkiNote { Id = 1661780059816L, NoteTypeId = 1661780059778L, Fields = new[] { "時【とき】", "time; hour; moment" }}},
            new AnkiCard { Id = 1661780059820L, NoteCardTypeOrdinal = 0, Note = new AnkiNote { Id = 1661780059819L, NoteTypeId = 1661780059778L, Fields = new[] { "水【みず】", "water (esp. cool, fresh water, e.g. drinking water)" }}},
            new AnkiCard { Id = 1661780059822L, NoteCardTypeOrdinal = 0, Note = new AnkiNote { Id = 1661780059821L, NoteTypeId = 1661780059778L, Fields = new[] { "火【ひ】", "fire; flame; blaze" }}},
            new AnkiCard { Id = 1661780059824L, NoteCardTypeOrdinal = 0, Note = new AnkiNote { Id = 1661780059823L, NoteTypeId = 1661780059778L, Fields = new[] { "土【つち】", "earth; soil; dirt; clay; mud" }}},
            new AnkiCard { Id = 1661780059825L, NoteCardTypeOrdinal = 0, Note = new AnkiNote { Id = 1661780059825L, NoteTypeId = 1661780059778L, Fields = new[] { "風【かぜ】", "wind; breeze; draught; draft" }}},
            new AnkiCard { Id = 1661780059827L, NoteCardTypeOrdinal = 0, Note = new AnkiNote { Id = 1661780059826L, NoteTypeId = 1661780059778L, Fields = new[] { "空【そら】", "sky; the air; the heavens" }}},
            new AnkiCard { Id = 1661780059829L, NoteCardTypeOrdinal = 0, Note = new AnkiNote { Id = 1661780059828L, NoteTypeId = 1661780059778L, Fields = new[] { "山【やま】", "mountain; hill" }}},
            new AnkiCard { Id = 1661780059831L, NoteCardTypeOrdinal = 0, Note = new AnkiNote { Id = 1661780059830L, NoteTypeId = 1661780059778L, Fields = new[] { "川【かわ】", "river; stream" }}},
            new AnkiCard { Id = 1661780059834L, NoteCardTypeOrdinal = 0, Note = new AnkiNote { Id = 1661780059832L, NoteTypeId = 1661780059778L, Fields = new[] { "木【き】", "tree; shrub; bush" }}},
            new AnkiCard { Id = 1661780059836L, NoteCardTypeOrdinal = 0, Note = new AnkiNote { Id = 1661780059835L, NoteTypeId = 1661780059778L, Fields = new[] { "花【はな】", "flower; blossom; bloom; petal" }}},
            new AnkiCard { Id = 1661780059838L, NoteCardTypeOrdinal = 0, Note = new AnkiNote { Id = 1661780059837L, NoteTypeId = 1661780059778L, Fields = new[] { "雨【あめ】", "rain" }}},
            new AnkiCard { Id = 1661780059840L, NoteCardTypeOrdinal = 0, Note = new AnkiNote { Id = 1661780059839L, NoteTypeId = 1661780059778L, Fields = new[] { "雪【ゆき】", "snow; snowfall" }}},
            new AnkiCard { Id = 1661780059841L, NoteCardTypeOrdinal = 0, Note = new AnkiNote { Id = 1661780059840L, NoteTypeId = 1661780059778L, Fields = new[] { "金【かね】", "money" }}},
            new AnkiCard { Id = 1661780059843L, NoteCardTypeOrdinal = 0, Note = new AnkiNote { Id = 1661780059843L, NoteTypeId = 1661780059778L, Fields = new[] { "刀【かたな】", "sword (esp. Japanese single-edged); katana" }}}
        };

        collection.Decks[1].Cards.Should().BeEquivalentTo(
            expectedJapaneseDeckCards
        );
    }
}