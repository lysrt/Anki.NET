namespace AnkiNet.model;

internal record Grave(
    long UpdateSequenceNumber,
    long OriginalId,
    GraveType Type
);