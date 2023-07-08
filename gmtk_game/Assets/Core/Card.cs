public class Card
{
    public readonly CardColor color;
    public readonly int number;

    public Card(CardColor color, int number)
    {
        this.color = color;
        this.number = number;
    }
}

public enum CardColor
{
    Heart = 0,
    Diamond = 1,
    Spade = 2,
    Club = 3
}
