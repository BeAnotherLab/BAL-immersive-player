using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;
using RenderHeads.Media.AVProVideo;

public class DisplaySettings : MonoBehaviour
{
    
    #region Public 
    public GameObject semisphere, fullsphere, assistantPlane;

    [HideInInspector]
    public GameObject selectedDisplay;//eventually change to scriptable object architecture.

    #endregion

    #region Private variables
    private string assistantVideoPath;
    private bool enableAssistantVideo;
    #endregion

    #region Monobehavior Methods
    private void Awake()
    {
        if (PlayerPrefs.GetInt("SphereSelector") == 0)
            Set360Settings(false);
        else
            Set360Settings(true);
    }
    #endregion

    #region Public Methods
    public void Set360Settings(bool enable360)
    {
        if (enable360)
        {
            fullsphere.SetActive(true);
            semisphere.SetActive(false);
            selectedDisplay = fullsphere;
        }

        else
        {
            semisphere.SetActive(true);
            fullsphere.SetActive(false);
            selectedDisplay = semisphere;
        }

    }

    public void SetAssistantVideoPath(string _path)
    {
        assistantVideoPath = _path;
    }

    public void SetAssistantVideoSettings(bool _enableAssistantVideo)
    {
        enableAssistantVideo = _enableAssistantVideo;
        
    }

    public void SetDisplaySettingsUponLoad()
    {
        Camera assistantVideoCamera = assistantPlane.GetComponent<Camera>();

        if (enableAssistantVideo && assistantVideoPath != null)
        {
            assistantPlane.SetActive(true);
            assistantVideoCamera.enabled = true;
        }

        else
        {
            assistantPlane.SetActive(false);
            assistantVideoCamera.enabled = false;
        }
    }

    public void SetStereoVideoSettings(bool enableStereo)
    {
        MediaPlayer _semisphereMediaPlayer = semisphere.GetComponent<MediaPlayer>();
        MediaPlayer _fullSphereMediaPlayer = fullsphere.GetComponent<MediaPlayer>();

        if (enableStereo)
        {
            Debug.Log("stereo mode enabled");
            _semisphereMediaPlayer.m_StereoPacking = StereoPacking.TopBottom;
            _fullSphereMediaPlayer.m_StereoPacking = StereoPacking.TopBottom;
        }

        else
        {
            Debug.Log("stereo mode disabled");
            _semisphereMediaPlayer.m_StereoPacking = StereoPacking.None;
            _fullSphereMediaPlayer.m_StereoPacking = StereoPacking.None;
        }

        fullsphere.GetComponent<ApplyToMesh>().ForceUpdate();
        fullsphere.GetComponent<ApplyToMesh>().ForceUpdate();

    }
    #endregion
}
