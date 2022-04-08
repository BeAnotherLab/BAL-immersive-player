using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class AssistantVideoPlayer : MonoBehaviour
{

    public GameObject fullSphere, semiSphere;
    private GameObject selectedSphere;
    public GameObject assistantVideoObject;
    public Camera assistantVideoCamera;

    public static AssistantVideoPlayer instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    public void SetSelectedDisplay(bool is360)
    {
        if (is360)
            selectedSphere = fullSphere;
        else
            selectedSphere = semiSphere;
    }

    public void SetAssistantVideoSettings(bool enableAssistantVideo)
    {

        if (enableAssistantVideo)
        {
            assistantVideoObject.SetActive(true);
            assistantVideoCamera.enabled = true;         

         selectedSphere.transform.GetChild(0).Find("Monitor Camera").gameObject.SetActive(false);//check 
         selectedSphere.transform.GetChild(0).Find("Monitor Camera round").gameObject.SetActive(false);//check
            
        }
        else
        {
            assistantVideoObject.SetActive(false);
            assistantVideoCamera.enabled = false;
        }
    }

}
