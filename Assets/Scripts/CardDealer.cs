using System.Collections.Generic;
using UnityEngine;

public class CardDealer : MonoBehaviour
{
    private CardSpawner spawner;

    private List<Transform> playerAreas = new List<Transform>();

    void Start()
{
    spawner = FindFirstObjectByType<CardSpawner>();
    Debug.Log("Spawner found? " + (spawner != null));

    GameObject playersRoot = GameObject.Find("Players");
    Debug.Log("Players root found? " + (playersRoot != null));

    if (playersRoot != null)
    {
        foreach (Transform player in playersRoot.transform)
        {
            Transform cardArea = player.Find("CardArea");
            Debug.Log(player.name + " CardArea found? " + (cardArea != null));
            if (cardArea != null)
                playerAreas.Add(cardArea);
        }
    }

    DealCards();
}


    void DealCards()
    {
        int playerCount = playerAreas.Count;
        List<CardData> deck = spawner.Deck;

        for (int i = 0; i < deck.Count; i++)
        {
            int playerIndex = i % playerCount;

            GameObject card = Instantiate(spawner.cardPrefab, playerAreas[playerIndex]);
            UnityEngine.UI.Image cardImage = card.GetComponentInChildren<UnityEngine.UI.Image>();


            if (cardImage != null)
            {
                cardImage.sprite = spawner.GetCardSprite(deck[i]);
            }
        }
    }
}
