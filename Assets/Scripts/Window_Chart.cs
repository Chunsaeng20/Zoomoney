using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Window_Chart : MonoBehaviour
{
    [SerializeField] private Sprite circleSprite;
    private RectTransform graphContainer;
    public void Awake()
    {
        graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
        List<float> valueList = new List<float>() {101.3f, 123.3f, 88.1f, 178f};
        ShowGraph(valueList);
    }
    private void CreateDot(Vector2 v, float width, bool isUp)
    {
        GameObject dot = new GameObject("circle", typeof(Image));
        dot.transform.SetParent(graphContainer, false);
        dot.GetComponent<Image>().sprite = circleSprite;
        if (isUp)
        {
            dot.GetComponent<Image>().color = new Color(255/255f, 150/255f, 138/255f, 1);
        }
        else
        {
            dot.GetComponent<Image>().color = new Color(85/255f, 203/255f, 205/255f, 1);
        }   
        RectTransform rect = dot.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(v.x, 0);
        rect.sizeDelta = new Vector2(width, v.y);
        rect.anchorMin = new Vector2(0, 0); 
        rect.anchorMax = new Vector2(0, 0);
        rect.pivot = new Vector2(.5f, 0f);
    }
    public void ShowGraph(List<float> valueList)
    {
        float graphHeight = graphContainer.sizeDelta.y;
        float yMaximum = valueList.Sum() / valueList.Count * 1.5f;
        float xSize = graphContainer.GetComponent<RectTransform>().sizeDelta.x / valueList.Count;
        for (int i = 0; i < valueList.Count; i++)
        {
            bool isup = false;
            float xPosition = (i + 0.5f) * xSize;
            float yPosition = (valueList[i] / yMaximum) * graphHeight;
            if (i >= 1) {
                if (valueList[i] >= valueList[i - 1])
                {
                    isup = true;
                }
            }
            CreateDot(new Vector2(xPosition, yPosition), graphContainer.GetComponent<RectTransform>().sizeDelta.x / (valueList.Count * 2), isup);
        }
    }
}