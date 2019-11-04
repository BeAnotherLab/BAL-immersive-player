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

	#region Unity Methods
    void Awake()
	{

		if (instance == null)
			instance = this;


		if (VideoPlayerSettings.is360) {
		    fullsphere.SetActive (true);
            semisphere.SetActive(false);
            selectedDisplay = fullsphere;
		} 

		else {
		    semisphere.SetActive (true);
            fullsphere.SetActive(false);
            selectedDisplay = semisphere;
		}
        
    }
		
	#endregion
}
