using UnityEngine;
using System.Collections;
using RenderHeads.Media.AVProVideo;
using UnityEngine.VR;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class VideoPlayerAV : MonoBehaviour {
	 
	public MediaPlayer	_mediaPlayer;
	public GameObject loadingImage;
	private bool isPlaying = false;

	//public Vector3 rotationStep1, rotationStep2, rotationStep3;
	///public float rotationTime1, rotationTime2, rotationTime3;

	public GameObject dome;

	private float currentRotationX;
	private float currentRotationY;

	public Text currentTimeText;

	// Use this for initialization
	void Start () {

		isPlaying = false;


		if (IntroSceneManager.videoPath != null)
			//_mediaPlayer.OpenVideoFromFile (MediaPlayer.FileLocation.AbsolutePathOrURL, "C:/Users/BeAnotherLab/Desktop/SittingTest1.mp4", false);
			_mediaPlayer.OpenVideoFromFile (MediaPlayer.FileLocation.AbsolutePathOrURL, IntroSceneManager.videoPath, false); // "C:/Users/BeAnotherLab/Desktop/SittingTest1.mp4"


		//Valve.VR.OpenVR.Compositor.SetTrackingSpace(Valve.VR.ETrackingUniverseOrigin.TrackingUniverseSeated);

		dome.transform.eulerAngles = IntroSceneManager.initialTiltConfiguration;
		currentRotationX = IntroSceneManager.initialTiltConfiguration.x;
		currentRotationY = IntroSceneManager.initialTiltConfiguration.y;

		currentTimeText.fontSize = 14;
		currentTimeText.color = Color.white;

	}

	// Update is called once per frame
	void Update () {

		currentTimeText.text = System.Math.Round((_mediaPlayer.Control.GetCurrentTimeMs()/1000), 2).ToString() + "  of  " + System.Math.Round((_mediaPlayer.Info.GetDurationMs()/1000),2).ToString();

		if (IntroSceneManager.dynamicTiltTime1 != 0) {

			if (_mediaPlayer.Control.GetCurrentTimeMs () >= IntroSceneManager.dynamicTiltTime1 && _mediaPlayer.Control.GetCurrentTimeMs () <= (IntroSceneManager.dynamicTiltTime1 + 50)) {
				dome.transform.eulerAngles = IntroSceneManager.dynamicTilt1;
				currentRotationX = IntroSceneManager.dynamicTilt1.x;
				currentRotationY = IntroSceneManager.dynamicTilt1.y;
			}
		}

		if (IntroSceneManager.dynamicTiltTime2 != 0) {
			if (_mediaPlayer.Control.GetCurrentTimeMs () >= IntroSceneManager.dynamicTiltTime2 && _mediaPlayer.Control.GetCurrentTimeMs () <= (IntroSceneManager.dynamicTiltTime2 + 50)) {
				dome.transform.eulerAngles = IntroSceneManager.dynamicTilt2;
				currentRotationX = IntroSceneManager.dynamicTilt1.x;
				currentRotationY = IntroSceneManager.dynamicTilt1.y;
			}
		}

		if (IntroSceneManager.dynamicTiltTime3 != 0) {
			if (_mediaPlayer.Control.GetCurrentTimeMs () >= IntroSceneManager.dynamicTiltTime3 && _mediaPlayer.Control.GetCurrentTimeMs () <= (IntroSceneManager.dynamicTiltTime3 + 50)) {
				dome.transform.eulerAngles = IntroSceneManager.dynamicTilt3;
				currentRotationX = IntroSceneManager.dynamicTilt1.x;
				currentRotationY = IntroSceneManager.dynamicTilt1.y;
			}
		}
			

		if (Input.GetKey("down"))
			currentRotationX = currentRotationX -0.25f;
		

		if (Input.GetKey("up"))
			currentRotationX = currentRotationX +0.25f;
		

		if (Input.GetKey("left"))
			currentRotationY = currentRotationY +0.25f;
		

		if (Input.GetKey("right"))
			currentRotationY = currentRotationY -0.25f;
		
		
		if (Input.GetKeyDown ("c")) {
			UnityEngine.XR.InputTracking.Recenter ();
			//Valve.VR.OpenVR.System.ResetSeatedZeroPose ();
		} 

		if (Input.GetKeyDown ("return")){
			_mediaPlayer.Control.Stop ();
			_mediaPlayer.Control.Seek (0);
			isPlaying = false;
		}

		if (Input.GetKeyDown ("escape")){
			SceneManager.LoadScene ("Intro Scene");
		}

		if (Input.GetKeyDown ("space")) {

			if (!isPlaying) {
				_mediaPlayer.Control.Play ();
				isPlaying = true;
			}


			else if (isPlaying) {
				_mediaPlayer.Control.Pause ();
				isPlaying = false;
			}

		}

		if (_mediaPlayer.Control.IsFinished()) {
			SceneManager.LoadScene ("Intro Scene");
		}

		dome.transform.eulerAngles = new Vector3 (currentRotationX, currentRotationY, 0);
	}


		
}
