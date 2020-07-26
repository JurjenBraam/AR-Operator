using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class mqttDataValues
{
    public string id;
    public string v;
    public bool q;
    public int t;
}

[System.Serializable]
public class mqttData
{
    public int timestamp;
    public mqttDataValues[] values;

    public static mqttData CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<mqttData>(jsonString);
    }

    // Given JSON input:
    // {"name":"Dr Charles","lives":3,"health":0.8}
    // this example will return a PlayerInfo object with
    // name == "Dr Charles", lives == 3, and health == 0.8f.
}





 //{
 //    "timestamp":1590585164002,
 //    "values":[
 //        {
 //            "id":"factory-1.factory-1.CPU 1511C-1 PN.Memory.A4 Farbsensor",
 //            "v":18550,
 //            "q":true,
 //            "t":1590585161491
 //        }
 //        ]
 //}