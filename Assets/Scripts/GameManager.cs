using UnityEngine;

public enum GameState
{
    INIT = 0,               // �ʱ� ���� ȭ��
    BEGINTURN = 1,          // �� ���� �� �غ�
    SELECTINFORMATION = 2,  // ���� ī�� ���� �ܰ�
    SELECTTRADER = 3,       // Ʈ���̴� ī�� ���� �ܰ�
    PLAYTURN = 4,           // �÷��� ȭ��
    RESULT = 5,             // ��� ȭ��
    HIRETRADER = 6,         // Ʈ���̴� ��� �ܰ�
    FINISH = 7,             // ����
    NONE = 8,
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
                Debug.Log("call");
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
        // ���� �ֽ� ����
        float previousStockPrice = 0;

        // ���⼺ <- Information
        float direction = 0;

        // ������ <- Information
        float volatilityRatio = 0;

        // ���� ������
        float priceChange = previousStockPrice * direction * volatilityRatio;

        // ���ο� ����
        float newStockPrice = previousStockPrice + priceChange;

        // ������ 0 ���Ϸ� �������� �ʵ��� ����
        if(newStockPrice < 0)
        {
            newStockPrice = 0;
        }
    }

    // (���� ����) Ʈ���̴��� ���� �� ���� ���
    public void CalculateTraderCurrentTurnProfit()
    {
        float currentTurnProfit = 0;

        // 1. �ŷڵ� ������ -> ���� ������ ���ο� ���� �����ϰ� ������ 2�� �̻� (�ִ� 10��)
        float trust = 20;
        float potential = Random.Range(0f, 100f);
        if(trust >= potential)
        {
            currentTurnProfit = 100;
            return;
        }

        // 2. ���� �о� -> �ֽ� �˻� �� ���� -> �ش� �ֽ��� ���� ���� �� ���ο� ���� ������
        float previousStockPrice = 0;
        float newStockPrice = 0;
        currentTurnProfit = newStockPrice - previousStockPrice;

        // 3. ���� ���� -> 0 ~ 2����
        float invest = 1f;
        currentTurnProfit *= invest;

        // 4. ��ų -> 
        float skill = 1f;
        currentTurnProfit *= skill;
    }

    // (���� ����) �÷��̾��� ���� �� ���� ���
    public void CalculatePlayerCurrentTurnProfit()
    {

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
