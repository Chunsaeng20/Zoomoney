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

    // ������ ���� Ŭ���� ����
    public Stock stock;
    public Trader trader;
    public PlayerStat playerStat;
    public Information information;

    // �� ������ ���� ����
    public int currentTurn;         // ���� �� ��
    public float currentFund;       // ���� �ڱ�
    public bool isGameOver;         // ���� ���� ����
    public GameState gameState;     // ���� ���� ����

    // UI ������ ���� ����

    #endregion

    #region User Methods

    public void ManageTurn()
    {
        switch (gameState)
        {
            case GameState.INIT:
                // ���� ȭ�� â �ε��ϱ� -> GM
                break;
            case GameState.BEGINTURN:
                // ���ο� ���� �����ϱ� <- Information Ŭ����
                // ���� ���� â �ε��ϱ� -> Information ���� �ʿ�
                gameState = GameState.SELECTINFORMATION;
                break;
            case GameState.SELECTINFORMATION:
                // ������ ���� �����ϱ� -> GM
                // Ʈ���̴� ���� â �ε��ϱ� -> Trader ���� �ʿ�
                gameState = GameState.SELECTTRADER;
                break;
            case GameState.SELECTTRADER:
                // ������ Ʈ���̴� �����ϱ� -> GM
                // ���� �� ��� ����ϱ� -> GM
                // �÷��� ȭ�� â �ε��ϱ� -> GM
                gameState = GameState.PLAYTURN;
                break;
            case GameState.PLAYTURN:
                // ���� ���� ���� ����ϱ� -> GM
                // ��� ȭ�� â �ε��ϱ� -> GM
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
                // ���ο� Ʈ���̴� �����ϱ� <- Trader Ŭ����
                // Ʈ���̴� ��� â �ε��ϱ� -> GM
                gameState = GameState.HIRETRADER;
                break;
            case GameState.HIRETRADER:
                // ����� Ʈ���̴� �����ϱ� -> GM
                // �ذ��� Ʈ���̴� �����ϱ� -> GM
                // ���� �� �ε� â �ε��ϱ� -> GM
                gameState = GameState.BEGINTURN;
                break;
            case GameState.FINISH:
                // ���� ȭ�� â �ε��ϱ� -> GM
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
    public void CalculateCurrentTurnProfit()
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

    // (�� ����) ���� ���� ���� Ȯ��
    public void IsGameOver()
    {
        // �� ����
        currentTurn = currentTurn + 1;
        // 20���� �Ѿ��ų� �ڱ��� 0���ϸ� ���� ����
        if(currentTurn > 20 || currentFund <= 0)
        {
            isGameOver = true;
        }
    }

    // UI ������ ���� �޼���
    // 1. StartCanvas
    // ȭ�� ��ü Ŭ��
    // 2. InformaionDeckCanvas
    // (�������� �Ѿ��) ��ư
    // 3. TraderDeckSelectCanvas
    // (�������� �Ѿ��) ��ư
    // 4. PlayTurnCanvas
    // (�� ������) ��ư
    // 5. ResultCanvas
    // Ȯ�ε� ����
    // (�������� �Ѿ��) ��ư
    // 6. TraderDeckHireCanvas
    // (�������� �Ѿ��) ��ư
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
