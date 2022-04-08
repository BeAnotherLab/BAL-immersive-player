using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplaySelector : MonoBehaviour
{
    
    #region Public variables
    public GameObject semisphere;
    public GameObject fullsphere;

    [HideInInspector]
    public GameObject selectedDisplay;

    public static DisplaySelector instance;

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
    #endregion
}
