using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrow : MonoBehaviour
{

    public mqttManager mqtt;
    public GameObject pause;
    public GameObject leftArrow;
    public GameObject rightArrow;
    public GameObject upArrow;
    public GameObject downArrow;
    public GameObject retractArrow;
    public GameObject extendArrow;

    // Start is called before the first frame update
    void Start()
    {
        mqtt = GameObject.FindGameObjectWithTag("mqtt").GetComponent<mqttManager>();
        pause = GameObject.Find("pause");
        leftArrow = GameObject.Find("arrow left");
        rightArrow = GameObject.Find("arrow right");
        upArrow = GameObject.Find("arrow up");
        downArrow = GameObject.Find("arrow down");
        retractArrow = GameObject.Find("arrow retract");
        extendArrow = GameObject.Find("arrow extend");
        leftArrow.SetActive(false);
        rightArrow.SetActive(false);
        upArrow.SetActive(false);
        downArrow.SetActive(false);
        retractArrow.SetActive(false);
        extendArrow.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        leftArrow.SetActive(mqtt.left);
        rightArrow.SetActive(mqtt.right);
        upArrow.SetActive(mqtt.up);
        downArrow.SetActive(mqtt.down);
        retractArrow.SetActive(mqtt.retract);
        extendArrow.SetActive(mqtt.extend);

        if(!mqtt.left && !mqtt.right && !mqtt.up && !mqtt.down && !mqtt.retract && !mqtt.extend) { pause.SetActive(true); }
        else { pause.SetActive(false); }
    }
}
