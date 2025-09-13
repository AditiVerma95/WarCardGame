using UnityEngine;
using UnityEngine.UI;

public class CardData : MonoBehaviour
{
    public Rank rank;
    public Suit suit;
    public Sprite faceSprite;
    public Sprite backSprite;

    private Image image;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    public void ShowFront()
    {
        if (image != null)
            image.sprite = faceSprite;
    }

    public void ShowBack()
    {
        if (image != null)
            image.sprite = backSprite;
    }
}