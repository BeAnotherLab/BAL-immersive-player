using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssistantVideoPlayer : MonoBehaviour
{

    private GameObject _dpCameras;
    public GameObject assistantVideoObject;

    public static AssistantVideoPlayer instance;

    private void Awake()
    {

        if (instance == null)
            instance = this;

        if (VideoPlayerSettings.useAssistantVideo)
            assistantVideoObject.SetActive(true);
        else
            assistantVideoObject.SetActive(false);
    }
    // Start is called before the first frame update

    void Start()
    {
        if (VideoPlayerSettings.useAssistantVideo) { 
            _dpCameras = DisplaySelector.instance.selectedDisplay.gameObject;

            _dpCameras.transform.GetChild(0).Find("Monitor Camera").gameObject.SetActive(false);
            _dpCameras.transform.GetChild(0).Find("Monitor Camera round").gameObject.SetActive(false);


        }

    }

}
