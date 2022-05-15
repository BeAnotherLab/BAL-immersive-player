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

    public void SetStereoVideoSettings(bool enableStereo)
    {
        MediaPlayer _semisphereMediaPlayer = semisphere.GetComponent<MediaPlayer>();
        MediaPlayer _fullSphereMediaPlayer = fullsphere.GetComponent<MediaPlayer>();
        ApplyToMesh _fullSphereApplyToMesh = fullsphere.GetComponent<ApplyToMesh>();
        //UpdateStereoMaterial _semisphereStereoMaterial = semisphere.GetComponent<UpdateStereoMaterial>();
        //UpdateStereoMaterial _fullSphereStereoMaterial = semisphere.GetComponent<UpdateStereoMaterial>();

        // assistantVideoCamera = assistantPlane.GetComponent<Camera>();

        if (enableStereo)
        {
            Debug.Log("stereo enabled");
            _semisphereMediaPlayer.m_StereoPacking = StereoPacking.LeftRight;
            _fullSphereMediaPlayer.m_StereoPacking = StereoPacking.LeftRight;
            _fullSphereApplyToMesh.ForceUpdate();
        }

        else
        {
            Debug.Log("stereo disabled");
            _semisphereMediaPlayer.m_StereoPacking = StereoPacking.None;
            _fullSphereMediaPlayer.m_StereoPacking = StereoPacking.None;
            _fullSphereApplyToMesh.ForceUpdate();
            //_semisphereStereoMaterial.
            //_fullSphereMediaPlayer.m_StereoPacking = StereoPacking.None;
        }
    }
    #endregion
}
