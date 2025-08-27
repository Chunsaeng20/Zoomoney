using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework.Internal;

[System.Serializable]
public class Info //json 양식이랑 똑같음
{
    public string Coperation; // 기업명
    public string HeadLine; //정보 제목
    public string Content; //정보 내용

    public string Type;//정보 종류 (News, Rumor)
    public float Volatility; //주가 변동폭 0~100
    public string Direction; //주가 방향성 (Up, Down)
}
//
[System.Serializable]
public class InfoList
{
    public List<Info> information;
}
public class Information : MonoBehaviour
{
    //json파일 경로
    private string jsonFilePath = "Assets/Scripts/InformationData.json";

    //한 턴에 제공할 정보의 개수
    public int InfoPerTurn = 5;
    //전체 정보 리스트
    public List<Info> allInformation = new List<Info>();

    private void Awake()
    {
        // Application.dataPath는 Assets 폴더까지의 경로를 의미합니다.
        string fullPath = Path.Combine(Application.dataPath, "Scripts", "InformationData.json");

        // 파일을 읽기 전에 존재하는지 먼저 확인합니다.
        if (File.Exists(fullPath))
        {
            //json파일 경로 읽어서 allinformation에 추가
            string jsonString = File.ReadAllText(fullPath);
            InfoList infoList = JsonUtility.FromJson<InfoList>(jsonString);
            allInformation = infoList.information;
            Debug.Log("json file loaded successfully");
        }
        else
        {
            // 파일이 없을 경우, 어떤 파일이 없는지 명확하게 알려줍니다.
            Debug.LogError($"[Information] 파일을 찾을 수 없습니다! 경로를 확인하세요: {fullPath}");
        }
    }

    // 회사별로 1개의 정보를 무작위로 뽑아 리스트로 반환합니다.
    public List<Info> GetRandomInformation()
    {
        if (allInformation == null || allInformation.Count == 0)
        {
            Debug.LogError("[Information] 정보 데이터가 없습니다!");
            return new List<Info>();
        }

        // 1. 회사 이름(Coperation)을 기준으로 모든 정보를 그룹화합니다.
        var infoByCompany = allInformation.GroupBy(info => info.Coperation);

        var resultList = new List<Info>();

        // 2. 각 회사 그룹을 순회하며, 그룹 내에서 정보 1개를 무작위로 선택합니다.
        foreach (var group in infoByCompany)
        {
            // 그룹 내에서 랜덤 인덱스를 뽑습니다.
            int randomIndex = Random.Range(0, group.Count());
            // 해당 인덱스의 Info 객체를 결과 리스트에 추가합니다.
            resultList.Add(group.ElementAt(randomIndex));
        }

        // 3. 각 회사별 정보가 1개씩 담긴 최종 리스트를 반환합니다.
        return resultList;
    }
    //test함수
    public List<Info> TestGetRandomInformation()
    {
        var result = GetRandomInformation();
        Debug.Log("TestGetRandomInformation");
        return result;
    }
    //test
    void Start()
    {
        TestGetRandomInformation();
    }
}
