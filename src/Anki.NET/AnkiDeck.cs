namespace AnkiNet;

public record AnkiDeck
{
    public long Id { get; private set; }
    public string Name { get; }
    public List<AnkiCard> Cards { get; }

    public AnkiDeck(long id, string name)
    {
        Id = id;
        Name = name;
        Cards = new List<AnkiCard>();
    }
}