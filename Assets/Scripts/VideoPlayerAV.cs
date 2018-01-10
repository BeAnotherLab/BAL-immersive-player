using UnityEngine;
using System.Collections;
using RenderHeads.Media.AVProVideo;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class VideoPlayerAV : MonoBehaviour {
	 
	public MediaPlayer	_mediaPlayer;
	public GameObject loadingImage;
	public GameObject dome;

	private bool isPlaying = false;
	private bool isPaused = false;

	private float currentRotationX, currentRotationY;
	private string audioName;

	public Text currentTimeText;
	public OscOut oscOut;
	public OscIn oscIn;

	// Use this for initialization
	void Awake () {
		XRDevice.SetTrackingSpaceType (TrackingSpaceType.Stationary);
	}

	void Start () {

		if (!oscOut)
			oscOut = gameObject.AddComponent<OscOut> ();
		
		oscOut.Open (7000);
		audioName = IntroSceneManager.audioName;
		Debug.Log ("the audio name is " + audioName);


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
			oscOut.Send ("stop", 0);
			isPlaying = false;
		}

		if (Input.GetKeyDown ("escape")){
			oscOut.Send ("stop", 0);
			isPlaying = false;
			SceneManager.LoadScene ("Intro Scene");
		}

		if (Input.GetKeyDown ("space")) {


			if (isPlaying) {
				_mediaPlayer.Control.Pause ();
				isPlaying = false;

				if (!isPaused) {
					oscOut.Send ("pause", 1);
					isPaused = true;
				} else if (isPaused) {
					oscOut.Send ("pause", 0);
					isPaused = false;
				}
			}

			else if (!isPlaying) {
				_mediaPlayer.Control.Play ();
				oscOut.Send ("play", 1);
				isPlaying = true;
			}



		}

		if (_mediaPlayer.Control.IsFinished()) {
			SceneManager.LoadScene ("Intro Scene");
		}

		dome.transform.eulerAngles = new Vector3 (currentRotationX, currentRotationY, 0);
	}

	void OnDisable(){
		oscOut.Send ("play", 1);
		isPlaying = false;
	}

		
}
