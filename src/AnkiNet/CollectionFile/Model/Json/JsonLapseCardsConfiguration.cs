using System.Text.Json.Serialization;

namespace AnkiNet.CollectionFile.Model.Json;

internal class JsonLapseCardsConfiguration
{
	/// <summary>
    /// TODO Is this description correct?
	/// The list of successive delay between the learning steps of the new cards, as explained in the manual.
	/// </summary>
	[JsonPropertyName("delays")]
	public float[] Delays { get; set; }

	/// <summary>
	/// What to do to leech cards. 0 for suspend, 1 for mark.
    /// Numbers according to the order in which the choices appear in aqt/dconf.ui
	/// </summary>
	[JsonPropertyName("leechAction")]
	public int LeechAction { get; set; }

	/// <summary>
	/// The number of lapses authorized before doing leechAction.
	/// </summary>
	[JsonPropertyName("leechFails")]
	public int LeechFailsAllowedCount { get; set; }

	/// <summary>
	/// Lower limit to the new interval after a leech.
	/// </summary>
	[JsonPropertyName("minInt")]
	public long MinimumInterfalAfterLeech { get; set; }

	/// <summary>
	/// Percent by which to multiply the current interval when a card has lapsed.
	/// </summary>
	[JsonPropertyName("mult")]
	public float LapsedIntervalMultiplierPercent { get; set; }
}