namespace AnkiNet.CollectionFile.Model;

internal record Card(
    long Id, // Timestamp
    long NoteId,
    long DeckId, // From col table
    long Ordinal,
    long ModificationTime, // TODO Convert to DateTime?
    long UpdateSequenceNumber,
    CardLearningType LearningType,
    long Queue,
    long Due,
    long Interval, // Negative = seconds, Positive = days // TODO Convert to TimeSpan?
    long EaseFactor,
    long ReviewsCount,
    long LapsesCount,
    long Left, // '2004' means 2 reps left today and 4 reps till graduation // TODO Split?
    long OriginalDue,
    long OriginalDid,
    long Flags, // Used for colors?
    string Data // Currently unused?
);