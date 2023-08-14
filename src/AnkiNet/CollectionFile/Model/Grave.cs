namespace AnkiNet.CollectionFile.Model;

internal record Grave(
    long UpdateSequenceNumber,
    long OriginalId,
    GraveType Type
);