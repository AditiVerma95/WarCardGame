using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

public class PlayManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DealManager dealManager;

    [Tooltip("Slots inside the 2x2 grid (assign in Inspector)")]
    [SerializeField] private Transform[] boardSlots = new Transform[4];

    [Header("Animation Settings")]
    [SerializeField] private float moveDuration = 0.5f;
    [SerializeField] private float flipDuration = 0.25f;

    public void PlayRound()
    {
        StartCoroutine(PlaySequence());
    }

    private IEnumerator PlaySequence()
    {
        // Player 1 -> slot 0
        yield return PlayCardFromQueue(dealManager.player1Cards, boardSlots[0]);

        // Player 2 -> slot 1
        yield return PlayCardFromQueue(dealManager.player2Cards, boardSlots[1]);

        // Player 3 -> slot 2
        yield return PlayCardFromQueue(dealManager.player3Cards, boardSlots[2]);

        // Player 4 -> slot 3
        yield return PlayCardFromQueue(dealManager.player4Cards, boardSlots[3]);
    }

    private IEnumerator PlayCardFromQueue(Queue<CardData> queue, Transform slot)
    {
        if (queue.Count == 0) yield break;

        CardData data = queue.Dequeue();
        GameObject cardGO = data.gameObject;

        Sequence seq = DOTween.Sequence();

        // Move to this player's slot
        seq.Append(cardGO.transform.DOMove(slot.position, moveDuration).SetEase(Ease.OutCubic));

        // Flip animation (90° → swap → +90°)
        seq.Append(cardGO.transform.DORotate(new Vector3(0, 90, 0), flipDuration))
           .AppendCallback(() => data.ShowFront())
           .Append(cardGO.transform.DORotate(new Vector3(0, 180, 0), flipDuration));

        bool finished = false;
        seq.OnComplete(() =>
        {
            // Parent to slot after animation so layout works
            cardGO.transform.SetParent(slot, false);
            finished = true;
        });

        yield return new WaitUntil(() => finished);
        yield return new WaitForSeconds(0.2f);
    }
}
