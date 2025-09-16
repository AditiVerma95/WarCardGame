using UnityEngine;
using UnityEngine.UI;

public class CardData : MonoBehaviour
{
    public Rank rank;
    public Suit suit;
    public Sprite faceSprite;
    public Sprite backSprite;

    private Image img;

    void Awake()
    {
        img = GetComponentInChildren<Image>();
        ShowBack(); // start face down
    }

    public void ShowFront() => img.sprite = faceSprite;
    public void ShowBack() => img.sprite = backSprite;
}
