using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayRotator : MonoBehaviour
{
	#region Private variables
	private DisplaySelector _displaySelector;
	private Transform _displayTransform;
	#endregion

	#region Unity methods
    // Start is called before the first frame update
    void Start()
    {
		_displaySelector = (DisplaySelector)FindObjectOfType(typeof(DisplaySelector));   
		_displayTransform = _displaySelector.selectedDisplay.transform;
    }
	#endregion

	#region Public Methods
	public IEnumerator RotateDisplaySmoothly(Vector3 targetRotation, float rotationDuration){


		//add from current vector (_displayTransform) to target vector (targetRotation) in time (rotationDuration).
		yield return null;


	}

	#endregion

}
