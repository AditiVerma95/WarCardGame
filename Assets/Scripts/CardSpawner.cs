using System;
using System.Collections.Generic;
using UnityEngine;


public class CardSpawner : MonoBehaviour
{
    public static CardSpawner Instance;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private GameObject cardHolder;
    public List<CardData> globalCards = new List<CardData>();
    private List<CardData> deck = new List<CardData>(); // Local deck reference

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

        int numRanks = ranks.Length;
        int numSuits = suits.Length;

        deck.Clear(); // important if re-creating deck

        for (int i = 0; i < numRanks * numSuits; i++)
        {
            GameObject cardObject = Instantiate(cardPrefab, cardHolder.transform);

            RectTransform cardRect = cardObject.GetComponent<RectTransform>();
            cardRect.anchoredPosition = new Vector2(xPos, 0f);
            xPos += xOffset;

            CardData cardData = cardObject.GetComponent<CardData>();

            cardData.rank = ranks[i % numRanks];
            cardData.suit = suits[i / numRanks];

            // Add card to local deck + global deck
            deck.Add(cardData);
            globalCards.Add(cardData);
        }
    }

    void ShuffleDeck()
    {
        System.Random rng = new System.Random();

        int n = deck.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1); // 0 <= k <= n
            CardData temp = deck[k];
            deck[k] = deck[n];
            deck[n] = temp;
        }

        Debug.Log("Deck shuffled!");
    }

    public List<CardData> GetDeck()
    {
        return deck;
    }
}
