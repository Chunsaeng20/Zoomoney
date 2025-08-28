using System.Collections;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    public float totalMoney = 100000;
    public int Leverage = 1; // 레버리지 배수

    public Trader traderManager;
    public Information informationManager;
    public GameManager gameManager;

    // 현재 턴의 수익을 총 자본에 더하는 함수
    public void AddCurrentTurnProfitToTotalMoney(float currentTurnPlayerProfit)
    {
        currentTurnPlayerProfit *= Leverage;
        totalMoney += currentTurnPlayerProfit;
    }

    //스킬 1 회식 : 트레이더들 스테미너 증가
    public void CompanyDinner()
    {
        int staminaIncrease = 1;
        foreach (TraderInfo trader in traderManager.traderList)
        {
            // 만약 트레이더가 투자에 참여했다면
            // -> 현재 턴에 내 덱에 있는 트레이더들
            if (trader.flag == TraderFlag.SELECT)
            {
                // 해당 트레이더의 체력(stamina)을 1 증가시킵니다.
                trader.stamina += staminaIncrease;

                // 콘솔에 변경된 내용을 출력하여 잘 작동하는지 확인합니다. (선택 사항)
                Debug.Log(trader.stamina);
            }
        }
    }

    //스킬 2 : 다음 턴 정보의 질 향상 -> 다음 턴 정보는 뉴스만 보여주기
    public void NoRumor()
    {
        informationManager.OnlyNews = true; // 다음 턴 정보는 뉴스만 보여주기
    }

    //스킬 3 : 이번 턴 레버리지 2배, 다음 턴에 다시 1배
    public IEnumerator Layoff()
    {
        Leverage *= 2; // 이번 턴 레버리지 2배
                       //현재 턴
        int currentTurn = gameManager.currentTurn;

        //다음 턴이 올 때 까지 대기
        yield return new WaitUntil(() => gameManager.currentTurn == currentTurn + 1);
        Leverage /= 2; // 다음 턴에 다시 1배
    }

}
