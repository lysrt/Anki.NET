namespace AnkiNet;

public record AnkiCard
{
    public long Id { get; set; }
    public long NoteCardTypeOrdinal { get; set; }
    public AnkiNote Note { get; set; }

    //public AnkiCard(AnkiNote note, long noteCardTypeOrdinal)
    //{
    //    Id = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    //    Note = note;
    //    NoteCardTypeOrdinal = noteCardTypeOrdinal;
    //}
}