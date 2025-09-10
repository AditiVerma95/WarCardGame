using System;
using System.Collections.Generic;
using UnityEngine;

public class CardSpawner : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private GameObject cardHolder;

    void Awake()
    {
        CreateDeck();
        //ShuffleDeck();
    }

    void CreateDeck()
    {
        float xPos = 0f;
        float xOffset = 1.5f;

        Rank[] ranks = (Rank[])Enum.GetValues(typeof(Rank));
        Suit[] suits = (Suit[])Enum.GetValues(typeof(Suit));

        // There are 13 ranks and 4 suits
        int numRanks = ranks.Length;
        int numSuits = suits.Length;

        for (int i = 0; i < numRanks * numSuits; i++)
        {
            GameObject cardObject = Instantiate(cardPrefab, cardHolder.transform);

            RectTransform cardRect = cardObject.GetComponent<RectTransform>();
            cardRect.anchoredPosition = new Vector2(xPos, 0f); // Use Vector2 for RectTransform
            xPos += xOffset;

            CardData cardData = cardObject.GetComponent<CardData>();

            // Correctly assign suit and rank
            cardData.rank = ranks[i % numRanks]; // Loops through 0-12
            cardData.suit = suits[i / numRanks]; // Changes after every 13 cards

            // Add the newly created card to the global list
            CardsManager.Instance.globalCards.Add(cardData);
        }
    }


    // void ShuffleDeck()
    // {
    //     for (int i = 0; i < deck.Count; i++)
    //     {
    //         CardData temp = deck[i];
    //         int randomIndex = Random.Range(i, deck.Count);
    //         deck[i] = deck[randomIndex];
    //         deck[randomIndex] = temp;
    //     }
    // }

    // public Sprite GetCardSprite(CardData card)
    // {
    //     int suitIndex = (int)card.suit;
    //     int rankIndex = (int)card.rank - 1;
    //     int spriteIndex = (suitIndex * 13) + rankIndex;
    //     return cardSprites[spriteIndex];
    // }
}
