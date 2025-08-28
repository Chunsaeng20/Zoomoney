using UnityEngine;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
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
                // �ʱ� ȭ�� â �ε��ϱ� -> GM
                StartPanel.GetComponent<RectTransform>().DOAnchorPosX(-960f, 2f);
                break;
            case GameState.BEGINTURN:
                // ���� ���� â �ε��ϱ� -> Information ���� �ʿ�
                break;
            case GameState.SELECTINFORMATION:
                // Ʈ���̴� ���� â �ε��ϱ� -> Trader ���� �ʿ�
                break;
            case GameState.SELECTTRADER:
                // �÷��� ȭ�� â �ε��ϱ� -> GM
                break;
            case GameState.PLAYTURN:
                // ��� ȭ�� â �ε��ϱ� -> GM
                break;
            case GameState.RESULT:
                // Ʈ���̴� ��� â �ε��ϱ� -> GM
                break;
            case GameState.HIRETRADER:
                // ���� �� �ε� â �ε��ϱ� -> GM
                break;
            case GameState.FINISH:
                // ���� ȭ�� â �ε��ϱ� -> GM
                break;
        }
    }

    #endregion

}
