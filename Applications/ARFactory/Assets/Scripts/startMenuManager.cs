using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class startMenuManager : MonoBehaviour
{
    private InputField brokerInput;

    // Start is called before the first frame update
    void Start()
    {
        brokerInput = GameObject.Find("Broker InputField").GetComponent<InputField>();
    }
    public void SaveBroker()
    {

        PlayerPrefs.SetString("brokerIP", brokerInput.text);
    }

    public void startARscene()
    {
        SceneManager.LoadScene("ARScene", LoadSceneMode.Single);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
