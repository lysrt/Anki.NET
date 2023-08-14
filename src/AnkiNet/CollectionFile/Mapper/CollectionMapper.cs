using System.Text.Json;
using AnkiNet.CollectionFile.Database.Model;
using AnkiNet.CollectionFile.Model;
using AnkiNet.CollectionFile.Model.Json;

namespace AnkiNet.CollectionFile.Mapper;

internal static class CollectionMapper
{
    public static Collection FromDb(col col)
    {
        var configuration = JsonSerializer.Deserialize<JsonConfiguration>(col.conf);
        var models = JsonSerializer.Deserialize<Dictionary<long, JsonModel>>(col.models);
        var decks = JsonSerializer.Deserialize<Dictionary<long, JsonDeck>>(col.decks);
        var decksConfiguration = JsonSerializer.Deserialize<Dictionary<long, JsonDeckConfguration>>(col.dconf);

        return new Collection(
            col.id,
            col.crt,
            col.mod,
            col.scm,
            col.ver,
            col.dty,
            col.usn,
            col.ls,
            configuration,
            models,
            decks,
            decksConfiguration,
            col.tags
        );
    }

    public static col ToDb(Collection collection)
    {
        var conf = JsonSerializer.Serialize(collection.Configuration);
        var models = JsonSerializer.Serialize(collection.Models);
        var decks = JsonSerializer.Serialize(collection.Decks);
        var dconf = JsonSerializer.Serialize(collection.DecksConfiguration);

        return new col(
            collection.Id,
            collection.CreationDateTime,
            collection.LastModifiedDateTime,
            collection.SchemaModificationDateTime,
            collection.Version,
            collection.Dirty,
            collection.UpdateSequenceNumber,
            collection.LastSyncDateTime,
            conf,
            models,
            decks,
            dconf,
            collection.Tags
        );
    }
}