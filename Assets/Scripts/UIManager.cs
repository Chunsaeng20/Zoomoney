using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    #region Variables

    public GameObject PlayCanvas;
    public GameObject StartCanvas;
    public GameObject InformationCanvas;
    public GameObject TraderSelectCanvas;
    public GameObject ResultCanvas;
    public GameObject TraderHireCanvas;
    public GameObject LoadingCanvas;

    public GameObject PlayPanel;
    public GameObject StartPanel;
    public GameObject InformationPanel;
    public GameObject TraderSelectPanel;
    public GameObject ResultPanel;
    public GameObject TraderHirePanel;
    public GameObject LoadingPanel;

    public GameManager gameManager;

    public GameObject buttonPrefab;
    public Transform traderSelectParent;
    public Transform traderHireParent;

    #endregion

    #region User Methods

    public void ChangeScene(GameObject canvas, GameObject panel, GameObject newCanvas)
    {
        // ���� ȭ�� â ġ���
        panel.GetComponent<RectTransform>().DOAnchorPosX(-1920f, 1.0f)
            .OnComplete(() =>
            {
                // ĵ���� ��Ȱ��ȭ �� ���� ��ġ��
                canvas.SetActive(false);
                panel.GetComponent<RectTransform>().DOAnchorPosX(0f, 0f);
            });
        // ���ο� ȭ�� ����
        newCanvas.SetActive(true);
    }

    public void LoadCanvas(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.INIT:
                break;
            case GameState.BEGINTURN:
                ChangeScene(StartCanvas, StartPanel, InformationCanvas);
                break;
            case GameState.SELECTINFORMATION:
                ChangeScene(InformationCanvas, InformationPanel, TraderSelectCanvas);
                break;
            case GameState.SELECTTRADER:
                ChangeScene(TraderSelectCanvas, TraderSelectPanel, PlayCanvas);
                break;
            case GameState.PLAYTURN:
                ChangeScene(PlayCanvas, PlayPanel, ResultCanvas);
                break;
            case GameState.RESULT:
                ChangeScene(ResultCanvas, ResultPanel, TraderHireCanvas);
                break;
            case GameState.HIRETRADER:
                ChangeScene(TraderHireCanvas, TraderHirePanel, InformationCanvas);
                break;
            case GameState.FINISH:
                ChangeScene(ResultCanvas, ResultPanel, StartCanvas);
                break;
        }
    }

    // "�������� �Ѿ��" ��ư �̺�Ʈ �ڵ鷯
    public void NextSceneButton()
    {
        gameManager.ManageTurn();
    }

    // "Ʈ���̴� ����" ��ư ���� �Լ�
    public void TraderSelectButtonGenerator(List<TraderInfo> traderList)
    {
        for (int i = 0; i < traderList.Count; i++)
        {
            int itemIndex = i;
            string itemName = traderList[i].traderName;

            // ��ư ����
            GameObject newButton = Instantiate(buttonPrefab, traderSelectParent);
            newButton.name = "Button_" + itemIndex;

            // ��ư �ؽ�Ʈ ����
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = itemName;

            Button button = newButton.GetComponent<Button>();

            // ��ư �̺�Ʈ �ڵ鷯 ���
            button.onClick.AddListener(() =>
            {
                OnTraderSelectButtonClick(itemIndex, itemName, traderList);
            });
        }
    }

    // "Ʈ���̴� ���" ��ư ���� �Լ�
    public void TraderHireButtonGenerator(List<TraderInfo> traderList)
    {
        for (int i = 0; i < traderList.Count; i++)
        {
            int itemIndex = i;
            string itemName = traderList[i].traderName;

            // ��ư ����
            GameObject newButton = Instantiate(buttonPrefab, traderHireParent);
            newButton.name = "Button_" + itemIndex;

            // ��ư �ؽ�Ʈ ����
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = itemName;

            Button button = newButton.GetComponent<Button>();

            // ��ư �̺�Ʈ �ڵ鷯 ���
            button.onClick.AddListener(() =>
            {
                OnTraderHireButtonClick(itemIndex, itemName, traderList);
            });
        }
    }

    // "Ʈ���̴� ����" ��ư �̺�Ʈ �ڵ鷯
    public void OnTraderSelectButtonClick(int index, string name, List<TraderInfo> traderList)
    {
        int participateNumber = 0;
        bool alreadySelected = false;
        foreach (TraderInfo trader in traderList)
        {
            if (trader.isParticipate)
            {
                participateNumber++;
            }

            if (name == trader.traderName)
            {
                alreadySelected = true;
            }
        }
        // �̹� �ִ븦 �� ���¿��� ���� ������ �Ұ�
        if (participateNumber > gameManager.MaxTraderSelect && alreadySelected == false) return;

        foreach (TraderInfo trader in traderList)
        {
            if(name == trader.traderName)
            {
                trader.isParticipate = !trader.isParticipate;
                break;
            }
        }
    }

    // "Ʈ���̴� ���" ��ư �̺�Ʈ �ڵ鷯
    public void OnTraderHireButtonClick(int index, string name, List<TraderInfo> traderList)
    {
        bool alreadyHired = false;
        foreach(TraderInfo trader in traderList)
        {
            if(name == trader.traderName)
            {
                alreadyHired = trader.isHire;
                if(gameManager.trader.traderList.Count == gameManager.MaxTraderNumber
                    && alreadyHired == false)
                {
                    return;
                }
                trader.isHire = !trader.isHire;
            }
        }
    }

    #endregion

    #region Unity Methods

    public void Start()
    {
        StartCanvas.SetActive(true);
        PlayCanvas.SetActive(false);
        InformationCanvas.SetActive(false);
        TraderSelectCanvas.SetActive(false);
        ResultCanvas.SetActive(false);
        TraderHireCanvas.SetActive(false);
        LoadingCanvas.SetActive(false);
    }

    #endregion

}
