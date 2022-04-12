using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using ScriptableObjectArchitecture;

public class KeyboardPlayerControls : MonoBehaviour
{


    #region private variables
    [SerializeField] private float rotationAcceleration;
    [SerializeField] private Vector3GameEvent transformProjector;
    [SerializeField] private GameEvent calibrateAllTransforms;
    
    #endregion

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
        if (Input.GetKey("down"))
            transformProjector.Raise(new Vector3(rotationAcceleration, 0f, 0f));

		if (Input.GetKey ("up"))
            transformProjector.Raise(new Vector3(-rotationAcceleration, 0f, 0f));

        if (Input.GetKey ("left"))
            transformProjector.Raise(new Vector3(0f, -rotationAcceleration, 0f));

        if (Input.GetKey ("right"))
            transformProjector.Raise(new Vector3(0f, rotationAcceleration, 0f));

        if (Input.GetKey ("x"))
            transformProjector.Raise(new Vector3(0f, 0f, rotationAcceleration));

        if (Input.GetKey ("z"))
            transformProjector.Raise(new Vector3(0f, 0f, -rotationAcceleration));

        if (Input.GetKeyDown ("c")) 
            calibrateAllTransforms.Raise();

		if (Input.GetKeyDown ("return")) {
			ImmersiveVideoPlayer.instance.StopImmersiveContent ();
			TemporalVideoControls.instance.OnStop ();
		}

		if (Input.GetKeyDown ("escape")){
			ImmersiveVideoPlayer.instance.StopImmersiveContent ();
			ImmersiveVideoPlayer.instance.ShowSelectionMenu ();
		}

		if (Input.GetKeyDown ("space")) {

			if (!ImmersiveVideoPlayer.instance.isPlaying) { 
				ImmersiveVideoPlayer.instance.PlayImmersiveContent ();
                Debug.Log("should now be playing");
            }

            else if (ImmersiveVideoPlayer.instance.isPlaying) {
				if (!ImmersiveVideoPlayer.instance.isPaused) 
					ImmersiveVideoPlayer.instance.PauseImmersiveContent ();

				else if (ImmersiveVideoPlayer.instance.isPaused)
					ImmersiveVideoPlayer.instance.ResumeImmersiveContent ();
			}

		}
    }
    #endregion
    
    public void UpdateProjectorTransform(string direction)
    {
        Debug.Log("listened to " + direction);
    }
}
