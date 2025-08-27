using UnityEngine;
using DG.Tweening;

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

    public void Button()
    {

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
