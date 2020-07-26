using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float Step = 0f;
    public float Offset = 0f;
    public float Amplitude = 0.002f;

    // Start is called before the first frame update
    void Start()
    {
        Offset = transform.position.y + transform.localScale.y;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Step += 0.1f;

        if (Step > 9999) { Step = 1; }
        Vector3 currentPosition = transform.position;

        currentPosition.y = Amplitude *  Mathf.Sin(Step) + Offset;

        transform.position = currentPosition;
    }
}
