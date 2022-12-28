namespace AnkiNet.database.model;

internal record note(
    long id,
    string guid,
    long mid,
    long mod,
    long usn,
    string tags,
    string flds,
    string sfld,
    long csum,
    long flags,
    string data
);