using System.Collections.Generic;
using UnityEngine;

public class MoveCard : MonoBehaviour
{

    public GameManager gameManager;
    [SerializeField] public List<TraderInfo> LeftCards;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LeftCards = gameManager.trader.traderList;
    }
}
