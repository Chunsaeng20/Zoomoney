using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScenarioManager : MonoBehaviour
{
    #region JsonParse
    [Serializable]
    public class Script
    {
        public string Speaker;
        public string Dialog;
    }
    [Serializable]
    public class Scenario
    {
        public int SceneNumber;
        public int ScriptSize;
        public Script[] Scripts;
    }
    [Serializable]
    public class ScenarioData
    {
        public Scenario[] Scenario;
    }
    #endregion
    #region Variables
    public int CurScenarioNumber;
    public int CurScriptNumber;
    public GameObject ScenarioCanvas;
    public GameObject ScenarioPopup;
    public GameObject ScenarioImage;
    public GameObject ScriptBackgroundImage;
    public GameObject ScriptText;
    public GameObject SpeakerName;

    public ScenarioData scenarioData;

    bool is_open = false;
    #endregion
    #region Methods
    public void ScenarioRead()
    {
        TextAsset scenario = Resources.Load<TextAsset>("Json/ScenarioData");
        ScenarioData data = JsonUtility.FromJson<ScenarioData>(scenario.text);
        scenarioData = data;
    }
    public void Play(int sceneNumber)
    {
        Open();
        //발화자 이미지 설정 필요
        //시나리오 이미지 설정 필요
        ScriptText.GetComponent<TextMeshProUGUI>().text = scenarioData.Scenario[sceneNumber].Scripts[0].Dialog;
        CurScenarioNumber = sceneNumber;
        CurScriptNumber++;

    }
    public void Open()
    {
        ScenarioCanvas.SetActive(true);
        is_open = true;
    }
    public void Next()
    {
        if (CurScriptNumber == scenarioData.Scenario[CurScenarioNumber].ScriptSize)
        {
            Close();
            return;
        }
        //발화자 이미지 설정 필요
        //시나리오 이미지 설정 필요
        ScriptText.GetComponent<TextMeshProUGUI>().text = scenarioData.Scenario[CurScenarioNumber].Scripts[CurScriptNumber].Dialog;
        SpeakerName.GetComponent<TextMeshProUGUI>().text = scenarioData.Scenario[CurScenarioNumber].Scripts[CurScriptNumber].Speaker;
        CurScriptNumber++;
    }
    public void Close()
    {
        ScenarioCanvas.SetActive(false);
        is_open = false;
    }
    #endregion
    private void Start()
    {
        ScenarioRead();
        Play(0);
    }
    private void Update()
    {
        if (is_open)
        {
            if(Input.GetKeyDown(KeyCode.Space)||Input.GetMouseButtonDown(0))
            {
                Next();
            }
        }
    }
}
