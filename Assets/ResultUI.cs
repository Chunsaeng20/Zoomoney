using UnityEngine;
using UnityEngine.UI;

public class ResultUI : MonoBehaviour
{
    private GameObject TotalResult;
    private GameObject TraderResult;
    private GameObject ZoospiResult;
    private GameObject SectorResult;
    [SerializeField] private Sprite TotalWindow;
    [SerializeField] private Sprite TraderWindow;
    [SerializeField] private Sprite ZoospiWindow;
    [SerializeField] private Sprite SectorWindow;
    public void Awake()
    {
        TotalResult = transform.Find("TotalResult").gameObject;
        TraderResult = transform.Find("TraderResult").gameObject;
        ZoospiResult = transform.Find("ZoospiResult").gameObject;
        SectorResult = transform.Find("SectorResult").gameObject;
    }
    public void OpenTotalWindow()
    {
        gameObject.GetComponent<Image>().sprite = TotalWindow;
        TotalResult.SetActive(true);
        TraderResult.SetActive(false);
        ZoospiResult.SetActive(false);
        SectorResult.SetActive(false);
    }
    public void OpenTraderWindow()
    {
        gameObject.GetComponent<Image>().sprite = TraderWindow;
        TotalResult.SetActive(false);
        TraderResult.SetActive(true);
        ZoospiResult.SetActive(false);
        SectorResult.SetActive(false);
    }
    public void OpenZoospiWindow()
    {
        gameObject.GetComponent<Image>().sprite = ZoospiWindow;
        TotalResult.SetActive(false);
        TraderResult.SetActive(false);
        ZoospiResult.SetActive(true);
        SectorResult.SetActive(false);
    }
    public void OpenSectorWindow()
    {
        gameObject.GetComponent<Image>().sprite = SectorWindow;
        TotalResult.SetActive(false);
        TraderResult.SetActive(false);
        ZoospiResult.SetActive(false);
        SectorResult.SetActive(true);
    }
}
