using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class DealManager : MonoBehaviour
{
    public GameObject dealButton;
    public static DealManager Instance;
    [Header("References")]
    [SerializeField] private Transform deckPosition;
    [SerializeField] private Transform playerArea1;
    [SerializeField] private Transform playerArea2;
    [SerializeField] private Transform playerArea3;
    [SerializeField] private Transform playerArea4;

    [Header("Settings")]
    [SerializeField] private float slideDuration = 0.6f;
    [SerializeField] private float spreadOffset = 0.3f; // spacing between cards

    public Queue<CardData> player1Cards = new Queue<CardData>();
    public Queue<CardData> player2Cards = new Queue<CardData>();
    public Queue<CardData> player3Cards = new Queue<CardData>();
    public Queue<CardData> player4Cards = new Queue<CardData>();

    [Header("Debug Queues (Inspector Only)")]
    [SerializeField] private List<CardData> debugPlayer1Cards = new List<CardData>();
    [SerializeField] private List<CardData> debugPlayer2Cards = new List<CardData>();
    [SerializeField] private List<CardData> debugPlayer3Cards = new List<CardData>();
    [SerializeField] private List<CardData> debugPlayer4Cards = new List<CardData>();

    private List<CardData> deck;
    
    public void Awake()
    {
        Instance = this;
    }

    public void StartDealing()
    {
        deck = CardSpawner.Instance.GetDeck();

        if (deck == null || deck.Count != 52)
        {
            Debug.LogError("Deck is not ready or does not have 52 cards!");
            return;
        }

        DealCardsAllAtOnce();
        dealButton.SetActive(false);
    }

    private void DealCardsAllAtOnce()
    {
        Transform[] playerAreas = { playerArea1, playerArea2, playerArea3, playerArea4 };
        Queue<CardData>[] playerQueues = { player1Cards, player2Cards, player3Cards, player4Cards };

        int cardIndex = 0;

        for (int player = 0; player < playerAreas.Length; player++)
        {
            for (int i = 0; i < 13; i++)
            {
                CardData cardData = deck[cardIndex];
                cardIndex++;

                // Reset card to deck position
                cardData.transform.position = deckPosition.position;

                // Calculate offset so cards spread instead of overlap
                Vector3 targetPos = playerAreas[player].position + new Vector3(i * spreadOffset, 0f, 0f);

                // Animate card
                cardData.transform.DOMove(targetPos, slideDuration)
                    .SetEase(Ease.OutQuad);

                // Add to player queue
                playerQueues[player].Enqueue(cardData);

            }
            SyncDebugQueues();
        }
    }
    private void SyncDebugQueues()
    {
        debugPlayer1Cards = new List<CardData>(player1Cards);
        debugPlayer2Cards = new List<CardData>(player2Cards);
        debugPlayer3Cards = new List<CardData>(player3Cards);
        debugPlayer4Cards = new List<CardData>(player4Cards);
    }



}
