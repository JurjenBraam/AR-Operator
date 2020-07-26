using UnityEngine;
using System.Collections;
using System.Net;
using System;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using uPLibrary.Networking.M2Mqtt.Utility;
using uPLibrary.Networking.M2Mqtt.Exceptions;
using UnityEngine.UI;

public class mqtt_test : MonoBehaviour
{
	private MqttClient client;
	public bool Enable = false;
	public bool isConnected = false;
	public String state;
	public String mqttBrokerIP = "broker.hivemq.com";
	public int mqttPort	= 1883;
	public CustomMovement latestMovement;

	private InputField brokerInput;

	// Start is called before the first frame update
	void Start()
	{


		if (Enable) { Connect(); }
		
	}

	public void Connect()
    {
		mqttBrokerIP = PlayerPrefs.GetString("brokerIP", mqttBrokerIP);
		// create client instance 
		client = new MqttClient(mqttBrokerIP, mqttPort, false, null);

		// register to message received 
		client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

		string clientId = Guid.NewGuid().ToString();
		byte code = client.Connect(clientId);
		Debug.Log("Mqtt: " + code);


		// subscribe to the topic "/home/temperature" with QoS 2 
		client.Subscribe(new string[] { "factory/ipad/location" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
	}

	public void Disconnect()
    {
		if (client != null)
		{
			client.Disconnect();
		}

	}


	

	void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
		{
			// Debug.Log("Received: " + System.Text.Encoding.UTF8.GetString(e.Message));

			state = System.Text.Encoding.UTF8.GetString(e.Message);

			latestMovement = JsonUtility.FromJson<CustomMovement>(state);

			


		//	for (int i = 0; i < mqttdata.values.length; i++)
		//	{
		//		if (mqttdata.values[i].id == "factory-1.factory-1.cpu 1511c-1 pn.memory.a4 farbsensor") {
		//			colorvalue = int.parse(mqttdata.values[i].v);
		//		}
		//
	}


	// Update is called once per frame
	void Update()
	{
	
		
	}

	private void OnDestroy()
	{
		if (client != null && client.IsConnected) 
		{
			client.Disconnect();
		}
		
	}
}