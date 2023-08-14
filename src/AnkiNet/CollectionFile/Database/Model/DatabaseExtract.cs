namespace AnkiNet.CollectionFile.Database.Model;

internal record DatabaseExtract(
    col col,
    List<card> cards,
    List<grave> graves,
    List<note> notes,
    List<revLog> revLogs
);
