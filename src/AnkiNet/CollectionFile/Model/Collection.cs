using AnkiNet.CollectionFile.Model.Json;

namespace AnkiNet.CollectionFile.Model;

internal record Collection(
    long Id, // Arbitrary number since there is only one row
    long CreationDateTime, // TODO Convert to DateTime?
    long LastModifiedDateTime, // TODO Convert to DateTime?
    long SchemaModificationDateTime, // TODO Convert to DateTime?
    long Version, // Schema version number of the database.
    long Dirty, // Unused, set to 0
    long UpdateSequenceNumber,
    long LastSyncDateTime, // Last sync timestamp in ms // TODO Convert to DateTime?
    JsonConfiguration? Configuration, // json object containing configuration options that are synced
    Dictionary<long, JsonModel>? Models, // json array of json objects containing the models (aka Note types)
    Dictionary<long, JsonDeck>? Decks, // json array of json objects containing the deck
    Dictionary<long, JsonDeckConfguration>? DecksConfiguration, // json array of json objects containing the deck options
    string Tags // a cache of tags used in the collection (probably for autocomplete etc)
)
{
    public Card[]? Cards { get; set; }
    public Grave[]? Graves { get; set; }
    public Note[]? Notes { get; set; }
    public RevisionLog[]? RevLogs { get; set; }
}