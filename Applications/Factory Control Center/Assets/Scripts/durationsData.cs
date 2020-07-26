using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class durationData
{
    public int participantID;
    public string timestamp;
    public List<failureRun> failureRuns;

}

[System.Serializable]
public class failureRun
{
    public string run;
    public int failureCounter;
    public int failureID;
    public float duration;
}
