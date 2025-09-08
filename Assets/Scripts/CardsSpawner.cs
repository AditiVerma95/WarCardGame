using System.Collections.Generic;
using UnityEngine;

// You can rename this file to DeckController.cs for clarity
public class CardsSpawner : MonoBehaviour 
{
    // Drag your PARENT prefab ("CardTemplate") here in the Inspector
    public GameObject cardPrefab;

    // Drag all 52 of your card face sprites here in the Inspector
    public Sprite[] cardSprites; 

    private List<CardData> deck = new List<CardData>();
    private List<GameObject> spawnedCards = new List<GameObject>();

    void Start()
    {
        CreateDeck();
        ShuffleDeck();
        SpawnCards();
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

    void SpawnCards()
    {
        float cardOffset = 0.01f;

        for(int i = 0; i < deck.Count; i++)
        {
            Vector3 spawnPosition = transform.position + new Vector3(0, i * cardOffset, 0);
            GameObject newCard = Instantiate(cardPrefab, spawnPosition, Quaternion.identity);

            // --- THIS IS THE IMPORTANT CHANGE ---
            // We get the SpriteRenderer from the child object.
            SpriteRenderer cardRenderer = newCard.GetComponentInChildren<SpriteRenderer>();

            // Find the correct sprite and assign it
            Sprite cardSprite = FindCardSprite(deck[i].suit, deck[i].rank);
            if (cardRenderer != null && cardSprite != null)
            {
                cardRenderer.sprite = cardSprite;
            }
            
            spawnedCards.Add(newCard);
        }
    }

    Sprite FindCardSprite(CardData.Suit suit, CardData.Rank rank)
    {
        // This logic assumes you ordered your sprites in the Inspector:
        // All Clubs (A-K), then Diamonds (A-K), then Hearts (A-K), then Spades (A-K)
        int suitIndex = (int)suit;
        int rankIndex = (int)rank - 1;
        int spriteIndex = (suitIndex * 13) + rankIndex;
        
        // Safety check
        if (spriteIndex >= 0 && spriteIndex < cardSprites.Length)
        {
            return cardSprites[spriteIndex];
        }
        return null; // Return null if sprite not found
    }
}
