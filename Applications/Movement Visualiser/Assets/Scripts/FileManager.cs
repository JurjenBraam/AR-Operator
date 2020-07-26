using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;

public class FileManager : MonoBehaviour
{
    public MovementLog movementLog;
    public GameObject cube;
    public float elapsed= 0f;
    public float current = 0;
    public Slider slider;
    public bool play = false;

    private SettingsMenuManager settingsManager;
    private mqtt_test mqtt;
    private TMP_Dropdown recordingsDropdown;
    private List<TMP_Dropdown.OptionData> options;
    private FileInfo[] files;
    public TMP_Text playPauseButton;

    // Start is called before the first frame update
    void Start()
    {

        settingsManager = GameObject.Find("Settings Panel").GetComponent<SettingsMenuManager>();
        mqtt = GameObject.Find("MQTT Manager").GetComponent<mqtt_test>();
        slider = GameObject.Find("Slider").GetComponent<Slider>();
        cube = GameObject.Find("iPad").gameObject;

        recordingsDropdown = GameObject.Find("Recordings Dropdown").GetComponent<TMP_Dropdown>();
        playPauseButton = GameObject.Find("Play Pause Button").GetComponentInChildren<TMP_Text>();

        Debug.Log(Application.persistentDataPath);

        var info = new DirectoryInfo(Application.persistentDataPath);
        files = info.GetFiles();

        options = new List<TMP_Dropdown.OptionData>();

        foreach ( FileInfo file in files)
        {
            Debug.Log(file.Extension);
            if (file.Extension == ".json")
            {
                options.Add(new TMP_Dropdown.OptionData(file.CreationTime.ToString()));
            }
            
          
            
        }

        recordingsDropdown.AddOptions(options);

        ;
      

    }

    public void togglePlayPause()
    {
        play =! play;

        if (play) { playPauseButton.text = "Pause"; }
        else { playPauseButton.text = "Play"; }

    }

    public void loadFile()
    {

        string fileName = files[recordingsDropdown.value -1].FullName;
   
        StreamReader stream = new StreamReader(fileName);
        string data = stream.ReadToEnd();
        stream.Close();
        movementLog = JsonUtility.FromJson<MovementLog>(data);

        current = 0;
        elapsed = 0f;
        slider.maxValue = movementLog.movements.Count;
    }

    public void moveiPad(CustomMovement newPosition)
    {
        Vector3 position = newPosition.position;
        Vector3 rotation = newPosition.rotation;

        cube.transform.position = position;
        cube.transform.eulerAngles = rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (play)
        {
            elapsed += Time.deltaTime;


            if (elapsed >= 0.1f)
            {
                elapsed %= 0.1f;

                moveiPad(movementLog.movements[(int)current]);

                current += 1;
                if (current >= movementLog.movements.Count)
                { current = 0; }

            }
            slider.value = current;
        }

        else if (settingsManager.realtime)
        {
            moveiPad(mqtt.latestMovement);
        }
       


    }
}
