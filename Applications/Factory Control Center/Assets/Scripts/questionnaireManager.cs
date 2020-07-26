using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class questionnaireManager : MonoBehaviour
{
    private Animator introAnimator;
    private Animator secondRunAnimator;
    private Animator panel1Animator;
    private Animator panel2Animator;
    private Animator panel3Animator;
    private Animator panel4Animator;

    private SessionManager sessionManager;
    private ToggleGroup[] toggleGroups;

    public questionnaireData questionnaireData;

    // Start is called before the first frame update
    void Start()
    {
        introAnimator = gameObject.transform.Find("Intro Panel").GetComponent<Animator>();
        secondRunAnimator = gameObject.transform.Find("Second Run").GetComponent<Animator>();
        panel1Animator = gameObject.transform.Find("Panel 1").GetComponent<Animator>();
        panel2Animator = gameObject.transform.Find("Panel 2").GetComponent<Animator>();
        panel3Animator = gameObject.transform.Find("Panel 3").GetComponent<Animator>();
        panel4Animator = gameObject.transform.Find("Panel 4").GetComponent<Animator>();
        sessionManager = GameObject.Find("Session Manager").GetComponent<SessionManager>();
        toggleGroups = GetComponentsInChildren<ToggleGroup>();
    }

    public void slideInIntroPanel()
    {
        introAnimator.SetTrigger("slideIn");
    }

    public void dismissIntroPanel()
    {
        panel1Animator.SetTrigger("slideIn");
        introAnimator.SetTrigger("slideOut");
    }

    public void slideInSecondRunPanel()
    {
        secondRunAnimator.SetTrigger("slideIn");
        introAnimator.SetTrigger("slideOut");
    }

    public void dismissSecondRunPanel()
    {
        panel1Animator.SetTrigger("slideIn");
        secondRunAnimator.SetTrigger("slideOut");
    }

    public void dismissPanel1()
    {
        panel2Animator.SetTrigger("slideIn");
        panel1Animator.SetTrigger("slideOut");
    }

    public void dismissPanel2()
    {

        panel3Animator.SetTrigger("slideIn");
        panel2Animator.SetTrigger("slideOut");
    }

    public void dismissPanel3()
    {
        panel4Animator.SetTrigger("slideIn");
        panel3Animator.SetTrigger("slideOut");
    }


    public void dismissPanel4AndSave()
    {

        saveQuestionnaire();



        panel4Animator.SetTrigger("slideOut");
        introAnimator.SetTrigger("toBase");
        panel1Animator.SetTrigger("toBase");
        panel2Animator.SetTrigger("toBase");
        panel3Animator.SetTrigger("toBase");
        panel4Animator.SetTrigger("toBase");

        resetQuestionnaire();

    }

    private void saveQuestionnaire()
    {


        Type myType = typeof(questionnaireData);


        foreach (ToggleGroup group in toggleGroups)
        {
            if(group.ActiveToggles().Count() < 1) { continue; }
            else if (group.transform.name == "Example Question") { continue; }

            int question = int.Parse(group.transform.name.Split(' ')[1]);
            int answer = int.Parse(group.ActiveToggles().First().name.Split(' ')[1]);

            FieldInfo fieldInfo = myType.GetField("question" + question.ToString());
 
            fieldInfo.SetValue(questionnaireData, answer);
          
        }
        questionnaireData.timestamp = DateTime.Now.ToString();
       
        sessionManager.saveQuestionnaireData(questionnaireData);

    }


    private void resetQuestionnaire()
    {
        foreach (ToggleGroup group in toggleGroups)
        {
            group.SetAllTogglesOff();
        }

        questionnaireData = new questionnaireData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
