using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class CameraSettingsManager : MonoBehaviour
{
    private SettingsMenuManager settingsMenuManager;
    private Toggle recordToggle;
    public GameObject startButton;
    private bool recording = false;
    public MovementLog movementLog;
    public string jsonSavePath;
    private mqttManager mqtt;
    private bool isActive = false;
    private Animator animator;
    private GameObject position;
    private GameObject rotation;

    private Text positionX;
    private Text positionY;
    private Text positionZ;

    private Text rotationX;
    private Text rotationY;
    private Text rotationZ;

    public float elapsed = 0f;

    // Start is called before the first frame update
    void Start()
    {
        settingsMenuManager = GameObject.Find("Settings Holder").GetComponent<SettingsMenuManager>();
        recordToggle = GameObject.Find("Record Toggle").GetComponent<Toggle>();
        jsonSavePath = Application.persistentDataPath;

        animator = gameObject.transform.GetComponent<Animator>();
        mqtt = GameObject.Find("MQTT Manager").GetComponent<mqttManager>();
        position = gameObject.transform.Find("Position").gameObject;
        rotation = gameObject.transform.Find("Rotation").gameObject;

        positionX = position.transform.Find("X-Value").GetComponent<Text>();
        positionY = position.transform.Find("Y-Value").GetComponent<Text>();
        positionZ = position.transform.Find("Z-Value").GetComponent<Text>();

        rotationX = rotation.transform.Find("X-Value").GetComponent<Text>();
        rotationY = rotation.transform.Find("Y-Value").GetComponent<Text>();
        rotationZ = rotation.transform.Find("Z-Value").GetComponent<Text>();


    }


    public void startMovementLog(int failure)


    {

        movementLog = new MovementLog(System.Guid.NewGuid().ToString(), failure);
        elapsed = 0f;
        if (movementLog.movements.Count > 0) { resetMovementData(failure); }
        recording = true;

    }


    public void stopMovementLog()
    {
        recording = false;
        elapsed = 0f;
        if (movementLog.movements.Count > 60) { writeMovementData(); }
    }


    void updateCameraValues(Vector3 cameraPosition, Vector3 cameraRotation)
    {
        positionX.text = cameraPosition.x.ToString();
        positionY.text = cameraPosition.y.ToString();
        positionZ.text = cameraPosition.z.ToString();

        rotationX.text = cameraRotation.x.ToString();
        rotationY.text = cameraRotation.y.ToString();
        rotationZ.text = cameraRotation.z.ToString();


        if (recording && elapsed >= 0.1f)
        {
            elapsed %= 0.1f;
            movementLog.addMovement(cameraPosition, cameraRotation);

            if (settingsMenuManager.realtime)
            {
               mqtt.sendMovementData(movementLog.movements[movementLog.movements.Count - 1]);
            }
        }
       

    }

    public void resetMovementData(int failure)
    {
        elapsed = 0f;
        movementLog = new MovementLog(System.Guid.NewGuid().ToString(), failure);
    }

    public void writeMovementData()
    {
        if (movementLog.movements.Count > 0)
        {
            TimeSpan time = TimeSpan.FromMilliseconds(movementLog.startTime);
            DateTime startdate = new DateTime(1970, 1, 1) + time;
            string data = JsonUtility.ToJson(movementLog);
            Debug.Log(startdate.ToLocalTime().ToString("HH:mm:ss dd-MM-yyyy"));
            File.WriteAllText(jsonSavePath + "/MoventLog - Failure " +  movementLog.failure.ToString() + " - " +  startdate.ToLocalTime().ToString("HH:mm:ss dd-MM-yyyy") + ".json", data);

        }
        else
        {
            Debug.Log("No movement data");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (recording) { elapsed += Time.deltaTime; }

        
        updateCameraValues(Camera.main.transform.position, Camera.main.transform.eulerAngles);
    }


    public void ToggleScreen(bool setActive)
    {
        isActive = setActive;
        animator.SetBool("isActive", isActive);
    }
}
