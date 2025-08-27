using UnityEngine;

public enum GameState
{
    INIT = 0,
    BEGINTURN = 1,
    SELECTINFORMATION = 2,
    SELECTTRADER = 3,
    PLAYTURN = 4,
    RESULT = 5,
    HIRETRADER = 6,
    FINISH = 7,
}

public class GameManager : MonoBehaviour
{
    #region Variables

    // 로직을 위한 클래스 변수
    public Stock stock;
    public Trader trader;
    public PlayerStat playerStat;
    public Information information;

    // 턴 관리를 위한 변수
    public int currentTurn;         // 현재 턴 수
    public float currentFund;       // 현재 자금
    public bool isGameOver;         // 게임 오버 여부
    public GameState gameState;     // 현재 게임 상태

    // UI 관리를 위한 변수

    #endregion

    #region User Methods

    public void ManageTurn()
    {
        switch (gameState)
        {
            case GameState.INIT:
                // 메인 화면 창 로딩하기 -> GM
                break;
            case GameState.BEGINTURN:
                // 새로운 정보 생성하기 <- Information 클래스
                // 정보 선택 창 로딩하기 -> Information 정보 필요
                gameState = GameState.SELECTINFORMATION;
                break;
            case GameState.SELECTINFORMATION:
                // 선택한 정보 저장하기 -> GM
                // 트레이더 선택 창 로딩하기 -> Trader 정보 필요
                gameState = GameState.SELECTTRADER;
                break;
            case GameState.SELECTTRADER:
                // 선택한 트레이더 저장하기 -> GM
                // 현재 턴 결과 계산하기 -> GM
                // 플레이 화면 창 로딩하기 -> GM
                gameState = GameState.PLAYTURN;
                break;
            case GameState.PLAYTURN:
                // 게임 오버 여부 계산하기 -> GM
                // 결과 화면 창 로딩하기 -> GM
                gameState = GameState.RESULT;
                break;
            case GameState.RESULT:
                // 게임 오버 여부 확인하기 -> GM
                if(isGameOver)
                {
                    // 게임 오버 창 로딩하기 -> GM
                    gameState = GameState.FINISH;
                    break;
                }
                // 새로운 트레이더 생성하기 <- Trader 클래스
                // 트레이더 고용 창 로딩하기 -> GM
                gameState = GameState.HIRETRADER;
                break;
            case GameState.HIRETRADER:
                // 고용한 트레이더 저장하기 -> GM
                // 해고한 트레이더 삭제하기 -> GM
                // 다음 턴 로딩 창 로딩하기 -> GM
                gameState = GameState.BEGINTURN;
                break;
            case GameState.FINISH:
                // 메인 화면 창 로딩하기 -> GM
                gameState = GameState.INIT;
                break;
        }
    }

    // (메인 로직) 주식의 새로운 가격 계산
    public void CalculateNewStockPrice()
    {
        // 이전 주식 가격
        float previousStockPrice = 0;

        // 방향성 <- Information
        float direction = 0;

        // 변동폭 <- Information
        float volatilityRatio = 0;

        // 가격 변동량
        float priceChange = previousStockPrice * direction * volatilityRatio;

        // 새로운 가격
        float newStockPrice = previousStockPrice + priceChange;

        // 가격이 0 이하로 내려가지 않도록 보정
        if(newStockPrice < 0)
        {
            newStockPrice = 0;
        }
    }

    // (메인 로직) 트레이더의 현재 턴 수익 계산
    public void CalculateCurrentTurnProfit()
    {
        float currentTurnProfit = 0;

        // 1. 신뢰도 돌림판 -> 포텐 터지면 새로운 가격 무시하고 무조건 2배 이상 (최대 10배)
        float trust = 20;
        float potential = Random.Range(0f, 100f);
        if(trust >= potential)
        {
            currentTurnProfit = 100;
            return;
        }

        // 2. 전문 분야 -> 주식 검색 및 선택 -> 해당 주식의 이전 가격 및 새로운 가격 가져옴
        float previousStockPrice = 0;
        float newStockPrice = 0;
        currentTurnProfit = newStockPrice - previousStockPrice;

        // 3. 투자 성향 -> 0 ~ 2배율
        float invest = 1f;
        currentTurnProfit *= invest;

        // 4. 스킬 -> 
        float skill = 1f;
        currentTurnProfit *= skill;
    }

    // (턴 로직) 게임 오버 여부 확인
    public void IsGameOver()
    {
        // 턴 증가
        currentTurn = currentTurn + 1;
        // 20턴이 넘었거나 자금이 0이하면 게임 오버
        if(currentTurn > 20 || currentFund <= 0)
        {
            isGameOver = true;
        }
    }

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

    #endregion

    #region Unity Methods

    public void Start()
    {
        gameState = GameState.INIT;
    }

    public void OnValidate()
    {
        gameState = GameState.INIT;
    }

    #endregion
}
