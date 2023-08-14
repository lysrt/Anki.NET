namespace AnkiNet.CollectionFile.Model;

internal record RevisionLog(
    long Id, // Timestamp
    long CardId,
    long UpdateSequenceNumber,
    long Ease,
    long Interval, // See cards table
    long LastInterval,
    long Factor,
    long TimeTookMs,
    RevisionType RevisionType
)
{
    public RevisionEaseType GetEaseType()
    {
        if (RevisionType == RevisionType.Review)
        {
            switch(Ease)
            {
                case 1: return RevisionEaseType.Wrong;
                case 2: return RevisionEaseType.Hard;
                case 3: return RevisionEaseType.Ok;
                case 4: return RevisionEaseType.Easy;
            }
        }
        else if (RevisionType == RevisionType.Learn || RevisionType == RevisionType.Relearn)
        {
            switch (Ease)
            {
                case 1: return RevisionEaseType.Wrong;
                case 2: return RevisionEaseType.Ok;
                case 3: return RevisionEaseType.Easy;
            }
        }

        throw new InvalidOperationException();
        // TODO Test
    }
}