using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; // DOTween namespace

public class DealManager : MonoBehaviour
{
    public CardSpawner cardSpawner;
    public Button dealButton;

    [Header("Player Areas (empty GameObjects)")]
    public Transform[] playerAreas; // 4 player targets

    private Queue<CardData>[] playerHands;

    void Start()
    {
        // Initialize 4 queues
        playerHands = new Queue<CardData>[4];
        for (int i = 0; i < 4; i++)
            playerHands[i] = new Queue<CardData>();

        dealButton.onClick.AddListener(DealCards);
    }

    void DealCards()
    {
        List<CardData> deck = cardSpawner.GetDeck();
        if (deck == null || deck.Count == 0)
        {
            Debug.LogError("Deck is empty!");
            return;
        }

        // Clear old hands if re-dealing
        for (int i = 0; i < 4; i++)
            playerHands[i].Clear();

        // Deal round-robin
        int playerIndex = 0;
        foreach (CardData card in deck)
        {
            playerHands[playerIndex].Enqueue(card);

            // Animate card to the player’s area
            MoveCardToPlayer(card.gameObject, playerAreas[playerIndex]);

            playerIndex = (playerIndex + 1) % 4;
        }

        Debug.Log("Cards dealt with animation!");
    }

    void MoveCardToPlayer(GameObject card, Transform targetArea)
    {
        // Animate to target with DOTween
        card.transform.SetParent(targetArea); // optional: set parent
        card.transform.DOMove(targetArea.position, 0.5f).SetEase(Ease.OutQuad);
        card.transform.DORotate(Vector3.zero, 0.5f); // face upright
    }

    public Queue<CardData> GetPlayerHand(int playerIndex)
    {
        if (playerIndex < 0 || playerIndex >= 4) return null;
        return playerHands[playerIndex];
    }
}
