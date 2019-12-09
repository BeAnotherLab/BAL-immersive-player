using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothDisplayRotator : MonoBehaviour
{

	#region Private variables
	private Transform _displayTransform;
	private Transform _cameraTransform;
	#endregion


	#region Public Methods
	public void InitializeSmoothRotation(Vector3 targetRotation, float rotationStartTime, float rotationDuration){
		_displayTransform = DisplaySelector.instance.gameObject.transform;
		_cameraTransform = ImmersiveVideoPlayer.instance.cameraParentTransform;
		StartCoroutine(RotateDisplaySmoothly(targetRotation, rotationStartTime, rotationDuration));
	}
	#endregion 

	#region Private variables
	private IEnumerator RotateDisplaySmoothly(Vector3 targetRotation, float rotationStartTime, float rotationDuration){

		bool pause;
		float timeCount = 0;

		while (timeCount < rotationStartTime) {
			pause = ImmersiveVideoPlayer.instance.isPaused;
			if (!pause) 
				timeCount += Time.fixedDeltaTime;
			yield return null;
		}


		//attempt for pausing
		float durationCount = 0;

		//ISSUE! Quaternion.Lerp completes full range before reaching one
		while (durationCount < rotationDuration) {
			pause =  ImmersiveVideoPlayer.instance.isPaused;
			if (!pause) {
				durationCount += Time.fixedDeltaTime;
				_displayTransform.rotation = Quaternion.Lerp (_displayTransform.rotation, Quaternion.Euler(new Vector3(targetRotation.x, targetRotation.y, 0f)), durationCount/rotationDuration);
				_cameraTransform.rotation = Quaternion.Lerp (_cameraTransform.rotation, Quaternion.Euler(new Vector3(0f, 0f, targetRotation.z)), durationCount/rotationDuration);
				//Debug.Log ("current moment is " + durationCount / rotationDuration);
			}

			yield return null;
		}	

		//Debug.Log("Reached "  + _displayTransform.rotation.eulerAngles);
		Destroy (this.GetComponent<SmoothDisplayRotator>());
	}

	#endregion

}
