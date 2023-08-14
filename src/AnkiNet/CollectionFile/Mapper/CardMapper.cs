using AnkiNet.CollectionFile.Database.Model;
using AnkiNet.CollectionFile.Model;

namespace AnkiNet.CollectionFile.Mapper;

internal static class CardMapper
{
    public static Card FromDb(card card)
    {
        return new Card(
            card.id,
            card.nid,
            card.did,
            card.ord,
            card.mod,
            card.usn,
            (CardLearningType)card.type,
            card.queue,
            card.due,
            card.ivl,
            card.factor,
            card.reps,
            card.lapses,
            card.left,
            card.odue,
            card.odid,
            card.flags,
            card.data
        );
    }

    public static card ToDb(Card card)
    {
        return new card(
            card.Id,
            card.NoteId,
            card.DeckId,
            card.Ordinal,
            card.ModificationTime,
            card.UpdateSequenceNumber,
            (long)card.LearningType,
            card.Queue,
            card.Due,
            card.Interval,
            card.EaseFactor,
            card.ReviewsCount,
            card.LapsesCount,
            card.Left,
            card.OriginalDue,
            card.OriginalDid,
            card.Flags,
            card.Data
        );
    }
}