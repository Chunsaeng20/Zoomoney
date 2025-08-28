using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    NONE = 0,
    INIT = 1,               // 초기 시작 화면
    BEGINTURN = 2,          // 턴 시작 전 준비
    SELECTINFORMATION = 3,  // 정보 카드 단계
    SELECTTRADER = 4,       // 트레이더 카드 선택 단계
    PLAYTURN = 5,           // 플레이 화면
    RESULT = 6,             // 결과 화면
    HIRETRADER = 7,         // 트레이더 고용 단계
    FINISH = 8,             // 엔딩
}

public class GameManager : MonoBehaviour
{
    #region Variables

    // 로직을 위한 클래스 변수
    public Stock stock;
    public Trader trader;
    public PlayerStat playerStat;
    public Information information;
    public int MaxTraderNumber = 12;
    public int MaxTraderSelect = 6;
    List<Info> infoList = new List<Info>();

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
                UIManager.LoadCanvas(gameState);
                gameState = GameState.NONE;
                break;
            case GameState.BEGINTURN:
                infoList.Clear();
                // 새로운 정보 생성하기
                infoList = information.GetRandomInformation();

                UIManager.LoadCanvas(gameState);
                gameState = GameState.SELECTINFORMATION;
                break;
            case GameState.SELECTINFORMATION:
                UIManager.LoadCanvas(gameState);
                gameState = GameState.SELECTTRADER;
                break;
            case GameState.SELECTTRADER:
                CalculateNewStockPrice();
                CalculateCurrentTurnProfit();
                
                UIManager.LoadCanvas(gameState);
                gameState = GameState.PLAYTURN;
                break;
            case GameState.PLAYTURN:
                // 게임 오버 여부 계산하기
                IsGameOver();

                UIManager.LoadCanvas(gameState);
                gameState = GameState.RESULT;
                break;
            case GameState.RESULT:
                // 게임 오버 여부 확인하기
                if(isGameOver)
                {
                    // 게임 오버 창 로딩하기
                    gameState = GameState.FINISH;
                    UIManager.LoadCanvas(gameState);
                    break;
                }
                UIManager.LoadCanvas(gameState);
                gameState = GameState.HIRETRADER;
                break;
            case GameState.HIRETRADER:
                // 새로운 트레이더 생성
                List<TraderInfo> newTraderList = trader.TwoInfoGenerate();
                // 트레이더 고용
                trader.enterTheTraderList();
                // 트레이더 해고
                trader.HireTheTraderList();

                UIManager.LoadCanvas(gameState);
                gameState = GameState.BEGINTURN;
                break;
            case GameState.FINISH:
                gameState = GameState.INIT;
                UIManager.LoadCanvas(gameState);
                break;
        }
    }

    // (메인 로직) 주식의 새로운 가격 계산
    public void CalculateNewStockPrice()
    {
        // 모든 주식 순회
        foreach (StockInformation stockInformation in stock.stockList)
        {
            // 주식 이름 조회
            string stockName = stockInformation.ID;

            // 이전 주식 가격 조회
            float previousStockPrice = stockInformation.PreviousStockPrice[stock.stockList.Count - 1];

            // 현재 턴 주식 정보 조회
            float direction = 0f;
            float volatility = 0f;
            foreach (Info info in information.allInformation)
            {
                if(info.Corporation == stockName)
                {
                    // 방향성
                    direction = info.GetDirection();
                    // 변동폭
                    volatility = info.Volatility;
                }
            }

            // 가격 변동량 계산
            float priceChange = previousStockPrice * direction * volatility;

            // 새로운 가격 계산
            float newStockPrice = previousStockPrice + priceChange;

            // 가격이 0 이하로 내려가지 않도록 보정
            if(newStockPrice < 0)
            {
                newStockPrice = 0;
            }

            // 새로운 가격 정보 저장
            stockInformation.ReplaceCurrentStockPrice(newStockPrice);
        }
    }

    // (메인 로직) 플레이어와 트레이더의 현재 턴 수익 계산
    public void CalculateCurrentTurnProfit()
    {
        // 플레이어의 현재 턴 수익
        float currentTurnPlayerProfit = 0f;

        // 모든 트레이더 순회
        foreach(TraderInfo traderInfo in trader.traderList)
        {
            // 현재 턴에 참여한 트레이터만 수익 계산
            if(traderInfo.isParticipate)
            {
                // 트레이더의 체력 감소
                traderInfo.stamina -= 1;

                // 트레이더의 현재 턴 수익
                float currentTurnProfit = 0f;

                // 전문 분야
                Sectors sector = traderInfo.sector;
                List<StockInformation> stockBuyList = new List<StockInformation>();
                foreach(StockInformation stockInformation in stock.stockList)
                {
                    // 해당되는 주식 정보 수집
                    if(sector == stockInformation.Sector)
                    {
                        stockBuyList.Add(stockInformation);
                    }
                }

                // 투자 성향
                float invest = traderInfo.trendencyPerMoney;

                // 각 주식을 살지말지 결정
                foreach (StockInformation stockInformation in stockBuyList)
                {
                    // 투자 성향이 0에 가까울수록 살 확률이 0
                    // 투자 성향이 2에 가까울수록 살 확률이 100
                    float exponent = 0f;
                    if(invest < 1.0f)
                    {
                        exponent = 1.0f / (1.0f - invest + 0.001f);
                    }
                    else
                    {
                        exponent = 1.0f - (invest - 1.0f);
                    }

                    // 확률에 따라 현재 주식으로 수익을 냄
                    float buyChance = Mathf.Pow(Random.value, exponent);
                    buyChance *= 100f;
                    if(buyChance >= Random.Range(0f, 100f))
                    {
                        currentTurnProfit += (stockInformation.CurrentStockPrice - stockInformation.PreviousStockPrice[stockInformation.PreviousStockPrice.Count - 1]);
                    }
                }

                // 투자 성향을 수익에 곱함
                currentTurnProfit *= invest;

                // 스킬 효과
                float skillEffect = trader.SkillManagement(traderInfo);
                currentTurnProfit *= skillEffect;

                // 신뢰도에 따라 확률적으로 수익 대박이 터짐
                float trust = traderInfo.confidence;
                if(trust >= Random.Range(0f, 100f))
                {
                    currentTurnProfit = Mathf.Abs(currentTurnProfit);
                    currentTurnProfit *= Random.Range(2f, 10f);
                }

                // 현재 턴 플레이어 수익에 더함
                currentTurnPlayerProfit += currentTurnProfit;

                // 최종 수익 저장
                traderInfo.profit = currentTurnProfit;
            }
        }

        playerStat.AddCurrentTurnProfitToTotalMoney(currentTurnPlayerProfit);
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

    #endregion
}
