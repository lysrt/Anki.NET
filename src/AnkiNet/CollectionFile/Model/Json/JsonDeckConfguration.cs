using System.Text.Json.Serialization;

namespace AnkiNet.CollectionFile.Model.Json;

internal class JsonDeckConfguration
{
	/// <summary>
	/// Deck ID (automatically generated long)
	/// </summary>
	[JsonPropertyName("id")]
	public long Id { get; set; }

	/// <summary>
	/// Last modification time TODO Use DateTime?
	/// </summary>
	[JsonPropertyName("mod")]
	public long LastModificationTime { get; set; }

	/// <summary>
	/// The name of the configuration.
	/// </summary>
	[JsonPropertyName("name")]
	public string Name { get; set; }

	/// <summary>
	/// Update Sequence Number.
	/// </summary>
	[JsonPropertyName("usn")]
	public long UpdateSequenceNumber { get; set; }

	/// <summary>
	/// Whether the audio associated to a question should be played when the question is shown.
	/// </summary>
	[JsonPropertyName("autoplay")]
	public bool AutoplayQuestionAudio { get; set; }

	/// <summary>
	/// Whether the audio associated to a question should be played when the answer is shown.
	/// </summary>
	[JsonPropertyName("replayq")]
	public bool ReplayQuestionAudio { get; set; }

	/// <summary>
	/// Whether timer should be shown (1) or not (0).
	/// </summary>
	[JsonPropertyName("timer")]
	public int ShowTimer { get; set; }

	/// <summary>
	/// Whether this deck is dynamic.
	/// </summary>
	[JsonPropertyName("dyn")]
	public bool IsDynamic { get; set; }

	/// <summary>
	/// The number of seconds after which to stop the timer.
	/// </summary>
	[JsonPropertyName("maxTaken")]
	public long StopTimerAfterSeconds { get; set; }

	/// <summary>
	/// The configuration for lapse cards.
	/// </summary>
	[JsonPropertyName("lapse")]
	public JsonLapseCardsConfiguration LapseCardsConfiguration { get; set; }

	/// <summary>
	/// The configuration for new cards.
	/// </summary>
	[JsonPropertyName("new")]
	public JsonNewCardsConfiguration NewCardsConfiguration { get; set; }

	/// <summary>
	/// The configuration for review cards.
	/// </summary>
	[JsonPropertyName("rev")]
	public JsonReviewCardsConfiguration ReviewCardsConfiguration { get; set; }

    /*
	 * Extra options in json file but not in documentation:
	 *
	 * "newMix":0,
	 * "newPerDayMinimum":0,
	 * "interdayLearningMix":0,
	 * "reviewOrder":0,
	 * "newSortOrder":0,
	 * "newGatherPriority":0,
	 * "buryInterdayLearning":false,
	 * 
	 */
}