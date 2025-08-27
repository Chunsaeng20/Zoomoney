using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    NONE = 0,
    INIT = 1,               // �ʱ� ���� ȭ��
    BEGINTURN = 2,          // �� ���� �� �غ�
    SELECTINFORMATION = 3,  // ���� ī�� ���� �ܰ�
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
                // �ʱ� ȭ�� â �ε��ϱ� -> GM
                UIManager.LoadCanvas(gameState);
                gameState = GameState.NONE;
                break;
            case GameState.BEGINTURN:
                // ���ο� ���� �����ϱ� <- Information Ŭ����
                information.GetRandomInformation();

                // ���� ���� â �ε��ϱ� -> Information ���� �ʿ�

                // ���� ���� ����
                gameState = GameState.SELECTINFORMATION;
                break;
            case GameState.SELECTINFORMATION:
                // Ʈ���̴� ���� â �ε��ϱ� -> Trader ���� �ʿ�

                // ���� ���� ����
                gameState = GameState.SELECTTRADER;
                break;
            case GameState.SELECTTRADER:
                // ������ Ʈ���̴� �����ϱ� -> GM
                // �÷��׷� ����

                // ���� �� ��� ����ϱ� -> GM

                // �÷��� ȭ�� â �ε��ϱ� -> GM

                // ���� ���� ����
                gameState = GameState.PLAYTURN;
                break;
            case GameState.PLAYTURN:
                // ���� ���� ���� ����ϱ� -> GM
                IsGameOver();

                // ��� ȭ�� â �ε��ϱ� -> GM

                // ���� ���� ����
                gameState = GameState.RESULT;
                break;
            case GameState.RESULT:
                // ���� ���� ���� Ȯ���ϱ� -> GM
                if(isGameOver)
                {
                    // ���� ���� â �ε��ϱ� -> GM
                    gameState = GameState.FINISH;
                    break;
                }

                // ���ο� Ʈ���̴� ����Ʈ �����ϱ� -> ����Ʈ ��ȯ

                // Ʈ���̴� ��� â �ε��ϱ� -> GM

                // ���� ���� ����
                gameState = GameState.HIRETRADER;
                break;
            case GameState.HIRETRADER:
                // 1. ���ο� Ʈ���̴� ����Ʈ �ޱ�
                // ����� Ʈ���̴� �����ϱ� -> GM

                // 2. ���� Ʈ���̴� ����Ʈ �ޱ�
                // �ذ��� Ʈ���̴� �����ϱ� -> GM

                // ���� �� �ε� â �ε��ϱ� -> GM

                // ���� ���� ����
                gameState = GameState.BEGINTURN;
                break;
            case GameState.FINISH:
                // ���� ȭ�� â �ε��ϱ� -> GM

                // ���� ���� ����
                gameState = GameState.INIT;
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

                currentTurnPlayerProfit += currentTurnProfit;
            }
        }
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

    public void Update()
    {
        ManageTurn();
    }

    #endregion
}
