using System;
using System.Collections.Generic;
using UnityEngine;

public class CardSpawner : MonoBehaviour
{
    public static CardSpawner Instance;

    [Header("Prefabs & Holders")]
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private GameObject cardHolder;

    [Header("Card Sprites (order: Ace → King)")]
    [SerializeField] private Sprite[] heartSprites;
    [SerializeField] private Sprite[] diamondSprites;
    [SerializeField] private Sprite[] clubSprites;
    [SerializeField] private Sprite[] spadeSprites;

    public List<CardData> globalCards = new List<CardData>();
    private List<CardData> deck = new List<CardData>();

    void Awake()
    {
        Instance = this;
        CreateDeck();
        ShuffleDeck();
    }

    void CreateDeck()
    {
        float xPos = 0f;
        float xOffset = 1f;

        Rank[] ranks = (Rank[])Enum.GetValues(typeof(Rank));
        Suit[] suits = (Suit[])Enum.GetValues(typeof(Suit));

        deck.Clear();
        globalCards.Clear();

        for (int s = 0; s < suits.Length; s++)
        {
            for (int r = 0; r < ranks.Length; r++)
            {
                GameObject cardObject = Instantiate(cardPrefab, cardHolder.transform);

                RectTransform cardRect = cardObject.GetComponent<RectTransform>();
                cardRect.anchoredPosition = new Vector2(xPos, 0f);
                xPos += xOffset;

                CardData cardData = cardObject.GetComponent<CardData>();
                cardData.rank = ranks[r];
                cardData.suit = suits[s];
                cardData.faceSprite = GetSpriteForCard(r, suits[s]); // ✅ assign correct sprite
                cardData.ShowBack(); // optional: start face down

                deck.Add(cardData);
                globalCards.Add(cardData);
            }
        }
    }

    Sprite GetSpriteForCard(int rankIndex, Suit suit)
    {
        // rankIndex = 0 → Ace, 12 → King
        switch (suit)
        {
            case Suit.Hearts: return heartSprites[rankIndex];
            case Suit.Diamonds: return diamondSprites[rankIndex];
            case Suit.Clubs: return clubSprites[rankIndex];
            case Suit.Spades: return spadeSprites[rankIndex];
            default: return null;
        }
    }

    void ShuffleDeck()
    {
        System.Random rng = new System.Random();
        int n = deck.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            (deck[k], deck[n]) = (deck[n], deck[k]);
        }
        Debug.Log("Deck shuffled!");
    }

    public List<CardData> GetDeck()
    {
        return deck;
    }
}
