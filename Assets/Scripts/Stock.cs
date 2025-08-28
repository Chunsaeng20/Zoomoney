using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum Sectors
{
    IT = 0,
    유통 = 1,
    엔터 = 2,
}
[System.Serializable]
public class StockInformation
{
    public string ID; // 주식 이름
    public Sectors Sector; // 섹터(분야)
    public List<float> PreviousStockPrice; // 이전 가격
    public float CurrentStockPrice; // 현재 가격
    public float initialStockPrice; // 초기 가격

    // [권장] 생성자에서 리스트를 미리 초기화하여 Null 오류를 방지합니다.
    public StockInformation()
    {
        PreviousStockPrice = new List<float>();
    }
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

[System.Serializable]
public class StockData
{
    // 이 변수 이름 "stocks"가 JSON 파일의 키와 일치해야 합니다.
    public List<StockInformation> stocks;
}
public class Stock : MonoBehaviour
{
    public List<StockInformation> stockList;

    // 초기화 함수 필요
    void Awake()
    {
        string fullPath = Path.Combine(Application.dataPath, "Scripts", "StockData.json");
        // 파일을 읽기 전에 존재하는지 먼저 확인합니다.
        if (File.Exists(fullPath))
        {
          
            string jsonString = File.ReadAllText(fullPath);
            // 정의한 래퍼 클래스(StockData)를 사용하여 JSON을 파싱합니다.
            StockData loadedData = JsonUtility.FromJson<StockData>(jsonString);

            // 선언된 변수 stockList에 파싱한 데이터를 할당합니다.
            stockList = loadedData.stocks;

            Debug.Log("JSON file loaded successfully. " + stockList.Count + " stock items loaded.");
        }
        else
        {
            // 파일이 없을 경우, 어떤 파일이 없는지 명확하게 알려줍니다.
            Debug.LogError($"[Stock] 파일을 찾을 수 없습니다! 경로를 확인하세요: {fullPath}");
        }
    }
}
