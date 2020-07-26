using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disableOnRuntime : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        gameObject.SetActive(false);
#if (UNITY_EDITOR)
        gameObject.SetActive(true);
#endif
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
