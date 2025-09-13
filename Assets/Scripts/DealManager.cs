using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DealManager : MonoBehaviour
{
    [Header("References")]
    public CardSpawner cardSpawner;
    public Button dealButton;
    public Button playRoundButton;

    [Header("Player Areas (empty GameObjects)")]
    public Transform[] playerAreas; // must be size 4
    public Transform battleArea;

    private List<Player> players = new List<Player>();
    private List<CardData> roundCards = new List<CardData>();
    private bool isRoundInProgress = false;

    void Start()
    {
        // ✅ Null-safety checks
        if (playerAreas == null || playerAreas.Length < 4)
        {
            Debug.LogError("⚠️ DealManager: Please assign 4 Player Areas in the Inspector!");
            return;
        }
        if (cardSpawner == null)
        {
            Debug.LogError("⚠️ DealManager: CardSpawner is not assigned!");
            return;
        }
        if (dealButton == null || playRoundButton == null)
        {
            Debug.LogError("⚠️ DealManager: Buttons not assigned!");
            return;
        }
        if (battleArea == null)
        {
            Debug.LogError("⚠️ DealManager: BattleArea is missing!");
            return;
        }

        // Create players
        players.Add(new Player("You", PlayerType.Human, playerAreas[0]));
        players.Add(new Player("AI 1", PlayerType.AI, playerAreas[1]));
        players.Add(new Player("AI 2", PlayerType.AI, playerAreas[2]));
        players.Add(new Player("AI 3", PlayerType.AI, playerAreas[3]));

        dealButton.onClick.AddListener(DealCards);
        playRoundButton.onClick.AddListener(PlayRound);
    }

    void DealCards()
    {
        List<CardData> deck = cardSpawner.GetDeck();
        if (deck == null || deck.Count == 0)
        {
            Debug.LogWarning("⚠️ DealManager: No deck available to deal.");
            return;
        }

        foreach (var p in players)
            p.hand.Clear();

        int playerIndex = 0;
        foreach (CardData card in deck)
        {
            players[playerIndex].hand.Enqueue(card);
            card.transform.SetParent(playerAreas[playerIndex]);

            // neatly space cards out
            int cardCount = players[playerIndex].hand.Count;
            float spacing = 30f;
            card.transform.localPosition = new Vector3((cardCount - 1) * spacing, 0, 0);

            // show card back until played
            card.ShowBack();

            playerIndex = (playerIndex + 1) % players.Count;
        }

        Debug.Log("✅ Cards dealt!");
    }

    public void PlayRound()
    {
        if (isRoundInProgress) return;
        StartCoroutine(RoundCoroutine());
    }

    IEnumerator RoundCoroutine()
    {
        isRoundInProgress = true;
        roundCards.Clear();

        // Human plays first
        PlayTurn(0);
        yield return new WaitForSeconds(1f);

        // AI players
        for (int i = 1; i < players.Count; i++)
        {
            PlayTurn(i);
            yield return new WaitForSeconds(1f);
        }

        // Evaluate winner
        yield return new WaitForSeconds(1f);
        int winnerIndex = GetRoundWinner();
        Debug.Log(players[winnerIndex].playerName + " wins the round!");

        // Winner collects cards
        players[winnerIndex].CollectCards(roundCards);
        roundCards.Clear();

        isRoundInProgress = false;
    }

    void PlayTurn(int playerIndex)
    {
        Player p = players[playerIndex];
        CardData card = p.PlayCard();
        if (card == null) return;

        roundCards.Add(card);

        // Animate into battle area
        Vector3 offset = new Vector3(playerIndex * 1.5f, 0, 0);
        card.transform.SetParent(battleArea);
        card.transform.DOMove(battleArea.position + offset, 0.5f);

        // Reveal human card
        if (p.type == PlayerType.Human)
            card.ShowFront();
    }

    int GetRoundWinner()
    {
        int winnerIndex = 0;
        int highestRank = -1;

        for (int i = 0; i < roundCards.Count; i++)
        {
            int rankValue = (int)roundCards[i].rank;
            if (rankValue > highestRank)
            {
                highestRank = rankValue;
                winnerIndex = i;
            }
        }

        return winnerIndex;
    }
}
