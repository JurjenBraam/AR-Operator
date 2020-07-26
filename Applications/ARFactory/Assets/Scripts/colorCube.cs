using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class colorCube : MonoBehaviour
{
    public Color currentColor;
    public Color lastColor;
    public mqttManager mqtt;

    void updateColor(int colorValue)
    {
        Color oldColor = currentColor;

        switch (colorValue)
        {
        case int v when (v > 16294 && v < 17800):
            // Debug.Log("Blauw");
            currentColor = Color.blue;
            break;

        case int v when (v > 6200 && v < 8900):
            // Debug.Log("Wit");
            currentColor = Color.white;
            break;

        case int v when (v > 11000 && v < 14500):
            // Debug.Log("Rood");
            currentColor = Color.red;
            break;

            default: 
            // color = new Color();
            // Debug.Log("Transparant");
            if (oldColor != Color.clear) {
                lastColor = oldColor;
            }
            currentColor = Color.clear;
            break;
        } 

        // if (currentColor != Color.clear) {
        //     // Debug.Log(color);
        //     lastColor = currentColor;

        // }
    }

    
    public void SetColor()
    {
        
        transform.Find("Current Disk").GetComponent<Renderer>().material.color = currentColor;
        transform.Find("Last Disk").GetComponent<Renderer>().material.color = lastColor;

        // gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "Color: " + currentColor;
    }

    // Start is called before the first frame update
    void Start()
    {
        mqtt = GameObject.FindGameObjectWithTag("mqtt").GetComponent<mqttManager>();
    }

    // Update is called once per frame
    void Update()
    {
        updateColor(mqtt.colorValue);
        SetColor();
        
    }
}
