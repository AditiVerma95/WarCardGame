using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

public class PlayManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DealManager dealManager;

    [Tooltip("Slots inside the 2x2 grid (assign in Inspector)")]
    [SerializeField] public Transform[] boardSlots = new Transform[4];

    [Tooltip("Each player’s win pile position (assign in Inspector)")]
    [SerializeField] public Transform[] winPiles = new Transform[4];

    [Header("Animation Settings")]
    [SerializeField] private float moveDuration = 0.5f;
    [SerializeField] private float flipDuration = 0.25f;

    [SerializeField, Range(0.1f, 1f)]
    private float slotSpacingFactor = 0.5f;

    void Start()
    {
        for (int i = 0; i < boardSlots.Length; i++)
        {
            RectTransform rt = boardSlots[i].GetComponent<RectTransform>();
            Vector2 offset = GetDistanceFromCanvasCenter(rt);
            Vector2 newOffset = offset * slotSpacingFactor;
            rt.localPosition = newOffset;
        }
    }

    Vector2 GetDistanceFromCanvasCenter(RectTransform target)
    {
        Canvas canvas = target.GetComponentInParent<Canvas>();
        if (canvas == null) return Vector2.zero;

        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        Vector3 localPos = canvasRect.InverseTransformPoint(target.position);
        return new Vector2(localPos.x, localPos.y);
    }

    public IEnumerator PlayCardFromQueue(Queue<CardData> queue, Transform slot)
    {
        if (queue.Count == 0) yield break;

        CardData data = queue.Dequeue();
        GameObject cardGO = data.gameObject;
        RectTransform rt = cardGO.GetComponent<RectTransform>();

        Sequence seq = DOTween.Sequence();

        // Smooth move into the slot’s position (world space is fine here)
        seq.Append(rt.DOMove(slot.position, moveDuration).SetEase(Ease.OutCubic));

        // Flip animation
        seq.Append(rt.DORotate(new Vector3(0, 90, 0), flipDuration))
           .AppendCallback(() => data.ShowFront())
           .Append(rt.DORotate(Vector3.zero, flipDuration));

        bool finished = false;
        seq.OnComplete(() =>
        {
            rt.SetParent(slot, true); // keep its world-space offset
            rt.rotation = Quaternion.identity;
            finished = true;
        });

        yield return new WaitUntil(() => finished);
    }




    // === Face-down play (burn cards in WAR) ===
    public IEnumerator PlayCardFaceDown(CardData data, Transform slot)
    {
        GameObject cardGO = data.gameObject;
        RectTransform rt = cardGO.GetComponent<RectTransform>();

        data.ShowBack();

        Sequence seq = DOTween.Sequence();
        seq.Append(rt.DOMove(slot.position, moveDuration).SetEase(Ease.OutCubic));

        bool finished = false;
        seq.OnComplete(() =>
        {
            rt.SetParent(slot, false);
            rt.rotation = Quaternion.identity;
            finished = true;
        });

        yield return new WaitUntil(() => finished);
    }

    // === Send won cards into winner’s pile visually ===
    public IEnumerator MoveToWinPile(CardData card, Transform winPile)
    {
        RectTransform rt = card.GetComponent<RectTransform>();
        RectTransform winRT = winPile.GetComponent<RectTransform>();

        Canvas canvas = rt.GetComponentInParent<Canvas>();
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();

        // Convert both card + winPile to canvas-local space
        Vector3 cardLocal = canvasRect.InverseTransformPoint(rt.position);
        Vector3 winLocal = canvasRect.InverseTransformPoint(winRT.position);

        Sequence seq = DOTween.Sequence();
        // Animate from cardLocal to winLocal (canvas-relative)
        seq.Append(DOTween.To(
            () => cardLocal,
            x =>
            {
                rt.position = canvasRect.TransformPoint(x);
            },
            winLocal,
            moveDuration
        ).SetEase(Ease.InOutCubic));

        bool finished = false;
        seq.OnComplete(() =>
        {
            rt.SetParent(winPile, false);
            rt.localPosition = Vector3.zero; // snap cleanly inside pile
            finished = true;
        });

        yield return new WaitUntil(() => finished);
    }

}
