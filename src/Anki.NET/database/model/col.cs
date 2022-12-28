namespace AnkiNet.database.model;

internal record col (
    long id,
    long crt,
    long mod,
    long scm,
    long ver,
    long dty,
    long usn,
    long ls,
    string conf,
    string models,
    string decks,
    string dconf,
    string tags
);