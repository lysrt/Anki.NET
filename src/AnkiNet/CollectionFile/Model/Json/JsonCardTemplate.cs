using System.Text.Json.Serialization;

namespace AnkiNet.CollectionFile.Model.Json;

internal class JsonCardTemplate
{
    /// <summary>
    /// Template name.
    /// </summary>
    [JsonPropertyName("name")]
    public string TemplateName { get; set; }

    /// <summary>
    /// Template number, see JsonField Fields attribute in JsonModel (flds).
    /// </summary>
    [JsonPropertyName("ord")]
    public long TemplateOrdinal { get; set; }

    /// <summary>
    /// Deck override (null by default)
    /// </summary>
    [JsonPropertyName("did")]
    public long? DeckOverrideId { get; set; }

    /// <summary>
    /// Answer template string.
    /// </summary>
    [JsonPropertyName("afmt")]
    public string AnswerFormat { get; set; }

    /// <summary>
    /// Browser answer format: used for displaying answer in browser.
    /// </summary>
    [JsonPropertyName("bafmt")]
    public string BrowserAnswerFormat { get; set; }

    /// <summary>
    /// Question format string.
    /// </summary>
    [JsonPropertyName("qfmt")]
    public string QuestionFormat { get; set; }
    //qfmt : "question format string"

    /// <summary>
    /// Browser question format: used for displaying question in browser.
    /// </summary>
    [JsonPropertyName("bqfmt")]
    public string BrowserQuestionFormat { get; set; }

    /// <summary>
    /// Undocumented.
    /// </summary>
    [JsonPropertyName("bfont")]
    public string BFont { get; set; }

    /// <summary>
    /// Undocumented.
    /// </summary>
    [JsonPropertyName("bsize")]
    public int BSize { get; set; }
}