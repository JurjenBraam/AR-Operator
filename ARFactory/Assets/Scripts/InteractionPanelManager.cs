using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionPanelManager : MonoBehaviour
{
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateInteractionPanel()
    {
        animator.SetBool("isActive", true);
    }

    public void ActivateInstructionState()
    {
        animator.SetBool("instructionOpen", true);
    }
    public void DismissInteractionPanel()
    {
        animator.SetBool("isActive", false);
        animator.SetBool("instructionOpen", false);

    }
}
