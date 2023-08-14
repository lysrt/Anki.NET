namespace AnkiNet.CollectionFile.Database.Model;

internal record card(
    long id,
    long nid,
    long did,
    long ord,
    long mod,
    long usn,
    long type,
    long queue,
    long due,
    long ivl,
    long factor,
    long reps,
    long lapses,
    long left,
    long odue,
    long odid,
    long flags,
    string data
);