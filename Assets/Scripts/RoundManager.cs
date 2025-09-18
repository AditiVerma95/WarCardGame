using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PlayedCard
{
    public int playerIndex;
    public CardData card;

    public PlayedCard(int playerIndex, CardData card)
    {
        this.playerIndex = playerIndex;
        this.card = card;
    }
}

public class RoundManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DealManager dealManager;
    [SerializeField] private PlayManager playManager;

    private List<PlayedCard> currentRound = new List<PlayedCard>();

    public void StartRound()
    {
        StartCoroutine(RunRound());
    }

    private IEnumerator RunRound()
    {
        currentRound.Clear();

        // Each player plays one card
        for (int i = 0; i < 4; i++)
        {
            Queue<CardData> queue = GetQueueForPlayer(i);
            if (queue.Count == 0) continue;

            CardData card = queue.Peek();
            yield return playManager.PlayCardFromQueue(queue, playManager.boardSlots[i]);

            currentRound.Add(new PlayedCard(i, card));
        }

        // Decide winner
        yield return ResolveRound(currentRound);
    }

    private IEnumerator ResolveRound(List<PlayedCard> pile)
    {
        List<int> winners = GetRoundWinners(pile);

        if (winners.Count == 1)
        {
            int winner = winners[0];
            Debug.Log($"Player {winner + 1} wins round!");

            yield return AwardCards(winner, pile);
        }
        else
        {
            Debug.Log("WAR triggered!");
            yield return PlayWar(winners, pile);
        }
    }

    private IEnumerator PlayWar(List<int> tiedPlayers, List<PlayedCard> pile)
    {
        foreach (int player in tiedPlayers)
        {
            Queue<CardData> queue = GetQueueForPlayer(player);
            if (queue.Count < 4)
            {
                Debug.Log($"Player {player + 1} ran out of cards!");
                continue;
            }

            // Burn 3 face-down
            for (int i = 0; i < 3; i++)
            {
                CardData burn = queue.Dequeue();
                pile.Add(new PlayedCard(player, burn));
                yield return playManager.PlayCardFaceDown(burn, playManager.boardSlots[player]);
            }

            // Play 1 face-up
            CardData warCard = queue.Peek();
            yield return playManager.PlayCardFromQueue(queue, playManager.boardSlots[player]);
            pile.Add(new PlayedCard(player, warCard));
        }

        // Re-resolve
        yield return ResolveRound(pile);
    }

    private List<int> GetRoundWinners(List<PlayedCard> roundCards)
    {
        int best = -1;
        List<int> winners = new List<int>();

        foreach (var pc in roundCards)
        {
            int value = GetRankValue(pc.card.rank);
            if (value > best)
            {
                best = value;
                winners.Clear();
                winners.Add(pc.playerIndex);
            }
            else if (value == best)
            {
                winners.Add(pc.playerIndex);
            }
        }
        return winners;
    }

    private IEnumerator AwardCards(int winner, List<PlayedCard> pile)
    {
        Queue<CardData> queue = GetQueueForPlayer(winner);
        Transform winPile = playManager.winPiles[winner];

        foreach (var pc in pile)
        {
            queue.Enqueue(pc.card);
            yield return playManager.MoveToWinPile(pc.card, winPile);
        }

        pile.Clear();
    }

    private Queue<CardData> GetQueueForPlayer(int index)
    {
        switch (index)
        {
            case 0: return dealManager.player1Cards;
            case 1: return dealManager.player2Cards;
            case 2: return dealManager.player3Cards;
            case 3: return dealManager.player4Cards;
            default: return null;
        }
    }

    private int GetRankValue(Rank rank)
    {
        switch (rank)
        {
            case Rank.Two: return 2;
            case Rank.Three: return 3;
            case Rank.Four: return 4;
            case Rank.Five: return 5;
            case Rank.Six: return 6;
            case Rank.Seven: return 7;
            case Rank.Eight: return 8;
            case Rank.Nine: return 9;
            case Rank.Ten: return 10;
            case Rank.Jack: return 11;
            case Rank.Queen: return 12;
            case Rank.King: return 13;
            case Rank.Ace: return 14;
            default: return 0;
        }
    }
}
