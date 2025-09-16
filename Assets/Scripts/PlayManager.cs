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

    [SerializeField, Range(0.1f, 1f)]
    private float slotSpacingFactor = 0.5f; // 1 = full distance, 0.5 = half distance, etc.

    void Start()
    {
        for (int i = 0; i < boardSlots.Length; i++)
        {
            RectTransform rt = boardSlots[i].GetComponent<RectTransform>();
            Vector2 offset = GetDistanceFromCanvasCenter(rt);

            // Scale the offset closer to center
            Vector2 newOffset = offset * slotSpacingFactor;

            // Apply new position
            rt.localPosition = newOffset;

            Debug.Log($"Slot {i} moved closer: {newOffset}");
        }
    }



    public void PlayRound()
    {
        StartCoroutine(PlaySequence());
    }

    Vector2 GetDistanceFromCanvasCenter(RectTransform target)
    {
        Canvas canvas = target.GetComponentInParent<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("Target is not inside a Canvas!");
            return Vector2.zero;
        }

        RectTransform canvasRect = canvas.GetComponent<RectTransform>();

        // World → local (canvas space)
        Vector3 localPos = canvasRect.InverseTransformPoint(target.position);

        // (x,y) offset from center
        return new Vector2(localPos.x, localPos.y);
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
        RectTransform rt = cardGO.GetComponent<RectTransform>();

        Sequence seq = DOTween.Sequence();

        // Move card to slot
        seq.Append(rt.DOMove(slot.position, moveDuration).SetEase(Ease.OutCubic));

        // Flip: rotate Y 0→90, swap sprite, then 90→0
        seq.Append(rt.DORotate(new Vector3(0, 90, 0), flipDuration))
           .AppendCallback(() => data.ShowFront()) // swap to face sprite
           .Append(rt.DORotate(Vector3.zero, flipDuration));

        bool finished = false;
        seq.OnComplete(() =>
        {
            //rt.SetParent(slot, false); // snap into grid after anim
            rt.rotation = Quaternion.identity; // reset rotation to 0,0,0
            finished = true;
        });

        yield return new WaitUntil(() => finished);
        yield return new WaitForSeconds(0.2f);
    }

}
