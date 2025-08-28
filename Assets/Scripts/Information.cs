using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;

[System.Serializable]
public class Info //json 양식이랑 똑같음
{
    public string Corporation; // 기업명
    public string HeadLine; //정보 제목
    public string Content; //정보 내용

    public string Type;//정보 종류 (News, Rumor)
    public float Volatility; //주가 변동폭 0~100
    public string Direction; //주가 방향성 (Up, Down)

    public float GetDirection()
    {
        if(Direction == "Up")
        {
            return 1f;
        }
        else
        {
            return -1f;
        }
    }
}


[System.Serializable]
public class InfoList
{
    public List<Info> information;
}

public class Information : MonoBehaviour
{

    //한 턴에 제공할 정보의 개수
    public int InfoPerTurn = 5;
    //전체 정보 리스트
    public List<Info> allInformation = new List<Info>();

    public bool OnlyNews = false; //턴 마다 제공되는 정보는 뉴스와 루머가 섞여있음 -> 스킬 사용시 뉴스만 제공

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

        if (OnlyNews)//스킬 사용했을 때, 기업별로 랜덤 뉴스 1개씩만 선택해서 보여줌
        {
            // 1. News만 필터링
            var allNews = allInformation.Where(info => info.Type == "News");

            // 2. 필터링된 뉴스들을 회사 이름(Corporation) 기준으로 그룹화합니다.
            var newsByCompany = allNews.GroupBy(info => info.Corporation);

            var resultList = new List<Info>();

            // 3. 각 회사 그룹을 순회하며, 그룹 내에서 뉴스 1개를 무작위로 선택합니다.
            foreach (var group in newsByCompany)
            {
                // 해당 회사에 뉴스가 하나라도 있는지 확인합니다.
                if (group.Any())
                {
                    int randomIndex = Random.Range(0, group.Count());
                    resultList.Add(group.ElementAt(randomIndex));
                }
            }

            // 4. 중요: 스킬 효과는 다음 한 턴에만 적용되므로, 플래그를 다시 false로 되돌립니다.
            OnlyNews = false;

            return resultList;
        }
        else
        {
            // 1. 회사 이름(Corporation)을 기준으로 모든 정보를 그룹화합니다.
            var infoByCompany = allInformation.GroupBy(info => info.Corporation);

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
    }





    //test함수
    public List<Info> TestGetRandomInformation()
    {
        var result = GetRandomInformation();
        Debug.Log($"--- 테스트 결과: {result.Count}개의 정보 ---");
        foreach (var info in result)
        {
            // 각 정보(info)의 헤드라인을 개별적으로 출력합니다.
            Debug.Log($"회사: {info.Corporation}, 헤드라인: {info.HeadLine}");
        }

        return result;
    }
    /*
    test
    void Start()
    {
        TestGetRandomInformation();
    }
    */
}
