using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public enum PassiveSkill // 패시브 스킬 종류
{
    없음,
    허약,
    난딴돈의반만가져가, 
    관망,
}

public enum Trendency // 투자 성향
{
    보수적,
    중립적,
    공격적
}

public enum TraderFlag
{
    NONE = 0,
    SELECT = 1,
    HIRE = 2,
    FIRE = 3,
}


[System.Serializable]
public class TraderInfo // 트레이더의 정보를 담는 클래스
{
    public Trendency traderTendency; // 투자 성향
    public string traderName; // 트레이더 이름
    public float trendencyPerMoney; // 투자 성향에 따른 투자 비율
    public Sectors sector; // 전문 분야
    public float salary; // 월급
    public int stamina; // 체력
    public float confidence; // 신뢰도
    public float profit; // 수익
    public PassiveSkill passiveSkill; // 패시브
    public string skillScript; // 패시브 스킬 설명
    public TraderFlag flag;
}
public class Trader : MonoBehaviour
{

    public List<TraderInfo> traderList = new List<TraderInfo>();
    public List<TraderInfo> tr2 = new List<TraderInfo>();
    public int counter = 0;


    void Awake()
    {
        traderList.Add(generateTraders());
    }

    public TraderInfo generateTraders()
    {
        TraderInfo newTrader = new TraderInfo();

        // 1. 투자 성향 및 전문 분야 랜덤 설정
        newTrader.traderName = "트레이더" + (counter++);
        newTrader.traderTendency = (Trendency)Random.Range(0, 3); //트레이더의 투자 성향 랜덤 설정
        newTrader.sector = (Sectors)Random.Range(0, 3); //트레이더의 전문 분야 랜덤 설정

        // 2. 스킬 랜덤 설정 
        newTrader.passiveSkill = (PassiveSkill)Random.Range(0, 4); //트레이더의 패시브 랜덤 설정
        newTrader.skillScript = SkillDescription(newTrader); // 패시브 스킬 설명 설정
        newTrader.flag = TraderFlag.NONE;
        newTrader.trendencyPerMoney = SetMoneyByTrendency(newTrader.traderTendency); // 투자 성향에 따른 투자 금액 비율 설정

        // 3. 기본 능력치 설정
        newTrader.salary = Random.Range(300000, 600000);
        if (newTrader.passiveSkill == PassiveSkill.허약)
        {
            newTrader.stamina = 2;
        }
        else
        {
            newTrader.stamina = 3; // 범위 0~3
        }
        newTrader.confidence = 0f; // 범위 0~100
        newTrader.profit = 1f;
        return newTrader;

    }

    public float SetMoneyByTrendency(Trendency tr) // 투자 성향에 따른 투자 금액 비율 설정
    {
        float tendencyPerMoney = 0f;
        switch (tr)
        {
            case Trendency.보수적:
                tendencyPerMoney = Random.Range(0f, 0.7f);
                break;
            case Trendency.중립적:
                tendencyPerMoney = Random.Range(0.6f, 1.3f);
                break;
            case Trendency.공격적:
                tendencyPerMoney = Random.Range(1.4f, 2f);
                break;
        }
        tendencyPerMoney = (float)Math.Round(tendencyPerMoney, 1);// 소수점 첫째자리까지 반올림
        return tendencyPerMoney;
    }

    public float SkillManagement(TraderInfo tr) // 패시브 스킬에 따른 효과 발동
    {
        float effectValue = 1f;
        switch (tr.passiveSkill)
        {
            case PassiveSkill.없음:
                break;
            case PassiveSkill.허약:
                break;
            case PassiveSkill.난딴돈의반만가져가:
                effectValue = 0.5f;
                break;
            case PassiveSkill.관망:
                effectValue = 0f;
                break;
        }

        return effectValue;
    }
    public string SkillDescription(TraderInfo tr) // 패시브 스킬 설명
    {
        string description = "";
        switch (tr.passiveSkill)
        {
            case PassiveSkill.없음:
                description = "패시브 스킬 없음";
                break;
            case PassiveSkill.허약:
                description = "이 동물은 선천적으로 허약해 스테미나가 낮습니다.";
                break;
            case PassiveSkill.난딴돈의반만가져가:
                description = "상승장일 경우 발동하여 수익의 절반만 가져갑니다.";
                break;
            case PassiveSkill.관망:
                description = "주식을 팔지 않고 한턴 더 지켜봅니다.";
                break;
        }
        return description;
    }

    public List<TraderInfo> TwoInfoGenerate() //두 명의 트레이더 정보를 리스트에 담아 반환
    {
        tr2 = new List<TraderInfo>();
        tr2.Add(generateTraders());
        tr2.Add(generateTraders());
        return tr2;
    }

    public void HireTheTraderList()
    {
        for (int i = 0; i < 2; i++)
        {
            if (tr2[i].flag == TraderFlag.HIRE)
            {
                traderList.Add(tr2[i]);
            }
        }
    }

    public void FireTheTraderList()
    {
        for (int i = 0; i < traderList.Count; i++)
        {
            if (traderList[i].flag == TraderFlag.FIRE)
            {
                traderList.RemoveAt(i);
            }
        }
    }
   
}
