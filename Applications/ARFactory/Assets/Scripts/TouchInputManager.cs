using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.EventSystems;
using Input = InputWrapper.Input;


public class TouchInputManager : MonoBehaviour
{

    public Camera arCamera;
    public Vector2 touchPostion = default;
    private SessionManager sessionManager;

    // Start is called before the first frame update
    void Start()
    {
        arCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        sessionManager = GameObject.Find("Session Manager").GetComponent<SessionManager>();
    }

    // Update is called once per frame
    void Update()
    {   
        // Debug.Log(Input.touchCount);

        if(Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);

            touchPostion = touch.position;
            
            if (touch.phase == TouchPhase.Began && !EventSystem.current.IsPointerOverGameObject(0) && !EventSystem.current.IsPointerOverGameObject(-1)) 
            {
                Ray ray = arCamera.ScreenPointToRay(touch.position);
                RaycastHit hitObject;
                if(Physics.Raycast(ray, out hitObject))
                {
                    Debug.Log(hitObject.transform.gameObject.name);
                    
                    if (hitObject.transform.gameObject != null && hitObject.transform.gameObject.GetComponent<stateManager>() != null) {
                        //hitObject.transform.gameObject.GetComponent<stateManager>().toggleSolution();
                        sessionManager.SelectComponent(hitObject.transform.gameObject);
                    }

                }
            }
        }        
    }

}
