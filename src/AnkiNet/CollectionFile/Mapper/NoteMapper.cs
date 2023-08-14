using AnkiNet.CollectionFile.Database.Model;
using AnkiNet.CollectionFile.Model;

namespace AnkiNet.CollectionFile.Mapper;

internal static class NoteMapper
{
    private const char FieldSeparator = '\u001f';

    public static Note FromDb(note note)
    {
        return new Note(
            note.id,
            note.guid,
            note.mid,
            note.mod,
            note.usn,
            note.tags,
            note.flds.Split(FieldSeparator),
            note.sfld,
            note.csum,
            note.flags,
            note.data
        );
    }

    public static note ToDb(Note note)
    {
        return new note(
            note.Id,
            note.Guid,
            note.ModelId,
            note.ModificationDateTime,
            note.UpdateSequenceNumber,
            note.Tags,
            string.Join(FieldSeparator, note.Fields),
            note.SortField,
            note.FieldChecksum,
            note.Flags,
            note.Data
        );
    }
}