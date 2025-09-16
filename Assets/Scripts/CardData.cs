using UnityEngine;

public class CardData : MonoBehaviour
{
    [Header("Card Info")]
    public Rank rank;
    public Suit suit;

    [Header("Card Sprites")]
    public Sprite faceSprite;
    public Sprite backSprite;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ShowBack(); // by default, card starts face-down
    }

    public void ShowFront()
    {
        if (spriteRenderer != null)
            spriteRenderer.sprite = faceSprite;
    }

    public void ShowBack()
    {
        if (spriteRenderer != null)
            spriteRenderer.sprite = backSprite;
    }
}
