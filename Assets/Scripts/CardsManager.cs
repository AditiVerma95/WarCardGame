using System.Collections.Generic;
using UnityEngine;

public class CardsManager : MonoBehaviour
{
    public static CardsManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    public List<CardData> globalCards = new();
    
}