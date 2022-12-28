using AnkiNet.model.json;

namespace AnkiNet.model;

internal record Collection(
    long Id, // Arbitrary number
    long CreationDateTime, // TODO Convert to DateTime?
    long LastModifiedDateTime, // TODO Convert to DateTime?
    long SchemaModificationDateTime, // TODO Convert to DateTime?
    long Version, // TODO Document? Version 11 is needed
    long Dirty, // Unused, set to 0
    long UpdateSequenceNumber,
    long LastSyncDateTime, // TODO Convert to DateTime?
    JsonConfiguration Configuration,
    Dictionary<long, JsonModel> Models,
    Dictionary<long, JsonDeck> Decks,
    Dictionary<long, JsonDeckConfguration> DecksConfiguration,
    string Tags // TODO What is the doc for this one?
)
{
    public Card[] Cards { get; set; }
    public Grave[] Graves { get; set; }
    public Note[] Notes { get; set; }
    public RevisionLog[] RevLogs { get; set; }
}