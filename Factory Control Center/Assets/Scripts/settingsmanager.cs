using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class settingsmanager : MonoBehaviour
{
    private Animator animator;
    private SessionManager sessionManager;
    private bool isActive = false;
    public GameObject panel;
    public mqttManager mqtt;
    public Toggle mqttConnectedToggle;
    public TMP_Text mqttConnectedLabel;
    public TMP_Text mqttLatestMessageText;
    private TMP_InputField brokerInput;
    private TMP_Text currentParticipantID;
    // Start is called before the first frame update
    void Start()
    {
        animator = transform.GetComponent<Animator>();

        
        panel = gameObject;
        sessionManager = GameObject.Find("Session Manager").GetComponent<SessionManager>();
        mqtt = GameObject.Find("MQTT Manager").GetComponent<mqttManager>();
        mqttConnectedToggle = GameObject.Find("MQTT State Togle").GetComponent<Toggle>();
        mqttConnectedLabel = mqttConnectedToggle.GetComponentInChildren<TMP_Text>();
        mqttLatestMessageText = panel.transform.Find("Latest MQTT Message Text").GetComponent<TMP_Text>();

        brokerInput = GameObject.Find("Broker InputField").GetComponent<TMP_InputField>();
        brokerInput.text = PlayerPrefs.GetString("brokerIP", "Enter ip..");
        currentParticipantID = GameObject.Find("Current Participant ID").GetComponent<TMP_Text>();

        currentParticipantID.text = sessionManager.getParticipantID().ToString();

        mqttConnectedToggle.onValueChanged.AddListener(delegate
        {
            ToggleValueChanged(mqttConnectedToggle); 
        });
    }

        public void onSaveButtonPressed()
    {
        PlayerPrefs.SetString("brokerIP", brokerInput.text);
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

    public void toggleSettings()
    {
        isActive =! isActive;
        animator.SetBool("isActive", isActive);
    }

    // Update is called once per frame
    void Update()
    {
        mqttLatestMessageText.text = mqtt.state;
    }
}
