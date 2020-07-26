using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuManager : MonoBehaviour
{
    private bool isActive = false;
    public bool realtime = false;
    public Animator animator;
    public GameObject panel;
    public Text buttonText;
    public mqttManager mqtt;
    public Toggle mqttConnectedToggle;
    public Text mqttConnectedLabel;
    public Text mqttLatestMessageText;
    private Button factoryStateButton;
    private Toggle factoryStateToggle;
    private Toggle cameraSettingsToggle;
    private Toggle realtimeUpdateToggle;
    public Toggle recordMovementToggle;
    private InputField brokerInput;
    private CameraSettingsManager cameraSettingsManager;
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        panel = gameObject.transform.Find("Settings Panel").gameObject;
        buttonText = gameObject.transform.Find("Settings Button").transform.Find("ButtonText").GetComponent<Text>();
        mqtt = GameObject.Find("MQTT Manager").GetComponent<mqttManager>();
        mqttConnectedToggle = GameObject.Find("MQTT State Togle").GetComponent<Toggle>();
        mqttConnectedLabel = mqttConnectedToggle.GetComponentInChildren<Text>();
        mqttLatestMessageText = panel.transform.Find("Latest MQTT Message Text").GetComponent<Text>();
        factoryStateToggle = GameObject.Find("Factory State Toggle").GetComponent<Toggle>();
        cameraSettingsToggle = GameObject.Find("Camera Settings Toggle").GetComponent<Toggle>();
        realtimeUpdateToggle = GameObject.Find("Realtime Location Update Toggle").GetComponent<Toggle>();
        recordMovementToggle = GameObject.Find("Record Movement Toggle").GetComponent<Toggle>();
        cameraSettingsManager = GameObject.Find("Camera Location Panel").GetComponent<CameraSettingsManager>();
        factoryStateButton = GameObject.Find("Factory state Button").GetComponent<Button>();
        factoryStateButton.transform.gameObject.SetActive(false);
        brokerInput = GameObject.Find("Broker InputField").GetComponent<InputField>();
        brokerInput.text = PlayerPrefs.GetString("brokerIP", "Enter ip..");

        factoryStateToggle.onValueChanged.AddListener(delegate
        {
            ToggleFactoryState(factoryStateToggle);
        });

        mqttConnectedToggle.onValueChanged.AddListener(delegate
        {
            ToggleValueChanged(mqttConnectedToggle); 
        });

        cameraSettingsToggle.onValueChanged.AddListener(delegate
            {
                ToggleCameraSettings(cameraSettingsToggle);
            });

        realtimeUpdateToggle.onValueChanged.AddListener(delegate
        {

            realtime = realtimeUpdateToggle.isOn;
        });
    }

    void ToggleCameraSettings(Toggle change)
    {
        cameraSettingsManager.ToggleScreen(change.isOn);
    }

    public void enableRealtimeLocationUpdates()
    {
        realtime = true;
    }
    public void onSaveButtonPressed()
    {
        PlayerPrefs.SetString("brokerIP", brokerInput.text);
    }

    void ToggleFactoryState(Toggle change)
    {
        if(change.isOn)
        {
            factoryStateButton.transform.gameObject.SetActive(true);
        }
        else
        {
            factoryStateButton.transform.gameObject.SetActive(false);
        }
    }
    void ToggleValueChanged(Toggle change)
    {
        if (change.isOn) { 
            if(!mqtt.isConnected) { mqtt.Connect(); }
            mqttConnectedLabel.text = "Connected";
        }
        else {
            mqtt.Disconnect();
            mqttConnectedLabel.text = "Disconnected"; 
        }
    }
    public void onButtonPress() {
        isActive = !isActive;

        animator.SetBool("isActive", isActive);

        if(isActive) 
        {
            buttonText.text = "Exit";

        }
        else 
        {
            buttonText.text = "Settings";
        }
    }

    // Update is called once per frame
    void Update()
    {
        //mqttConnectedToggle.isOn = mqtt.isConnected;
        mqttLatestMessageText.text = mqtt.state;

    }
}
