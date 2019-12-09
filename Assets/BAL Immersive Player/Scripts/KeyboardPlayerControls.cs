using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class KeyboardPlayerControls : MonoBehaviour
{


	#region unity methods

    // Update is called once per frame
    void Update()
    {

		//dynamic rotation test
		if (Input.GetKeyDown ("r")) {
			SmoothDisplayRotator smoothy = (SmoothDisplayRotator) gameObject.AddComponent(typeof(SmoothDisplayRotator));
			Vector3 rotationTarget = new Vector3 (90, 90, 0);
			smoothy.InitializeSmoothRotation (rotationTarget, 0, 1);
		}

		//tilt adjustments
		if (Input.GetKey ("down"))
			ImmersiveVideoPlayer.instance.UpdateProjectorTransform (0.25f, 0f, 0f);

		if (Input.GetKey ("up"))
			ImmersiveVideoPlayer.instance.UpdateProjectorTransform (-0.25f, 0f, 0f);

		if (Input.GetKey ("left"))
			ImmersiveVideoPlayer.instance.UpdateProjectorTransform (0f, 0.25f, 0f);

		if (Input.GetKey ("right"))
			ImmersiveVideoPlayer.instance.UpdateProjectorTransform (0f, -0.25f, 0f);

		if (Input.GetKey ("x"))
			ImmersiveVideoPlayer.instance.UpdateProjectorTransform (0f, 0f, 0.25f);

		if (Input.GetKey ("z"))
			ImmersiveVideoPlayer.instance.UpdateProjectorTransform (0f, 0f, -0.25f);


		if (Input.GetKeyDown ("c")) {
			ImmersiveVideoPlayer.instance.CalibrateAllTransforms ();
		}

		if (Input.GetKeyDown ("return")) {
			ImmersiveVideoPlayer.instance.StopImmersiveContent ();
			TemporalVideoControls.instance.OnStop ();
		}

		if (Input.GetKeyDown ("escape")){
			ImmersiveVideoPlayer.instance.StopImmersiveContent ();
			ImmersiveVideoPlayer.instance.BackToMenu ();
		}

		if (Input.GetKeyDown ("space")) {

			if (!ImmersiveVideoPlayer.instance.isPlaying) 
				ImmersiveVideoPlayer.instance.PlayImmersiveContent ();

			else if (ImmersiveVideoPlayer.instance.isPlaying) {

				if (!ImmersiveVideoPlayer.instance.isPaused) 
					ImmersiveVideoPlayer.instance.PauseImmersiveContent ();

				else if (ImmersiveVideoPlayer.instance.isPaused)
					ImmersiveVideoPlayer.instance.ResumeImmersiveContent ();
			}

		}
    }
	#endregion
}
