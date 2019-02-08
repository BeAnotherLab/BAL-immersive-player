using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothDisplayRotator : MonoBehaviour
{
	#region Public variables

	#endregion

	#region Private variables
	private DisplaySelector _displaySelector;
	private Transform _displayTransform;
	private ImmersiveVideoPlayer _imVideoPlayer;
	private Transform _cameraTransform;
	private bool _pause;
	#endregion


	#region Unity methods

	#endregion


	#region Public Methods

	public void InitializeSmoothRotation(Vector3 targetRotation, float rotationStartTime, float rotationDuration){
		
		_displaySelector = (DisplaySelector)FindObjectOfType(typeof(DisplaySelector));   
		_displayTransform = _displaySelector.selectedDisplay.transform;

		Debug.Log("Initial display transform " + _displaySelector.transform.rotation.eulerAngles);

		_cameraTransform = GameObject.FindGameObjectWithTag ("MainCamera").transform;
		_imVideoPlayer = (ImmersiveVideoPlayer)FindObjectOfType (typeof(ImmersiveVideoPlayer));
		StartCoroutine(RotateDisplaySmoothly(targetRotation, rotationStartTime, rotationDuration));
	}
	#endregion 

	#region Private variables
	private IEnumerator RotateDisplaySmoothly(Vector3 targetRotation, float rotationStartTime, float rotationDuration){

		float timeCount = 0;

		while (timeCount < rotationStartTime) {
			_pause = _imVideoPlayer.isPaused;
			if (!_pause) 
				timeCount += Time.fixedDeltaTime;
			yield return null;
		}


		//attempt for pausing
		float durationCount = 0;

		while (durationCount < rotationDuration) {
			_pause = _imVideoPlayer.isPaused;
			if (!_pause) {
				durationCount += Time.fixedDeltaTime;
				_displayTransform.rotation = Quaternion.Lerp (_displayTransform.rotation, Quaternion.Euler(new Vector3(targetRotation.x, targetRotation.y, 0f)), durationCount/rotationDuration);
				_cameraTransform.rotation = Quaternion.Lerp (_cameraTransform.rotation, Quaternion.Euler(new Vector3(0f, 0f, targetRotation.z)), durationCount/rotationDuration);
				Debug.Log ("current moment is " + durationCount / rotationDuration);
			}

			yield return null;
		}	

		Debug.Log("Reached "  + _displayTransform.rotation.eulerAngles);
		Destroy (this.GetComponent<SmoothDisplayRotator>());
	}

	#endregion

}
