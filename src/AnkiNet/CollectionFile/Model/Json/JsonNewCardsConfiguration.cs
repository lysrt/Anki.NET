using System.Text.Json.Serialization;

namespace AnkiNet.CollectionFile.Model.Json;

internal class JsonNewCardsConfiguration
{
	/// <summary>
	/// Whether to bury cards related to new cards answered.
	/// </summary>
	[JsonPropertyName("bury")]
	public bool Bury { get; set; }

	/// <summary>
	/// The list of successive delay between the learning steps of the new cards, as explained in the manual.
	/// </summary>
	[JsonPropertyName("delays")]
	public float[] Delays { get; set; }

	/// <summary>
	/// The initial ease factor.
	/// </summary>
	[JsonPropertyName("initialFactor")]
	public int InitialEaseFactor { get; set; } // TODO What's the type? Int or float?

	/// <summary>
	/// The list of delays according to the button pressed while leaving the learning mode.
    /// Good, easy and unused.
    /// In the GUI, the first two elements corresponds to Graduating Interval and Easy interval.
	/// </summary>
	[JsonPropertyName("ints")]
	public int[] IntDelays { get; set; }

	/// <summary>
	/// In which order new cards must be shown. NEW_CARDS_RANDOM = 0 and NEW_CARDS_DUE = 1.
	/// </summary>
	[JsonPropertyName("order")]
	public int NewCardsShowOrder { get; set; }

	/// <summary>
	/// Maximal number of new cards shown per day.
	/// </summary>
	[JsonPropertyName("perDay")]
	public int NewCardsPerDay { get; set; }

	/// <summary>
	/// Seems to be unused in the code.
	/// </summary>
	[JsonPropertyName("separate")]
	public int Separate { get; set; } // TODO What's the type?
}