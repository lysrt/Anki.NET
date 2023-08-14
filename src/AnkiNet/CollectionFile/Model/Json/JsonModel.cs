using System.Text.Json.Serialization;

namespace AnkiNet.CollectionFile.Model.Json;

internal class JsonModel
{
    /// <summary>
    /// Model ID, matches 'mid' column from [notes] table.
    /// </summary>
    [JsonPropertyName("id")]
    public long Id { get; set; }

    /// <summary>
    /// Model name.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// Modification time in seconds. // TODO Use DateTime?
    /// </summary>
    [JsonPropertyName("mod")]
    public long ModificationTime { get; set; }

    /// <summary>
    /// CSS, shared for all templates.
    /// </summary>
    [JsonPropertyName("css")]
    public string Css { get; set; }

    /// <summary>
    /// Long specifying the id of the deck that cards are added to by default.
    /// </summary>
    [JsonPropertyName("did")]
    public long? DefaultDeckId { get; set; }

    /// <summary>
    /// Integer specifying what type of model.
    /// 0 for standard, 1 for cloze.
    /// </summary>
    [JsonPropertyName("type")]
    public int ModelType { get; set; }

    /// <summary>
    /// Update sequence number: used in same way as other usn vales in db.
    /// </summary>
    [JsonPropertyName("usn")]
    public long UpdateSequenceNumber { get; set; }

    /// <summary>
    /// Legacy version number (unused), use an empty array [].
    /// </summary>
    [JsonPropertyName("vers")]
    public int[] LegacyVersionNumber { get; set; }

    /// <summary>
    /// String added to end of LaTeX expressions (usually \\end{document}).
    /// </summary>
    [JsonPropertyName("latexPost")]
    public string LatexPost { get; set; }

    /// <summary>
    /// Preamble string for LaTeX expressions.
    /// </summary>
    [JsonPropertyName("latexPre")]
    public string LatexPre { get; set; }

    /// <summary>
    /// Undocumented.
    /// </summary>
    [JsonPropertyName("latexsvg")]
    public bool LatexSvg { get; set; }

    /// <summary>
    /// Integer specifying which field is used for sorting in the browser.
    /// </summary>
    [JsonPropertyName("sortf")]
    public int BrowserSortField { get; set; }

    /// <summary>
    /// Anki saves the tags of the last added note to the current model, use an empty array [].
    /// </summary>
    [JsonPropertyName("tags")]
    public string[] LastAddedNoteTags { get; set; }

    /// <summary>
    /// JSONArray containing object of CardTemplate for each card in model.
    /// </summary>
    [JsonPropertyName("tmpls")]
    public JsonCardTemplate[] CardTemplates { get; set; }

    /// <summary>
    /// JSONArray containing object for each field in the model.
    /// </summary>
    [JsonPropertyName("flds")]
    public JsonField[] Fields { get; set; }

    /// <summary>
    /// req is unused in modern clients. May exist for backwards compatibility.
    /// https://forums.ankiweb.net/t/is-req-still-used-or-present/9977
    /// AnkiDroid 2.14 uses it, AnkiDroid 2.15 does not use it but still generates it.
    /// Array of arrays describing, for each template T, which fields are required to generate T.
    /// The array is of the form[T, string, list], where:
    /// -  T is the ordinal of the template.
    /// - The string is 'none', 'all' or 'any'. 
    /// - The list contains ordinal of fields, in increasing order.
    /// The meaning is as follows:
    /// - if the string is 'none', then no cards are generated for this template.The list should be empty.
    /// - if the string is 'all' then the card is generated only if each field of the list are filled
    /// - if the string is 'any', then the card is generated if any of the field of the list is filled.
    /// 
    /// The algorithm to decide how to compute req from the template is explained on: 
    /// https://github.com/Arthur-Milchior/anki/blob/commented/documentation//templates_generation_rules.md
    ///
    /// Example: [[0, "any", [0]]]
    /// </summary>
    [JsonPropertyName("req")]
    public object[] RequiredFields { get; set; }
}