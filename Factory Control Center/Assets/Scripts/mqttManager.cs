using UnityEngine;
using System.Collections;
using System.Net;
using System;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using uPLibrary.Networking.M2Mqtt.Utility;
using uPLibrary.Networking.M2Mqtt.Exceptions;
using UnityEngine.UI;

public class mqttManager : MonoBehaviour
{
	private MqttClient client;
	private SessionManager sessionManager;
	public bool Enable = false;
	public bool isConnected = false;
	public String state;
	public String mqttBrokerIP = "broker.hivemq.com";
	public int mqttPort	= 1883;
	public int colorValue = 0;
	public bool startPosition = false;
	public bool failure = false;
	public bool running = false;
	public bool left = false;
	public bool right = false;
	public bool up = false;
	public bool down = false;
	public bool extend = false;
	public bool retract = false;
	public bool retract1 = false;
	public bool sagen = false;
	public bool afterSagen = false;
	public bool failure1 = false;
	public bool failure2 = false;
	public bool failure3 = false;
	public bool failure4 = false;
	public bool failure5 = false;
	public bool failure6 = false;
	public bool failure7 = false;
	public bool failure8 = false;
	public bool failure9 = false;

	public bool[] failures;

	private InputField brokerInput;
	// Start is called before the first frame update
	void Start()
	{
		sessionManager = GameObject.Find("Session Manager").GetComponent<SessionManager>();
		failures = new bool[9];

		failures[0] = failure1;
		failures[1] = failure2;
		failures[2] = failure3;
		failures[3] = failure4;
		failures[4] = failure5;
		failures[5] = failure6;
		failures[6] = failure7;

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

        try
        {
			byte code = client.Connect(clientId);
			Debug.Log("Mqtt: " + code);
		}
        catch (Exception ex)
        {
			Debug.Log("Error: " + ex.Message);
        }

		
		


		// subscribe to the topic "/home/temperature" with QoS 2 
		client.Subscribe(new string[] { "iotgateway" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
		client.Subscribe(new string[] { "factory/state/#" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
	}

	public void Disconnect()
    {
		if (client != null)
		{
			client.Disconnect();
		}

	}

	public void startFactory() {
		client.Publish("iotgateway/write", System.Text.Encoding.UTF8.GetBytes("[{\"id\": \"factory-1.factory-1.CPU 1511C-1 PN.Inputs.A7-I3 - Start button\",\"v\": true}]"), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE , false);
		}

	public void stopFactory() {
		client.Publish("iotgateway/write", System.Text.Encoding.UTF8.GetBytes("[{\"id\": \"factory-1.factory-1.CPU 1511C-1 PN.Inputs.A7-I1 - Stop button make\",\"v\": true}]"), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, false);
		}

	public void sendFailureState(int failureNumber, bool failure)
    {
		client.Publish("factory/state/"+failureNumber, System.Text.Encoding.UTF8.GetBytes("{ \"timestamp\":1590585164002,\"values\":[{\"id\":\"failure-" + failureNumber +"\",\"v\":" + failure.ToString().ToLower() + ",\"q\":true,\"t\":1590585161491}]}"), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, true);

    }

	public void removeFactoryError() {
		client.Publish("iotgateway/write", System.Text.Encoding.UTF8.GetBytes("[{\"id\": \"factory-1.factory-1.CPU 1511C-1 PN.Memory.Failure\",\"v\": 0}]"), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, false);
	}

	public void sendDuration(int failureNumber, float duration){
	  client.Publish("experiment/failure/"+failureNumber, System.Text.Encoding.UTF8.GetBytes("{ \"timestamp\":1590585164002,\"values\":[{\"id\":\"failure-" + failureNumber +"\",\"v\":" + duration.ToString() + ",\"q\":true,\"t\":1590585161491}]}"), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, true);

	}

	

	void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
		{
			// Debug.Log("Received: " + System.Text.Encoding.UTF8.GetString(e.Message));

			state = System.Text.Encoding.UTF8.GetString(e.Message);
			
			mqttData mqttdata =  JsonUtility.FromJson<mqttData>(state);

			for (int i = 0; i < mqttdata.values.Length; i++)
			{
				if (mqttdata.values[i].id == "factory-1.factory-1.CPU 1511C-1 PN.Memory.A4 Farbsensor") {
					colorValue = int.Parse(mqttdata.values[i].v);
				}

				else if (mqttdata.values[i].id == "factory-1.factory-1.CPU 1511C-1 PN.Memory.A0 Default position") {
					startPosition = bool.Parse(mqttdata.values[i].v);
				}

				else if (mqttdata.values[i].id == "factory-1.factory-1.CPU 1511C-1 PN.Memory.A0 Failure") {
					failure = bool.Parse(mqttdata.values[i].v);
				}

				else if (mqttdata.values[i].id == "factory-1.factory-1.CPU 1511C-1 PN.Memory.SQ1 Sequence is running") {
					running = bool.Parse(mqttdata.values[i].v);
				}

				else if (mqttdata.values[i].id == "factory-1.factory-1.CPU 1511C-1 PN.Outputs.A1-Q3 - H motor towards cabinet") {
					left = bool.Parse(mqttdata.values[i].v);
				}

				else if (mqttdata.values[i].id == "factory-1.factory-1.CPU 1511C-1 PN.Outputs.A1-Q4 - H motor towards conveyor") {
					right = bool.Parse(mqttdata.values[i].v);
				}

				else if (mqttdata.values[i].id == "factory-1.factory-1.CPU 1511C-1 PN.Outputs.A1-Q5 - Vert_ motor down") {
					down = bool.Parse(mqttdata.values[i].v);
				}

				else if (mqttdata.values[i].id == "factory-1.factory-1.CPU 1511C-1 PN.Outputs.A1-Q6 - Vert_ motor up") {
					up = bool.Parse(mqttdata.values[i].v);
				}

				else if (mqttdata.values[i].id == "factory-1.factory-1.CPU 1511C-1 PN.Outputs.A1-Q7 - Fork extend") {
					extend = bool.Parse(mqttdata.values[i].v);
				}

				else if (mqttdata.values[i].id == "factory-1.factory-1.CPU 1511C-1 PN.Outputs.A1-Q8 - Fork retract") {
					retract = bool.Parse(mqttdata.values[i].v);
				}

				else if (mqttdata.values[i].id == "factory-1.factory-1.CPU 1511C-1 PN.Outputs.A1-Q8 - Fork retract1") {
					retract1 = bool.Parse(mqttdata.values[i].v);
				}

				else if (mqttdata.values[i].id == "factory-1.factory-1.CPU 1511C-1 PN.Inputs.A3-I2") {
					//bool oldSagen = sagen;
					sagen = bool.Parse(mqttdata.values[i].v);
					//if (oldSagen && !sagen) {
					//	afterSagen = true;
					//}
				}
				

				//else if (mqttdata.values[i].id == "factory-1.factory-1.CPU 1511C-1 PN.Memory.A6 Failure 1") {
				//						failure1 = bool.Parse(mqttdata.values[i].v);
				//	failures[0] = failure1;
				//}



            else if (mqttdata.values[i].id == "failure-1")
            {
                failure1 = bool.Parse(mqttdata.values[i].v);
   
            }

			else if (mqttdata.values[i].id == "failure-2")
			{
				failure2 = bool.Parse(mqttdata.values[i].v);
			}

			else if (mqttdata.values[i].id == "failure-3")
			{
				failure3 = bool.Parse(mqttdata.values[i].v);
	
			}

			else if (mqttdata.values[i].id == "failure-4")
			{
				failure4 = bool.Parse(mqttdata.values[i].v);

			}

			else if (mqttdata.values[i].id == "failure-5")
			{
				failure5 = bool.Parse(mqttdata.values[i].v);
	
			}

			else if (mqttdata.values[i].id == "failure-6")
			{
				failure6 = bool.Parse(mqttdata.values[i].v);
			
			}


			else if (mqttdata.values[i].id == "failure-7")
			{
				failure7 = bool.Parse(mqttdata.values[i].v);
	
			}


			else if (mqttdata.values[i].id == "failure-8")
			{
				failure8 = bool.Parse(mqttdata.values[i].v);

			}


			else if (mqttdata.values[i].id == "failure-9")
			{
				failure9 = bool.Parse(mqttdata.values[i].v);
		
			}



		}


			


			//float Red = float.Parse(converted[0]) ;
			//float Green = float.Parse(converted[1]);
			//float Blue = float.Parse(converted[2]);
			//float Sum = Red + Green + Blue;
			//Red /= Sum;
			//Green /= Sum;
			//Blue /= Sum;
			//Debug.Log("Red: " + Red + " Green: " + Green + " Blue: " + Blue);
			//newColor = new Color(Red, Green, Blue);

			//Debug.Log(newColor);
		}

	// Update is called once per frame
	void Update()
	{
		if(client != null) { isConnected = client.IsConnected; }
		
	}

	private void OnDestroy()
	{
		if (client != null) 
		{
			client.Disconnect();
		}
		
	}
}