using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class pathPoint : MonoBehaviour
{

    public PathCreator pathCreator;
    public float x = 0;
    public float y = 0;
    public float z = 0;
    public int points;
    public float elapsed = 0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
      
    pathCreator.bezierPath.MovePoint(0, new Vector3(x, y, z));

    }

}
