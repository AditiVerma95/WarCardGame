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

        int rankIndex = 0;
        int suitIndex = 0;
        Rank[] ranks = (Rank[])Enum.GetValues(typeof(Rank));
        Suit[] suits = (Suit[])Enum.GetValues(typeof(Suit));


        for (int i = 0; i < 52; i++)
        {
            GameObject cardObject = Instantiate(cardPrefab, cardHolder.transform);

            RectTransform cardRect = cardObject.GetComponent<RectTransform>();
            cardRect.anchoredPosition = new Vector3(xPos + xOffset, 0f, 0f);
            xPos += xOffset;

            CardData cardData = cardObject.GetComponent<CardData>();
            cardData.rank = ranks[rankIndex];
            cardData.suit = suits[suitIndex];
            if (rankIndex == 13)
            {
                suitIndex++;
                rankIndex = 0;
            }

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
