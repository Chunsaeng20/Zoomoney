using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class MoveCard : MonoBehaviour
{

    public GameObject GO;
    public Trader trader;
    public GameObject[] cardPrefab;
    public Transform parentObject;
    [SerializeField] public List<TraderInfo> LeftCards;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        trader = GO.GetComponent<Trader>();
        LeftCards = trader.traderList;
        UpdateCard();
    }

    void Update()
    {

    }
    void UpdateCard()
    {
        
        for (int i = 0; i < LeftCards.Count(); i++)
        {
            cardPrefab[i].SetActive(true);
        }
    }
}
