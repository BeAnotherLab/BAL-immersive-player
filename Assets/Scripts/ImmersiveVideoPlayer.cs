using UnityEngine;
using System.Collections;
using RenderHeads.Media.AVProVideo;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class ImmersiveVideoPlayer : MonoBehaviour {

	#region public variables
	public MediaPlayer	_mediaPlayer;
	public GameObject loadingImage;
	public GameObject dome;
	public Text currentTimeText;
	#endregion

	#region private variables
	private bool _isPlaying = false;
	private bool _isPaused = false;
	private float _currentRotationX, _currentRotationY;
	private string _audioName;
	#endregion


	#region monobehavior methods
	// Use this for initialization
	void Awake () {
		XRDevice.SetTrackingSpaceType (TrackingSpaceType.Stationary);
	}

	void Start () {
		
		_audioName = IntroSceneManager.audioName;


		if (IntroSceneManager.videoPath != null)
			_mediaPlayer.OpenVideoFromFile (MediaPlayer.FileLocation.AbsolutePathOrURL, IntroSceneManager.videoPath, false); // "C:/Users/BeAnotherLab/Desktop/SittingTest1.mp4"

		//Valve.VR.OpenVR.Compositor.SetTrackingSpace(Valve.VR.ETrackingUniverseOrigin.TrackingUniverseSeated);

		dome.transform.eulerAngles = IntroSceneManager.initialTiltConfiguration;
		_currentRotationX = IntroSceneManager.initialTiltConfiguration.x;
		_currentRotationY = IntroSceneManager.initialTiltConfiguration.y;

	}

	// Update is called once per frame
	void Update () {

		currentTimeText.text = System.Math.Round((_mediaPlayer.Control.GetCurrentTimeMs()/1000), 2).ToString() + "  of  " 
			+ System.Math.Round((_mediaPlayer.Info.GetDurationMs()/1000),2).ToString();



		if (Input.GetKey("down"))
			_currentRotationX = _currentRotationX -0.25f;
		

		if (Input.GetKey("up"))
			_currentRotationX = _currentRotationX +0.25f;
		

		if (Input.GetKey("left"))
			_currentRotationY = _currentRotationY +0.25f;
		

		if (Input.GetKey("right"))
			_currentRotationY = _currentRotationY -0.25f;
		
		
		if (Input.GetKeyDown ("c")) {
			UnityEngine.XR.InputTracking.Recenter ();
			//Valve.VR.OpenVR.System.ResetSeatedZeroPose ();
		} 

		if (Input.GetKeyDown ("return")){
			_mediaPlayer.Control.Stop ();
			_mediaPlayer.Control.Seek (0);
			//oscOut.Send ("stop", 0);
			_isPlaying = false;
		}

		if (Input.GetKeyDown ("escape")){
			//oscOut.Send ("stop", 0);
			_isPlaying = false;
			SceneManager.LoadScene ("Intro Scene");
		}

		if (Input.GetKeyDown ("space")) {


			if (_isPlaying) {


				if (!_isPaused) {
					_mediaPlayer.Control.Pause ();
					//oscOut.Send ("pause", 1);
					_isPaused = true;
				} 

				else if (_isPaused) {
					_mediaPlayer.Control.Play ();
					//oscOut.Send ("pause", 0);
					_isPaused = false;
				}
					
			}

			else if (!_isPlaying) {
				_mediaPlayer.Control.Play ();
				//oscOut.Send ("play", 1);
				_isPlaying = true;
			}



		}


		if (_mediaPlayer.Control.IsFinished()) {
			_mediaPlayer.Control.Stop ();
		}

		dome.transform.eulerAngles = new Vector3 (_currentRotationX, _currentRotationY, 0);
	}

	void OnDisable(){
		//oscOut.Send ("stop", 0);
		_isPlaying = false;
	}
	#endregion

		
}
