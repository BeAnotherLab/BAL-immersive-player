using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssistantVideoPlayer : MonoBehaviour
{

    private GameObject _dpCameras;
    public GameObject assistantVideoObject;
    public Camera assistantVideoCamera;

    public static AssistantVideoPlayer instance;

    public void SetAssistantVideoSettings(bool enableAssistantVideo)
    {
        if (enableAssistantVideo)
        {
            assistantVideoObject.SetActive(true);
            assistantVideoCamera.enabled = true;

         _dpCameras = DisplaySelector.instance.selectedDisplay.gameObject;//check
         _dpCameras.transform.GetChild(0).Find("Monitor Camera").gameObject.SetActive(false);//check
         _dpCameras.transform.GetChild(0).Find("Monitor Camera round").gameObject.SetActive(false);//check
            
        }
        else
        {
            assistantVideoObject.SetActive(false);
            assistantVideoCamera.enabled = false;
        }
    }

}
