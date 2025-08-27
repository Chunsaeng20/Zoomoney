using System.Collections.Generic;
using UnityEngine;

public enum Sectors
{
    IT = 0,
    Retail,
    Entertainment
}
public class StockInformation
{
    public string ID; //주식 이름
    public Sectors Sector; //섹터(분야)
    public float PriorStockPrice; //이전 가격
    public float CurrentStockPrice; //현재 가격
}
public class Stock : MonoBehaviour
{
    public List<StockInformation> stocks;
    public void StockPriceFluctation(List<float> Cur)//게임매니저 클래스에서 받은 변동폭으로 주가 변동
    {
        for (int i = 0; i < Cur.Count; i++)
        {
            stocks[i].PriorStockPrice = stocks[i].CurrentStockPrice;
            stocks[i].CurrentStockPrice = Cur[i];
        }
    }

    public void NewStockPrice(string id, float newStockPrice)
    {

    }

    public List<StockInformation> CurrentStocks()//현재 주식리스트를 게임매니저 클래스에 반환
    {
        return stocks;
    }
}