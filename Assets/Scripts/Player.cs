using System.Collections.Generic;
using UnityEngine;

public enum PlayerType { Human, AI }

public class Player
{
    public string playerName;
    public PlayerType type;
    public Queue<CardData> hand = new Queue<CardData>();
    public Transform areaTransform; // where to keep cards

    public Player(string name, PlayerType type, Transform area)
    {
        this.playerName = name;
        this.type = type;
        this.areaTransform = area;
    }

    // Play the top card
    public CardData PlayCard()
    {
        if (hand.Count == 0) return null;

        CardData card = hand.Dequeue();
        card.ShowFront(); // flip to face when played
        return card;
    }

    // Collect won cards
    public void CollectCards(List<CardData> wonCards)
    {
        foreach (var card in wonCards)
        {
            hand.Enqueue(card);
            card.transform.SetParent(areaTransform);
            card.transform.localPosition = Vector3.zero; // stack neatly
            card.ShowBack(); // face down again
        }
    }
}
