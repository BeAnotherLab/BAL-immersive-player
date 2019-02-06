using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class KeyboardPlayerControls : MonoBehaviour
{
	#region private variables
	private ImmersiveVideoPlayer _videoPlayer;
	#endregion

	#region unity methods

	void Awake(){
		_videoPlayer = (ImmersiveVideoPlayer)FindObjectOfType(typeof(ImmersiveVideoPlayer));
	}

    // Update is called once per frame
    void Update()
    {

		//tilt adjustments
		if (Input.GetKey ("down")) 
			_videoPlayer.UpdateProjectorTransform(-0.25f, 0f);

		if (Input.GetKey("up"))
			_videoPlayer.UpdateProjectorTransform(0.25f, 0f);

		if (Input.GetKey("left"))
			_videoPlayer.UpdateProjectorTransform(0f, 0.25f);

		if (Input.GetKey("right"))
			_videoPlayer.UpdateProjectorTransform(0f, -0.25f);

		if (Input.GetKeyDown ("c"))
			UnityEngine.XR.InputTracking.Recenter ();


		if (Input.GetKeyDown ("return"))
			_videoPlayer.StopImmersiveContent ();


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
