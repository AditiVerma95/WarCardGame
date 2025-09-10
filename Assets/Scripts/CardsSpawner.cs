using System.Collections.Generic;
using UnityEngine;

public class CardSpawner : MonoBehaviour
{
    public GameObject cardPrefab;
    public Sprite[] cardSprites;

    private List<CardData> deck = new List<CardData>();

    public List<CardData> Deck => deck;

    void Awake()
    {
        CreateDeck();
        ShuffleDeck();
    }

    void CreateDeck()
    {
        foreach (CardData.Suit suit in System.Enum.GetValues(typeof(CardData.Suit)))
        {
            foreach (CardData.Rank rank in System.Enum.GetValues(typeof(CardData.Rank)))
            {
                deck.Add(new CardData(suit, rank));
            }
        }
    }

    void ShuffleDeck()
    {
        for (int i = 0; i < deck.Count; i++)
        {
            CardData temp = deck[i];
            int randomIndex = Random.Range(i, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
    }

    public Sprite GetCardSprite(CardData card)
    {
        int suitIndex = (int)card.suit;
        int rankIndex = (int)card.rank - 1;
        int spriteIndex = (suitIndex * 13) + rankIndex;
        return cardSprites[spriteIndex];
    }
}
