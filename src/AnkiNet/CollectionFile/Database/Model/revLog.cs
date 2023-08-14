namespace AnkiNet.CollectionFile.Database.Model;

internal record revLog(
    long id,
    long cid,
    long usn,
    long ease,
    long ivl,
    long lastIvl,
    long factor,
    long time,
    long type
);