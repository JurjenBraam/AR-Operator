using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class stateManager : MonoBehaviour
{
    public string componentName;
    public string failure;
    public string componentInstruction;
    public GameObject okSign;
    public GameObject errorSign;
    public bool setActive = false;
    public bool isSelected = false;
    public bool solutionOpen = false;
    public mqttManager mqtt;
    public int number;
    public GameObject solutionHolder;
    public GameObject selectionIndicator;



    // Start is called before the first frame update
    void Start()
    {
        mqtt = GameObject.Find("MQTT Manager").GetComponent<mqttManager>();
        
        okSign = transform.Find("OK Sign").gameObject;
        errorSign = transform.Find("Error Sign").gameObject;
        selectionIndicator = transform.Find("Selection Indicator").gameObject;

        solutionHolder = transform.parent.transform.Find("Solution Holder").gameObject;
        
        solutionHolder.SetActive(false);
        selectionIndicator.SetActive(false);
        errorSign.SetActive(false);
        int.TryParse(gameObject.transform.parent.name.Split(' ')[0], out number);

        
    }


    public void toggleComponent()
    {
        isSelected = !isSelected;
 

    }

    public void toggleSolution()
    {
        solutionOpen = !solutionOpen;
        solutionHolder.SetActive(solutionOpen);
    }

    // Update is called once per frame
    void Update()
    {
        setActive = mqtt.failures[number - 1];
        errorSign.SetActive(setActive && !solutionOpen);
        selectionIndicator.SetActive(isSelected && !solutionOpen);
        solutionHolder.SetActive(solutionOpen);
        okSign.SetActive(!setActive && !solutionOpen);
    }
}
