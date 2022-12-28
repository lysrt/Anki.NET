namespace AnkiNet.database.model;

internal record DatabaseExtract(
    col col,
    List<card> cards,
    List<grave> graves,
    List<note> notes,
    List<revLog> revLogs
);
