using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using ScriptableObjectArchitecture;

public class KeyboardPlayerControls : MonoBehaviour
{

    #region private variables
    private bool playbackAvailable;
    [SerializeField] private float rotationAcceleration;
    [SerializeField] private Vector3GameEvent transformProjector;
    [SerializeField] private GameEvent calibrateAllTransforms, stopPlayback, startUIPlayback, pausePlayback, resumePlayback;
    [SerializeField] private BoolGameEvent selectionMenuOn, videoControlOn;
    [SerializeField] private BoolVariable isPlaying, isPaused;
    #endregion

    #region unity methods

    // Update is called once per frame
    void Update()
    {
        if(playbackAvailable) { 
		    //dynamic rotation test
		    /*if (Input.GetKeyDown ("r")) {
			    SmoothDisplayRotator smoothy = (SmoothDisplayRotator) gameObject.AddComponent(typeof(SmoothDisplayRotator));
			    Vector3 rotationTarget = new Vector3 (90, 90, 0);
			    smoothy.InitializeSmoothRotation (rotationTarget, 0, 1);
		    }*/
            
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
                stopPlayback.Raise();
		    }

		    if (Input.GetKeyDown ("escape")){
                stopPlayback.Raise();
                selectionMenuOn.Raise(true);
                videoControlOn.Raise(false);
            }

		    if (Input.GetKeyDown ("space")) {
                if (!isPlaying.Value) {
                    startUIPlayback.Raise();
                }

                else {
                    if (!isPaused.Value)
                        pausePlayback.Raise();
                    else 
                        resumePlayback.Raise();
                    
                }

		    }
        }
    }
    #endregion

    #region Public methods
    /*
    public void UpdateProjectorTransform(string direction)
    {
        Debug.Log("listened to " + direction);
    }*/
    public void AvailKeyboardControls(bool availKeyboardPlayback)
    {
        if (availKeyboardPlayback)
            playbackAvailable = true;
        else
            playbackAvailable = false;
    }
    #endregion

}
