using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class SessionManager : MonoBehaviour
{
    public InteractionPanelManager interactionPanelManager;
    public CameraSettingsManager cameraSettingsManager;
    public SettingsMenuManager settingsMenuManager;
    public GameObject interactionPanel;
    public mqttManager mqtt;
    public GameObject selectedComponent= null;
    public Button solvedButton;
    public bool startedRecording = false;

    // Start is called before the first frame update
    void Start()
    {
        interactionPanel = GameObject.Find("Interaction panel").gameObject;
        cameraSettingsManager = GameObject.Find("Camera Location Panel").GetComponent<CameraSettingsManager>();
        settingsMenuManager = GameObject.Find("Settings Holder").GetComponent<SettingsMenuManager>();
        interactionPanelManager = interactionPanel.GetComponent<InteractionPanelManager>();
        mqtt = GameObject.Find("MQTT Manager").GetComponent<mqttManager>();
        solvedButton = GameObject.Find("Solved Button").GetComponent<Button>();
        solvedButton.gameObject.SetActive(false);
    }


    public void SelectComponent(GameObject selectedGameObject)
    {
        // Disable reselection and selecting multiple components
        if(selectedComponent == selectedGameObject) { return; }
        if (selectedComponent != null) { return; }

        selectedComponent = selectedGameObject;
        selectedComponent.GetComponent<stateManager>().toggleComponent();

        interactionPanel.transform.Find("Selected Placeholder").GetComponent<Text>().text = selectedComponent.GetComponent<stateManager>().componentName;

        if(selectedComponent.GetComponent<stateManager>().setActive)
        {
            interactionPanel.transform.Find("Instruction Placeholder").GetComponent<Text>().text = selectedComponent.GetComponent<stateManager>().componentInstruction;

            interactionPanel.transform.Find("Failure Placeholder").GetComponent<Text>().text = selectedComponent.GetComponent<stateManager>().failure;
            interactionPanel.transform.Find("Show Instruction Button").gameObject.SetActive(true);
        }

        else
        {
            interactionPanel.transform.Find("Instruction Placeholder").GetComponent<Text>().text = "";
            interactionPanel.transform.Find("Failure Placeholder").GetComponent<Text>().text = "No failure";
            interactionPanel.transform.Find("Show Instruction Button").gameObject.SetActive(false);
        }




        interactionPanelManager.ActivateInteractionPanel();
        Debug.Log(selectedComponent.GetComponent<stateManager>().componentName);
    }

    public void toggleInstruction()
    {
        if (selectedComponent == null) { return; }
        interactionPanelManager.ActivateInstructionState();
        solvedButton.gameObject.SetActive(true);
        selectedComponent.GetComponent<stateManager>().solutionOpen = true;

    }
    public void DeselectComponent()
    {
        interactionPanelManager.DismissInteractionPanel();

        selectedComponent.GetComponent<stateManager>().isSelected = false;
        selectedComponent.GetComponent<stateManager>().solutionOpen = false;

        selectedComponent = null;
        solvedButton.gameObject.SetActive(false);
    }


    public void solvedProblemAndDeselectComponent()
    {
        interactionPanelManager.DismissInteractionPanel();

        if (startedRecording)
        {
            cameraSettingsManager.stopMovementLog();
            startedRecording = false;
        }


        if (mqtt.isConnected)
        {
            mqtt.sendFailureState(selectedComponent.GetComponent<stateManager>().number, false);
        }

        selectedComponent.GetComponent<stateManager>().isSelected = false;
        selectedComponent.GetComponent<stateManager>().solutionOpen = false;

        selectedComponent = null;
        solvedButton.gameObject.SetActive(false);
    }




    // Update is called once per frame
    void Update()
    {
        if (selectedComponent != null)
        {
            interactionPanel.transform.Find("Status Sign").transform.Find("OkSign").gameObject.SetActive(!selectedComponent.GetComponent<stateManager>().setActive);
            interactionPanel.transform.Find("Status Sign").transform.Find("ErrorSign").gameObject.SetActive(selectedComponent.GetComponent<stateManager>().setActive);
        }

        if(settingsMenuManager.recordMovementToggle.isOn && Array.Exists(mqtt.failures, element => element == true))
        {
            if (!startedRecording)
            {
                cameraSettingsManager.startMovementLog(Array.IndexOf(mqtt.failures, true) + 1);
                startedRecording = true;
            }
            
        }



    }
}
