using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class toggleChecker : MonoBehaviour
{
    private Button nextButton;
    private ToggleGroup[] toggleGroups;
    // Start is called before the first frame update
    void Start()
    {
        toggleGroups = GetComponentsInChildren<ToggleGroup>();
        nextButton = GetComponentInChildren<Button>();
    }


    private void checkToggleGroups()
    {
        if (toggleGroups == null) { return; }

        int togglesOn = 0;

        foreach (ToggleGroup group in toggleGroups)
        {
            if(group.AnyTogglesOn()) { togglesOn += 1; }
        }

        if(togglesOn == toggleGroups.Length)
        {
            nextButton.interactable = true;
        }
        else
        {
            nextButton.interactable = false;
        }

    }

    // Update is called once per frame
    void Update()
    {
        checkToggleGroups();
    }
}
