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
        // 기존 화면 창 치우기
        panel.GetComponent<RectTransform>().DOAnchorPosX(-1920f, 1.0f)
            .OnComplete(() =>
            {
                // 캔버스 비활성화 및 원래 위치로
                canvas.SetActive(false);
                panel.GetComponent<RectTransform>().DOAnchorPosX(0f, 0f);
            });
        // 새로운 화면 띄우기
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

    // "다음으로 넘어가기" 버튼 이벤트 핸들러
    public void NextSceneButton()
    {
        gameManager.ManageTurn();
    }

    // "트레이더 선택" 버튼 생성 함수
    public void TraderSelectButtonGenerator(List<TraderInfo> traderList)
    {
        for (int i = 0; i < traderList.Count; i++)
        {
            int itemIndex = i;
            string itemName = traderList[i].traderName;

            // 버튼 생성
            GameObject newButton = Instantiate(buttonPrefab, traderSelectParent);
            newButton.name = "Button_" + itemIndex;

            // 버튼 텍스트 설정
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = itemName;

            Button button = newButton.GetComponent<Button>();

            // 버튼 이벤트 핸들러 등록
            button.onClick.AddListener(() =>
            {
                OnTraderSelectButtonClick(itemIndex, itemName, traderList);
            });
        }
    }

    // "트레이더 고용" 버튼 생성 함수
    public void TraderHireButtonGenerator(List<TraderInfo> traderList)
    {
        for (int i = 0; i < traderList.Count; i++)
        {
            int itemIndex = i;
            string itemName = traderList[i].traderName;

            // 버튼 생성
            GameObject newButton = Instantiate(buttonPrefab, traderHireParent);
            newButton.name = "Button_" + itemIndex;

            // 버튼 텍스트 설정
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = itemName;

            Button button = newButton.GetComponent<Button>();

            // 버튼 이벤트 핸들러 등록
            button.onClick.AddListener(() =>
            {
                OnTraderHireButtonClick(itemIndex, itemName, traderList);
            });
        }
    }

    // "트레이더 선택" 버튼 이벤트 핸들러
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
        // 이미 최대를 고른 상태에서 새로 선택은 불가
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

    // "트레이더 고용" 버튼 이벤트 핸들러
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
