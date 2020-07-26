using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FactoryStatePanelManager : MonoBehaviour
{

    private Animator animator;
    private bool isActive = false;
    private mqttManager mqtt;


    private List<Toggle> failures;

    // Start is called before the first frame update
    void Start()
    {
        animator = GameObject.Find("Factory state Holder").GetComponent<Animator>();
        mqtt = GameObject.Find("MQTT Manager").GetComponent<mqttManager>();

        failures = GetComponentsInChildren<Toggle>().ToList<Toggle>();

        

        foreach(Toggle failure in failures)
        {
            Toggle selectedFailure = failure;
            selectedFailure.onValueChanged.AddListener((value) => onFailureToggleChange(selectedFailure));
        }
    }

    void checkStates()
    {
        int failureCount = 0;

        foreach (Toggle failure in failures)
        {
            failure.SetIsOnWithoutNotify(mqtt.failures[failureCount]);
            failureCount += 1;
        }
    }


    // Update is called once per frame
    void Update()
    {
        checkStates();
    }

    void onFailureToggleChange(Toggle failure)
    {
        string numb = failure.name.Split(' ')[1];
        int number;
        int.TryParse(numb, out number);

        if(mqtt.isConnected)
        {
            mqtt.sendFailureState(number, failure.isOn);
            if(failure.isOn)
            {
                mqtt.stopFactory();
            }
        }
        
    }



    public void onButtonPress()
    {
        isActive = !isActive;
        animator.SetBool("isActive", isActive);
    }
}
