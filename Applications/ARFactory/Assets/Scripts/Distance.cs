using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Distance : MonoBehaviour
{
    public float distanceToCamera = 0f;
    public Animator animator;
    public Animator textAnimator;
    public float rotationXOffset = 0.0f;
    public float rotationYOffset = 0.0f;
    public float distanceSetting = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        rotationXOffset = GameObject.Find("X-axis OffsetSlider").GetComponent<Slider>().value;
        rotationYOffset = GameObject.Find("Y-axis OffsetSlider").GetComponent<Slider>().value;
        distanceSetting = GameObject.Find("Distance Slider").GetComponent<Slider>().value;

        //transform.localEulerAngles = new Vector3(0f, Camera.main.transform.localEulerAngles.y + rotationOffset, 0.0f);
        transform.eulerAngles = new Vector3(Camera.main.transform.localEulerAngles.x + rotationYOffset, Camera.main.transform.localEulerAngles.y + rotationXOffset, 0.0f);
        distanceToCamera = Vector3.Distance(Camera.main.transform.position, transform.position);
        if (distanceToCamera > distanceSetting) {
            
            //animator.SetBool("feelsPressence", false);
        } 
        else {
            //animator.SetBool("feelsPressence", true);
        }
    }
}
