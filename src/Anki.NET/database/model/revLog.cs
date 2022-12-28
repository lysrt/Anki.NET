namespace AnkiNet.database.model;

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