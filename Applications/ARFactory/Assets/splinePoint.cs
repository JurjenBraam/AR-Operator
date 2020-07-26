using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SplineMesh;

public class splinePoint : MonoBehaviour
{
    public Spline spline;
    float elapsed = 0f;
    Vector3 currentPosition;
    Vector3 newPosition;
    Vector3 knopPosition;
    GameObject Knop;

    // Start is called before the first frame update
    void Start()
    {
        spline = transform.GetComponent<Spline>();
        transform.GetComponent<SplineMeshTiling>().curveSpace = false;
        transform.GetComponent<SplineMeshTiling>().curveSpace = true;
        Knop = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        elapsed += Time.deltaTime;

        currentPosition = spline.nodes[0].Position;
        knopPosition = Knop.transform.position;
        knopPosition.z = knopPosition.z + Mathf.Sin(elapsed) * 0.0008f;

        newPosition = currentPosition;
        newPosition.z = newPosition.z + Mathf.Sin(elapsed) * 0.01f;

        spline.nodes[0].Position = newPosition;
        Knop.transform.position = knopPosition;
    }
}
