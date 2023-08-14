using System.Text.Json.Serialization;

namespace AnkiNet.CollectionFile.Model.Json;

internal class JsonDeck
{
    /// <summary>
    /// Deck ID (automatically generated long)
    /// </summary>
    [JsonPropertyName("id")]
    public long Id { get; set; }

    /// <summary>
    /// Last modification time // TODO To DateTime?
    /// </summary>
    [JsonPropertyName("mod")]
    public long LastModificationTime { get; set; }

    /// <summary>
    /// Name of the deck.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// Update sequence number.
    /// </summary>
    [JsonPropertyName("usn")]
    public long UpdateSequenceNumber { get; set; }

    /// <summary>
    /// Two number array.
    /// First one is the number of days that have passed between the collection was created and the deck was last updated.
    /// The second one is equal to the number of cards seen today in this deck minus the number of new cards in custom study today.
    /// </summary>
    [JsonPropertyName("newToday")]
    public int[] NewToday { get; set; }

    /// <summary>
    /// Two number array.
    /// First one is the number of days that have passed between the collection was created and the deck was last updated.
    /// The second one is equal to the number of cards seen today in this deck minus the number of new cards in custom study today.
    /// </summary>
    [JsonPropertyName("revToday")]
    public int[] ReviewedToday { get; set; }

    /// <summary>
    /// Two number array.
    /// First one is the number of days that have passed between the collection was created and the deck was last updated.
    /// The second one is equal to the number of cards seen today in this deck minus the number of new cards in custom study today.
    /// </summary>
    [JsonPropertyName("lrnToday")]
    public int[] LearnedToday { get; set; }

    /// <summary>
    /// Two number array used somehow for custom study. Currently unused in the code.
    /// </summary>
    [JsonPropertyName("timeToday")]
    public int[] TimeToday { get; set; }

    /// <summary>
    /// True when deck is collapsed.
    /// </summary>
    [JsonPropertyName("collapsed")]
    public bool IsCollapsed { get; set; }

    /// <summary>
    /// True when deck collapsed in browser.
    /// </summary>
    [JsonPropertyName("browserCollapsed")]
    public bool IsCollapsedInBrowser { get; set; }

    /// <summary>
    /// Deck's description.
    /// </summary>
    [JsonPropertyName("desc")]
    public string Description { get; set; }

    /// <summary>
    /// 1 if dynamic (aka. filtered) deck.
    /// </summary>
    [JsonPropertyName("dyn")]
    public int IsDynamic { get; set; }

    /// <summary>
    /// Id of option group from 'dconf' column in [col]table.
    /// Or absent if the deck is dynamic (aka. filtered).
    /// </summary>
    [JsonPropertyName("conf")]
    public long ConfigurationGroupId { get; set; }

    /// <summary>
    /// Extended new card limit (for custom study).
    /// Potentially absent, in this case it's considered to be 10, by aqt.customstudy.
    /// </summary>
    [JsonPropertyName("extendNew")]
    public int ExtendedNewCardLimit { get; set; }

    /// <summary>
    /// Extended review card limit (for custom study).
    /// Potentially absent, in this case it's considered to be 10, by aqt.customstudy.
    /// </summary>
    [JsonPropertyName("extendRev")]
    public int ExtendedReviewCardLimit { get; set; }
}