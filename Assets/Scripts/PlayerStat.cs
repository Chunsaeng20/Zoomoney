using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    public int totalMoney = 0;
    //스킬 1 회식 : 트레이더들 스테미너 증가
    
    public Trader traderManager;
    public void CompanyDinner()
    {
        foreach (TraderInfo trader in traderManager.traderList)
        {
            // 만약 트레이더가 고용(isHire)되었고, 투자에 참여(isParticipate)했다면
            if (trader.isHire && trader.isParticipate)
            {
                // 해당 트레이더의 체력(stamina)을 1 증가시킵니다.
                trader.stamina += 1;

                // 콘솔에 변경된 내용을 출력하여 잘 작동하는지 확인합니다. (선택 사항)
                Debug.Log(trader.traderName + "의 체력이 1 증가했습니다. 현재 체력: " + trader.stamina);
            }
        }
    }
    //스킬 2 : 다음 턴 정보의 질 향상
    
    //스킬 3 : 기도

}
