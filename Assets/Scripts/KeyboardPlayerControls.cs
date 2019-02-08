using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class KeyboardPlayerControls : MonoBehaviour
{
	#region private variables
	private ImmersiveVideoPlayer _videoPlayer;
	private TemporalVideoControls _timeVideoControls;
	#endregion

	#region unity methods

	void Awake(){
		_videoPlayer = (ImmersiveVideoPlayer)FindObjectOfType(typeof(ImmersiveVideoPlayer));
		_timeVideoControls = (TemporalVideoControls)FindObjectOfType(typeof(TemporalVideoControls));
	}

    // Update is called once per frame
    void Update()
    {
		//dynamic rotation test
		if (Input.GetKeyDown ("r")) {
			SmoothDisplayRotator smoothy = (SmoothDisplayRotator) gameObject.AddComponent(typeof(SmoothDisplayRotator));
			Vector3 rotationTarget = new Vector3 (90, 90, 0);
			smoothy.InitializeSmoothRotation (rotationTarget, 0, 5);
		}

		//tilt adjustments
		if (Input.GetKey ("down"))
			_videoPlayer.UpdateProjectorTransform (0.25f, 0f, 0f);

		if (Input.GetKey ("up"))
			_videoPlayer.UpdateProjectorTransform (-0.25f, 0f, 0f);

		if (Input.GetKey ("left"))
			_videoPlayer.UpdateProjectorTransform (0f, 0.25f, 0f);

		if (Input.GetKey ("right"))
			_videoPlayer.UpdateProjectorTransform (0f, -0.25f, 0f);

		if (Input.GetKey ("x"))
			_videoPlayer.UpdateProjectorTransform (0f, 0f, 0.25f);

		if (Input.GetKey ("z"))
			_videoPlayer.UpdateProjectorTransform (0f, 0f, -0.25f);


		if (Input.GetKeyDown ("c")) {
			_videoPlayer.CalibrateAllTransforms ();
		}

		if (Input.GetKeyDown ("return")) {
			_videoPlayer.StopImmersiveContent ();
			_timeVideoControls.OnStop ();
		}

		if (Input.GetKeyDown ("escape")){
			_videoPlayer.StopImmersiveContent ();
			_videoPlayer.BackToMenu ();
		}

		if (Input.GetKeyDown ("space")) {

			if (!_videoPlayer.isPlaying) 
				_videoPlayer.PlayImmersiveContent ();

			else if (_videoPlayer.isPlaying) {

				if (!_videoPlayer.isPaused) 
					_videoPlayer.PauseImmersiveContent ();

				else if (_videoPlayer.isPaused)
					_videoPlayer.ResumeImmersiveContent ();
			}

		}
    }
	#endregion
}
