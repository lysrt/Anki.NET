using System.Text.Json.Serialization;

namespace AnkiNet.CollectionFile.Model.Json;

internal class JsonField
{
    /// <summary>
    /// Field name.
    /// </summary>
    [JsonPropertyName("name")]
    public string FieldName { get; set; }

    /// <summary>
    /// Ordinal of the field - goes from 0 to num fields -1.
    /// </summary>
    [JsonPropertyName("ord")]
    public int FieldNumber { get; set; }

    /// <summary>
    /// Whether this field uses right-to-left script.
    /// </summary>
    [JsonPropertyName("rtl")]
    public bool IsRightToLeft { get; set; }

    /// <summary>
    /// Sticky fields retain the value that was last added when adding new notes.
    /// </summary>
    [JsonPropertyName("sticky")]
    public bool IsSticky { get; set; }

    /// <summary>
    /// DisplayFont
    /// </summary>
    [JsonPropertyName("font")]
    public string Font { get; set; }

    /// <summary>
    /// Font size
    /// </summary>
    [JsonPropertyName("size")]
    public int FontSize { get; set; }

    /// <summary>
    /// Undocumented
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; set; }

    /// <summary>
    /// Array of media. appears to be unused.
    /// </summary>
    [JsonPropertyName("media")]
    public string[]? Media { get; set; }
}
