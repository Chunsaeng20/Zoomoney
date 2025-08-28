using UnityEngine;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    // UI 관리를 위한 메서드
    // 1. StartCanvas
    // 화면 전체 클릭
    // 2. InformaionDeckCanvas
    // (다음으로 넘어가기) 버튼
    // 3. TraderDeckSelectCanvas
    // (다음으로 넘어가기) 버튼
    // 4. PlayTurnCanvas
    // (턴 끝내기) 버튼
    // 5. ResultCanvas
    // 확인된 정보
    // (다음으로 넘어가기) 버튼
    // 6. TraderDeckHireCanvas
    // (다음으로 넘어가기) 버튼
    // 7. LoadingCanvas

    #region Variables

    public GameObject PlayCanvas;
    public GameObject StartCanvas;
    public GameObject InformationCanvas;
    public GameObject TraderSelectCanvas;
    public GameObject ResultCanvas;
    public GameObject TraderHireCanvas;
    public GameObject LoadingCanvas;

    public GameObject StartPanel;

    public GameObject ScenarioCanvas;
    #endregion

    #region User Methods

    public void LoadCanvas(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.INIT:
                // 초기 화면 창 로딩하기 -> GM
                StartPanel.GetComponent<RectTransform>().DOAnchorPosX(-960f, 2f);
                break;
            case GameState.BEGINTURN:
                // 정보 선택 창 로딩하기 -> Information 정보 필요
                break;
            case GameState.SELECTINFORMATION:
                // 트레이더 선택 창 로딩하기 -> Trader 정보 필요
                break;
            case GameState.SELECTTRADER:
                // 플레이 화면 창 로딩하기 -> GM
                break;
            case GameState.PLAYTURN:
                // 결과 화면 창 로딩하기 -> GM
                break;
            case GameState.RESULT:
                // 트레이더 고용 창 로딩하기 -> GM
                break;
            case GameState.HIRETRADER:
                // 다음 턴 로딩 창 로딩하기 -> GM
                break;
            case GameState.FINISH:
                // 메인 화면 창 로딩하기 -> GM
                break;
        }
    }

    #endregion

}
