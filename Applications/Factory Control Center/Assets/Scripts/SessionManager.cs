using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using System.Linq;
using System;

public class SessionManager : MonoBehaviour
{

    private mqttManager mqtt;
    private GameObject errorPanel;
    private GameObject settingsPanel;

    private GameObject introductionPanels;
    private GameObject welcomePanel;
    private GameObject demographic1Panel;
    private GameObject demographic2Panel;
    private GameObject exitPanel;

    private questionnaireManager questionnaireManager;

    private ToggleGroup[] toggleGroups;

    private Button restartButton;
    private TMP_Text timerText;
    private TMP_Text componentText;
    private TMP_Text failureCodeText;
    private TMP_Text instructionText;

    public int failureCounter = 0;
    public float timer = 0;
    public int currentFailure = 0;
    private bool failureState = false;
    private bool latestValue = false;

    private TMP_Dropdown groupDropDown;
    private TMP_Dropdown runDropDown;

    public bool firstRun = true;
    public bool errorFixed = false;
    public bool failure4Stop = true;

    public durationData durationData;


    // Start is called before the first frame update
    void Start()
    {
        mqtt = GameObject.Find("MQTT Manager").GetComponent<mqttManager>();
        questionnaireManager = GameObject.Find("Questionnaire Panels").GetComponent<questionnaireManager>();
        errorPanel = GameObject.Find("Error Panel").gameObject;
        settingsPanel = GameObject.Find("Settings Panel").gameObject;
        welcomePanel = GameObject.Find("Welcome Panel").gameObject;
        demographic1Panel = GameObject.Find("Demographic Panel").gameObject;
        demographic2Panel = GameObject.Find("Demographic Panel 2").gameObject;
        exitPanel = GameObject.Find("Exit Panel").gameObject;
        restartButton = errorPanel.transform.Find("Restart Factory").GetComponent<Button>();
        timerText = settingsPanel.transform.Find("Timer").GetComponent<TMP_Text>();
        componentText = errorPanel.transform.Find("Component Field").GetComponent<TMP_Text>();
        failureCodeText = errorPanel.transform.Find("Failure Field").GetComponent<TMP_Text>();
        instructionText = errorPanel.transform.Find("Failure Instruction Field").GetComponent<TMP_Text>();
        errorPanel.SetActive(false);
        exitPanel.SetActive(false);
        restartButton.interactable = false;
        groupDropDown = settingsPanel.transform.Find("Participant Group Value").GetComponent<TMP_Dropdown>();
        runDropDown = settingsPanel.transform.Find("Participant Run Value").GetComponent<TMP_Dropdown>();

        introductionPanels = GameObject.Find("Introduction Panels").gameObject;
        toggleGroups = introductionPanels.GetComponentsInChildren<ToggleGroup>();


    }


    public void dismissWelcomePanel()
    {
        welcomePanel.GetComponent<Animator>().SetTrigger("DismissPanel");
    }

    public void dismissDemographicPanel1()
    {
        demographic1Panel.GetComponent<Animator>().SetTrigger("DismissPanel");
    }

    public int getParticipantID()
    {
        int participantID = 1;

        var info = new DirectoryInfo(Application.persistentDataPath);
        var folders = info.GetDirectories();
        
        if (folders.Length < 1)
        {
            return 1;
        }
        else
        {
            foreach (DirectoryInfo folder in folders)
            {
                int folderID = int.Parse(folder.Name.Split('-')[1]);
                if (folderID > participantID)
                {
                    participantID = folderID;
                }
            }

            participantID += 1;
        }
            return participantID;

    }

        private void createFolder(int participantID)
        {
        var info = new DirectoryInfo(Application.persistentDataPath);
        info.CreateSubdirectory("participant-" + participantID);


    }

    public void startQuestionnaire()
    {
        questionnaireManager.slideInIntroPanel();
    }

    public void saveDemographicData()
    {
        demographicData demoData = new demographicData();

        Type myType = typeof(demographicData);
        int toggleCount = 1;
        foreach (ToggleGroup group in toggleGroups)
        {
            string question = group.transform.name.Split(' ')[1];
            Debug.Log(question);
            if (toggleCount > 3)
            {
                string answer = group.ActiveToggles().First().gameObject.GetComponentInChildren<TMP_Text>().text;
                Debug.Log(answer);
                FieldInfo fieldInfo = myType.GetField(question);

                fieldInfo.SetValue(demoData, answer);
                
            }
            else
            {
                int answer = int.Parse(group.ActiveToggles().First().name.Split(' ')[1]);
                FieldInfo fieldInfo = myType.GetField(question);

                fieldInfo.SetValue(demoData, answer);
                toggleCount += 1;
            }

           
            
        }

        demoData.participantID = getParticipantID();
        demoData.timestamp = DateTime.Now.ToString();

        demographic2Panel.GetComponent<Animator>().SetTrigger("DismissPanel");

        durationData.timestamp = DateTime.Now.ToString();



        string data = JsonUtility.ToJson(demoData, true);
        createFolder(demoData.participantID);
        File.WriteAllText(Application.persistentDataPath + "/participant-" + demoData.participantID + "/demographic.json", data);

    }


    public void saveQuestionnaireData(questionnaireData questionnaireData)
    {
        if(failureCounter > 5)
        {
            exitPanel.SetActive(true);
        }
        questionnaireData.participantID = int.Parse(GameObject.Find("Current Participant ID").GetComponent<TMP_Text>().text);
        questionnaireData.group = groupDropDown.options[groupDropDown.value].text.Split(' ')[1];
        questionnaireData.run = runDropDown.options[runDropDown.value].text;

        runDropDown.value = 1 - runDropDown.value;
      
        string data = JsonUtility.ToJson(questionnaireData, true);
        File.WriteAllText(Application.persistentDataPath + "/participant-" + questionnaireData.participantID + "/questionnaire-" + questionnaireData.group +  "-"+ questionnaireData.run +  ".json", data);
    }

    public void saveFailureDuration(int failureNumber, float duration)
    {
        failureRun failureRun = new failureRun();
        failureRun.failureID = failureNumber;
        failureRun.duration = duration;
        failureRun.failureCounter = failureCounter + 1;
        failureRun.run = runDropDown.options[runDropDown.value].text;

        durationData.failureRuns.Add(failureRun);

        if (failureCounter == 5)
        {
            durationData.participantID = int.Parse(GameObject.Find("Current Participant ID").GetComponent<TMP_Text>().text);
            string data = JsonUtility.ToJson(durationData, true);
            File.WriteAllText(Application.persistentDataPath + "/participant-" + durationData.participantID + "/durations.json", data);
        }

    }


    public void activateErrorPopup(string component, string failureCode, string instruction)
    {
        componentText.text = component;
        failureCodeText.text = failureCode;
        instructionText.text = instruction;
        
        if (firstRun) {
            StartCoroutine(EnableButtonAfterTime(5));
            firstRun = false;
        }

        errorPanel.SetActive(true);
    }

    public void deactivateErrorPopup()
    {
        firstRun = true;
        componentText.text = "Test";
        errorPanel.SetActive(false);
        restartButton.interactable = false;
    }

     IEnumerator ExecuteAfterTime(float time)
 {
     yield return new WaitForSeconds(time);
 
     // Code to execute after the delay
     errorFixed = false;
    }

    IEnumerator StopFactoryAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        // Code to execute after the delay
        mqtt.stopFactory();
    }

    IEnumerator EnableButtonAfterTime(float time)
 {
     yield return new WaitForSeconds(time);
 
     // Code to execute after the delay
     restartButton.interactable = true;
    }

    public void stopTimerAndRestartFactory()
    {
        errorFixed = true;
        StartCoroutine(ExecuteAfterTime(2));
        deactivateErrorPopup();

        Debug.Log(timer);

        mqtt.sendFailureState(currentFailure, false);

        mqtt.sendDuration(currentFailure, timer);
        saveFailureDuration(currentFailure, timer);

        timer = 0;

        if(mqtt.afterSagen) {mqtt.afterSagen = false;}
        failure4Stop = true;

        mqtt.removeFactoryError();

        mqtt.startFactory();
        mqtt.startFactory();
        latestValue = false;
        failureCounter += 1;

        if(failureCounter % 3 == 0)
        {
            
            if (failureCounter == 3) {
                mqtt.stopFactory();
                questionnaireManager.slideInIntroPanel();
            }
            if(failureCounter == 6) {
                mqtt.stopFactory();
                questionnaireManager.slideInSecondRunPanel();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!errorFixed)
        {
        if(mqtt.failure1 )
        {   
            currentFailure = 1;
            activateErrorPopup("High-bay Warehouse", "F1 - Reach motor failure",  @"Please investigate the component and solve the isue.

Afterwards, restart the factory with the button below.");
        }
        else if (mqtt.failure2)
        {
            currentFailure = 2;

            activateErrorPopup("High-bay Warehouse", "F2 - Horizontal motor failure", @"Please investigate the component and solve the isue.

Afterwards, restart the factory with the button below.");
        }
        else if (mqtt.failure3)
        {
            currentFailure = 3;

            activateErrorPopup("High -bay Warehouse","F3 - Conveyer belt failure", @"Please investigate the component and solve the isue.

Afterwards, restart the factory with the button below.");
        }

        else if (mqtt.failure4) 
        {
            currentFailure = 4;
            activateErrorPopup("Processing Line", "F4 - Saw motor failure", @"Please investigate the component and solve the isue.

Afterwards, restart the factory with the button below.");
        }

        else if (mqtt.failure5)
        {
            currentFailure = 5;

            activateErrorPopup("Processing Line", "F5 - Pneumatic pressure failure", @"Please investigate the component and solve the isue.

Afterwards, restart the factory with the button below.");
        }

        else if(mqtt.failure6)
            {
                currentFailure = 6;
                activateErrorPopup("Quality Check Line", "F6 - Conveyer belt failure", @"Please investigate the component and solve the isue.

Afterwards, restart the factory with the button below.");
            }


        else if (mqtt.failure7)
            {
                currentFailure = 7;
                activateErrorPopup("Quality Check Line", "F7 - Pneumatic pressure failure", @"Please investigate the component and solve the isue.

Afterwards, restart the factory with the button below.");
            }


        else if (mqtt.failure8)
            {
                currentFailure = 8;
                activateErrorPopup("Quality Check Line", "F8 - Color reading failure", @"Please investigate the component and solve the isue.

Afterwards, restart the factory with the button below.");
            }


        else if (mqtt.failure9)
            {
                currentFailure = 9;
                activateErrorPopup("Processing Line", "F9 - Oven melting failure", @"Please investigate the component and solve the isue.

Afterwards, restart the factory with the button below.");
            }

        }
        if (errorPanel.activeSelf && !errorFixed)
        {
            timer += Time.deltaTime;
            timerText.text = timer.ToString();
        }


    }

    public  void doExitGame() {
     Application.Quit();
 }
}
