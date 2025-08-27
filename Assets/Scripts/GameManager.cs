using UnityEngine;

public enum GameState
{
    INIT = 0,               // 초기 시작 화면
    BEGINTURN = 1,          // 턴 시작 전 준비
    SELECTINFORMATION = 2,  // 정보 카드 선택 단계
    SELECTTRADER = 3,       // 트레이더 카드 선택 단계
    PLAYTURN = 4,           // 플레이 화면
    RESULT = 5,             // 결과 화면
    HIRETRADER = 6,         // 트레이더 고용 단계
    FINISH = 7,             // 엔딩
    NONE = 8,
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
    public bool isGameOver;         // 게임 오버 여부
    public GameState gameState;     // 현재 게임 상태
    public UIManager UIManager;

    // UI 관리를 위한 변수

    #endregion

    #region User Methods

    // 버튼을 눌렀을 때 호출되는 함수
    public void ManageTurn()
    {
        switch (gameState)
        {
            case GameState.INIT:
                // 초기 화면 창 로딩하기 -> GM
                Debug.Log("call");
                UIManager.LoadCanvas(gameState);
                gameState = GameState.NONE;
                break;
            case GameState.BEGINTURN:
                // 새로운 정보 생성하기 <- Information 클래스
                information.GetRandomInformation();

                // 정보 선택 창 로딩하기 -> Information 정보 필요

                // 게임 상태 변이
                gameState = GameState.SELECTINFORMATION;
                break;
            case GameState.SELECTINFORMATION:
                // 트레이더 선택 창 로딩하기 -> Trader 정보 필요

                // 게임 상태 변이
                gameState = GameState.SELECTTRADER;
                break;
            case GameState.SELECTTRADER:
                // 선택한 트레이더 저장하기 -> GM
                // 플래그로 구분

                // 현재 턴 결과 계산하기 -> GM

                // 플레이 화면 창 로딩하기 -> GM

                // 게임 상태 변이
                gameState = GameState.PLAYTURN;
                break;
            case GameState.PLAYTURN:
                // 게임 오버 여부 계산하기 -> GM
                IsGameOver();

                // 결과 화면 창 로딩하기 -> GM

                // 게임 상태 변이
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

                // 새로운 트레이더 리스트 생성하기 -> 리스트 반환

                // 트레이더 고용 창 로딩하기 -> GM

                // 게임 상태 변이
                gameState = GameState.HIRETRADER;
                break;
            case GameState.HIRETRADER:
                // 1. 새로운 트레이더 리스트 받기
                // 고용한 트레이더 저장하기 -> GM

                // 2. 기존 트레이더 리스트 받기
                // 해고한 트레이더 삭제하기 -> GM

                // 다음 턴 로딩 창 로딩하기 -> GM

                // 게임 상태 변이
                gameState = GameState.BEGINTURN;
                break;
            case GameState.FINISH:
                // 메인 화면 창 로딩하기 -> GM

                // 게임 상태 변이
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
    public void CalculateTraderCurrentTurnProfit()
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

    // (메인 로직) 플레이어의 현재 턴 수익 계산
    public void CalculatePlayerCurrentTurnProfit()
    {

    }

    // (턴 로직) 게임 오버 여부 확인
    public void IsGameOver()
    {
        // 턴 증가
        currentTurn = currentTurn + 1;

        float currentFund = 1f;

        // 20턴이 넘었거나 자금이 0 이하면 게임 오버
        if(currentTurn > 20 || currentFund <= 0)
        {
            isGameOver = true;
        }
    }

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

    public void Update()
    {
        ManageTurn();
    }

    #endregion
}
