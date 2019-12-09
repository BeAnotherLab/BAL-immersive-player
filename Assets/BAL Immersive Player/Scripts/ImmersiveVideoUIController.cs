using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImmersiveVideoUIController : MonoBehaviour
{

	#region Public variables

	public Text rotationText;
	public Text elapsedTimeText;
	public Slider timeSlider;	

	public static ImmersiveVideoUIController instance;

	[HideInInspector]
	public bool timeSliderIsInteracting;

	#endregion

	#region Private variables
	//private DisplaySelector _dpSelector;
	private Transform _dpTransform;
	private Transform _cameraTransform;

	#endregion

	#region Unity methods

	void Awake(){
		
		if (instance == null)
			instance = this;
		
	}

    // Start is called before the first frame update
    void Start()
    {
		_dpTransform = DisplaySelector.instance.selectedDisplay.transform;
		_cameraTransform = GameObject.FindGameObjectWithTag ("MainCamera").transform;
    }

    // Update is called once per frame
    void Update()
    {
		UpdateRotationText ();
    }

	#endregion

	#region Public methods

	public void UpdateTimeText(string textToPrint){
		elapsedTimeText.text = textToPrint;
	}

	#endregion

	#region Private methods

	private void UpdateRotationText(){
		rotationText.text = "Pitch: " + _dpTransform.rotation.eulerAngles.x.ToString () +
			"\nYaw: " + _dpTransform.rotation.eulerAngles.y.ToString () +
			"\nRoll: " + _cameraTransform.eulerAngles.z.ToString ();
	}

	#endregion
}
