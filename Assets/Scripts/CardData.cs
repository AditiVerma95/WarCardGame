// CardData.cs
// This script does NOT attach to a GameObject. It's just for holding data.

[System.Serializable]
public class CardData
{
    public enum Suit { Clubs, Diamonds, Hearts, Spades }
    public enum Rank { Ace = 1, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King }

    public Suit suit;
    public Rank rank;

    public CardData(Suit s, Rank r)
    {
        suit = s;
        rank = r;
    }
}