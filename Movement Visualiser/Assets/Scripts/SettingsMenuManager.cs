using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuManager : MonoBehaviour
{

    public mqtt_test mqtt;
    public Button mqttConnectButton;
    public Button saveBrokerButton;
    public TMP_Text mqttConnectedLabel;
    public TMP_Text mqttLatestMessageText;
    public Toggle realtimeToggle;
    private TMP_InputField brokerInput;
    private Animator playerAnimator;
    public bool realtime = false;
    private bool connected = false;

    // Start is called before the first frame update
    void Start()
    {

        realtimeToggle = GameObject.Find("Realtime Toggle").GetComponent<Toggle>();
        playerAnimator = GameObject.Find("Player Controls Panel").GetComponent<Animator>();

        realtimeToggle.onValueChanged.AddListener(delegate
       { 
       
           if(realtimeToggle.isOn)
           {
               //Disable play controls
               realtime = true;
               playerAnimator.SetBool("isRealtime", realtime);

           }

           else
           {
               //Enable play controls
               realtime = false;
               if (connected) { ToggleConnect(); }
             
               playerAnimator.SetBool("isRealtime", realtime);
           }

       });
    
        mqtt = GameObject.Find("MQTT Manager").GetComponent<mqtt_test>();
        mqttConnectButton = GameObject.Find("MQTT Connect Button").GetComponent<Button>();
        saveBrokerButton = GameObject.Find("Broker Save Button").GetComponent<Button>();
        mqttConnectedLabel = GameObject.Find("MQTT Status Value").GetComponent<TMP_Text>();
        mqttLatestMessageText = GameObject.Find("MQTT Latest Message").GetComponent<TMP_Text>();

        brokerInput = GameObject.Find("Broker InputField").GetComponent<TMP_InputField>();
        brokerInput.text = PlayerPrefs.GetString("brokerIP", "Enter ip..");


    }


    public void ToggleConnect()
    {
        if(!connected)
        {
            mqtt.Connect();
            mqttConnectedLabel.text = "Connected";
            mqttConnectButton.GetComponentInChildren<TMP_Text>().text = "Disconnect";
            connected = true;
        }
        else
        {
            mqtt.Disconnect();
            mqttConnectedLabel.text = "Disconnected";
            mqttConnectButton.GetComponentInChildren<TMP_Text>().text = "Connect";
            connected = false;
        }
    }

    public void onSaveButtonPressed()
    {
        PlayerPrefs.SetString("brokerIP", brokerInput.text);
    }



    // Update is called once per frame
    void Update()
    {
        mqttLatestMessageText.text = mqtt.state;

    }
}
