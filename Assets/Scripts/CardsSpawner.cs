using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // for UI Image

public class CardsSpawner : MonoBehaviour 
{
    public GameObject cardPrefab;     // UI prefab with an Image component
    public Sprite[] cardSprites;      // all 52 card faces
    public Sprite cardBackSprite;     // card back image
    public Transform deckParent;      // parent under Canvas (e.g., an empty GameObject)

    private List<CardData> deck = new List<CardData>();

    void Start()
    {
        CreateDeck();
        ShuffleDeck();
        SpawnDeck();
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

    void SpawnDeck()
    {
        float offset = -1.5f; 

        for (int i = 0; i < deck.Count; i++)
        {
            GameObject newCard = Instantiate(cardPrefab, deckParent);
            newCard.GetComponentInChildren<Image>().sprite = cardBackSprite; // show back first

            // Stack with small offset
            RectTransform rt = newCard.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2( i * offset, 0);
        }
    }
}
