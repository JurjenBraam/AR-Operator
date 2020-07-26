using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;


[System.Serializable]
public class MovementLog
{
    public string ID;
    public int failure;
    public long startTime;
    public List<CustomMovement> movements;

    public MovementLog(string nID, int nfailure)
    {
        ID = nID;
        failure = nfailure;
        startTime = new System.DateTimeOffset(System.DateTime.Now).ToUnixTimeMilliseconds();
        movements = new List<CustomMovement>();
    }

    public void addMovement(Vector3 position, Vector3 rotation)
    {
        long time = new System.DateTimeOffset(System.DateTime.Now).ToUnixTimeMilliseconds();
        CustomMovement movement = new CustomMovement(time - startTime, position, rotation );
        movements.Add(movement);
    }

}

[System.Serializable]
public class CustomMovement
{
    public long timestamp;
    public Vector3 position;
    public Vector3 rotation;

    public CustomMovement(long newTimestamp, Vector3 newPosition, Vector3 newRotation)
    {
        timestamp = newTimestamp;
        position = newPosition;
        rotation = newRotation;
    }
}
