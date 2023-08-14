namespace AnkiNet.CollectionFile.Model;

internal record Note(
    long Id, // Timestamp
    string Guid,
    long ModelId,
    long ModificationDateTime, // TODO Convert to DateTime?
    long UpdateSequenceNumber,
    string Tags, // Space separated with spaces at beginning and end
    string[] Fields,
    string SortField,
    long FieldChecksum, // integer representation of first 8 digits of sha1 hash of the first field
    long Flags, // Unused
    string Data // Unused
);