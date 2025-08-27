using System.Collections.Generic;
using UnityEngine;

public enum Sectors
{
    IT = 0,
    Retail = 1,
    Entertainment = 2,
}
public class StockInformation
{
    public string ID; // 주식 이름
    public Sectors Sector; // 섹터(분야)
    public List<float> PreviousStockPrice; // 이전 가격
    public float CurrentStockPrice; // 현재 가격

    // 새로운 현재 가격으로 교체
    public void ReplaceCurrentStockPrice(float NewStockPrice)
    {
        PreviousStockPrice.Add(CurrentStockPrice);
        CurrentStockPrice = NewStockPrice;
    }

    // 모든 가격 정보 삭제
    public void Flush()
    {
        PreviousStockPrice.Clear();
        CurrentStockPrice = 0;
    }
}

public class Stock : MonoBehaviour
{
    public List<StockInformation> stockList;

    // 초기화 함수 필요
}
