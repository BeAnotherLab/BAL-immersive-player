using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class DisplaySettings : MonoBehaviour
{
    
    #region Public 
    public GameObject semisphere, fullsphere, assistantPlane;

    [HideInInspector]
    public GameObject selectedDisplay;//eventually change to scriptable object architecture.

    #endregion

    #region Private variables
    private string assistantVideoPath;
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

    public void SetAssistantVideoSettings(bool enableAssistantVideo)
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
    #endregion
}
