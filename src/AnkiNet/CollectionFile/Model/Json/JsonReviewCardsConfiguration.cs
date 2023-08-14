using System.Text.Json.Serialization;

namespace AnkiNet.CollectionFile.Model.Json;

internal class JsonReviewCardsConfiguration
{
	/// <summary>
	/// Whether to bury cards related to new cards answered.
	/// </summary>
	[JsonPropertyName("bury")]
	public bool Bury { get; set; }

	/// <summary>
	/// The number to add to the easyness when the easy button is pressed.
	/// </summary>
	[JsonPropertyName("ease4")]
	public float Ease4 { get; set; }

	/// <summary>
	/// The new interval is multiplied by a random number between -fuzz and fuzz
	/// </summary>
	[JsonPropertyName("fuzz")]
	public int Fuzz { get; set; } // TODO What type is it???? Float or Int?

	/// <summary>
	/// Multiplication factor applied to the intervals Anki generates.
	/// </summary>
	[JsonPropertyName("ivlFct")]
	public float IntervalMultiplicationFactor { get; set; }

	/// <summary>
	/// Maximal interval for reviews. // TODO What's the unit?
	/// </summary>
	[JsonPropertyName("maxIvl")]
	public int MaximumReviewInterval { get; set; }

	/// <summary>
	/// Not currently used. // TODO What is the type?
	/// </summary>
	[JsonPropertyName("minSpace")]
	public int MinSpace { get; set; }

	/// <summary>
	/// Numbers of cards to review per day
	/// </summary>
	[JsonPropertyName("perDay")]
	public int CardsToReviewPerDay { get; set; }

	/// <summary>
	/// ?? TODO Not in the doc
	/// </summary>
	[JsonPropertyName("hardFactor")]
	public float HardFactor { get; set; }
}