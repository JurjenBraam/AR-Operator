using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rotating : MonoBehaviour
{
    public Slider rotationSlider;

    // Start is called before the first frame update
    void Start()
    {
        rotationSlider = GameObject.Find("RotationSlider").GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {

        transform.localEulerAngles = new Vector3(0f, 7*rotationSlider.value, 0f);
    }


}
