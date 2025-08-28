using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    NONE = 0,
    INIT = 1,               // �ʱ� ���� ȭ��
    BEGINTURN = 2,          // �� ���� �� �غ�
    SELECTINFORMATION = 3,  // ���� ī�� �ܰ�
    SELECTTRADER = 4,       // Ʈ���̴� ī�� ���� �ܰ�
    PLAYTURN = 5,           // �÷��� ȭ��
    RESULT = 6,             // ��� ȭ��
    HIRETRADER = 7,         // Ʈ���̴� ��� �ܰ�
    FINISH = 8,             // ����
}

public class GameManager : MonoBehaviour
{
    #region Variables

    // ������ ���� Ŭ���� ����
    public Stock stock;
    public Trader trader;
    public PlayerStat playerStat;
    public Information information;
    public int MaxTraderNumber = 12;
    public int MaxTraderSelect = 6;
    List<Info> infoList = new List<Info>();

    // �� ������ ���� ����
    public int currentTurn;         // ���� �� ��
    public bool isGameOver;         // ���� ���� ����
    public GameState gameState;     // ���� ���� ����
    public UIManager UIManager;

    // UI ������ ���� ����

    #endregion

    #region User Methods

    // ��ư�� ������ �� ȣ��Ǵ� �Լ�
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
                // ���ο� ���� �����ϱ�
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
                // ���� ���� ���� ����ϱ�
                IsGameOver();

                UIManager.LoadCanvas(gameState);
                gameState = GameState.RESULT;
                break;
            case GameState.RESULT:
                // ���� ���� ���� Ȯ���ϱ�
                if(isGameOver)
                {
                    // ���� ���� â �ε��ϱ�
                    gameState = GameState.FINISH;
                    UIManager.LoadCanvas(gameState);
                    break;
                }
                UIManager.LoadCanvas(gameState);
                gameState = GameState.HIRETRADER;
                break;
            case GameState.HIRETRADER:
                // ���ο� Ʈ���̴� ����
                List<TraderInfo> newTraderList = trader.TwoInfoGenerate();
                // Ʈ���̴� ���
                trader.enterTheTraderList();
                // Ʈ���̴� �ذ�
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

    // (���� ����) �ֽ��� ���ο� ���� ���
    public void CalculateNewStockPrice()
    {
        // ��� �ֽ� ��ȸ
        foreach (StockInformation stockInformation in stock.stockList)
        {
            // �ֽ� �̸� ��ȸ
            string stockName = stockInformation.ID;

            // ���� �ֽ� ���� ��ȸ
            float previousStockPrice = stockInformation.PreviousStockPrice[stock.stockList.Count - 1];

            // ���� �� �ֽ� ���� ��ȸ
            float direction = 0f;
            float volatility = 0f;
            foreach (Info info in information.allInformation)
            {
                if(info.Corporation == stockName)
                {
                    // ���⼺
                    direction = info.GetDirection();
                    // ������
                    volatility = info.Volatility;
                }
            }

            // ���� ������ ���
            float priceChange = previousStockPrice * direction * volatility;

            // ���ο� ���� ���
            float newStockPrice = previousStockPrice + priceChange;

            // ������ 0 ���Ϸ� �������� �ʵ��� ����
            if(newStockPrice < 0)
            {
                newStockPrice = 0;
            }

            // ���ο� ���� ���� ����
            stockInformation.ReplaceCurrentStockPrice(newStockPrice);
        }
    }

    // (���� ����) �÷��̾�� Ʈ���̴��� ���� �� ���� ���
    public void CalculateCurrentTurnProfit()
    {
        // �÷��̾��� ���� �� ����
        float currentTurnPlayerProfit = 0f;

        // ��� Ʈ���̴� ��ȸ
        foreach(TraderInfo traderInfo in trader.traderList)
        {
            // ���� �Ͽ� ������ Ʈ�����͸� ���� ���
            if(traderInfo.isParticipate)
            {
                // Ʈ���̴��� ü�� ����
                traderInfo.stamina -= 1;

                // Ʈ���̴��� ���� �� ����
                float currentTurnProfit = 0f;

                // ���� �о�
                Sectors sector = traderInfo.sector;
                List<StockInformation> stockBuyList = new List<StockInformation>();
                foreach(StockInformation stockInformation in stock.stockList)
                {
                    // �ش�Ǵ� �ֽ� ���� ����
                    if(sector == stockInformation.Sector)
                    {
                        stockBuyList.Add(stockInformation);
                    }
                }

                // ���� ����
                float invest = traderInfo.trendencyPerMoney;

                // �� �ֽ��� �������� ����
                foreach (StockInformation stockInformation in stockBuyList)
                {
                    // ���� ������ 0�� �������� �� Ȯ���� 0
                    // ���� ������ 2�� �������� �� Ȯ���� 100
                    float exponent = 0f;
                    if(invest < 1.0f)
                    {
                        exponent = 1.0f / (1.0f - invest + 0.001f);
                    }
                    else
                    {
                        exponent = 1.0f - (invest - 1.0f);
                    }

                    // Ȯ���� ���� ���� �ֽ����� ������ ��
                    float buyChance = Mathf.Pow(Random.value, exponent);
                    buyChance *= 100f;
                    if(buyChance >= Random.Range(0f, 100f))
                    {
                        currentTurnProfit += (stockInformation.CurrentStockPrice - stockInformation.PreviousStockPrice[stockInformation.PreviousStockPrice.Count - 1]);
                    }
                }

                // ���� ������ ���Ϳ� ����
                currentTurnProfit *= invest;

                // ��ų ȿ��
                float skillEffect = trader.SkillManagement(traderInfo);
                currentTurnProfit *= skillEffect;

                // �ŷڵ��� ���� Ȯ�������� ���� ����� ����
                float trust = traderInfo.confidence;
                if(trust >= Random.Range(0f, 100f))
                {
                    currentTurnProfit = Mathf.Abs(currentTurnProfit);
                    currentTurnProfit *= Random.Range(2f, 10f);
                }

                // ���� �� �÷��̾� ���Ϳ� ����
                currentTurnPlayerProfit += currentTurnProfit;

                // ���� ���� ����
                traderInfo.profit = currentTurnProfit;
            }
        }

        playerStat.AddCurrentTurnProfitToTotalMoney(currentTurnPlayerProfit);
    }

    // (�� ����) ���� ���� ���� Ȯ��
    public void IsGameOver()
    {
        // �� ����
        currentTurn = currentTurn + 1;

        float currentFund = 1f;

        // 20���� �Ѿ��ų� �ڱ��� 0 ���ϸ� ���� ����
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
