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
	public AudioOSCController oscOut;
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
		oscOut.Send("audioname " + _audioName);

		if (IntroSceneManager.videoPath != null)
			_mediaPlayer.OpenVideoFromFile (MediaPlayer.FileLocation.AbsolutePathOrURL, IntroSceneManager.videoPath, false); // "C:/Users/BeAnotherLab/Desktop/SittingTest1.mp4"
		
		//Valve.VR.OpenVR.Compositor.SetTrackingSpace(Valve.VR.ETrackingUniverseOrigin.TrackingUniverseSeated);

		dome.transform.Rotate (IntroSceneManager.initialTiltConfiguration);//(90f,0f,0f,Space.Self);
	}


	// Update is called once per frame
	void Update () {

		currentTimeText.text = System.Math.Round((_mediaPlayer.Control.GetCurrentTimeMs()/1000), 2).ToString() + "  of  " 
			+ System.Math.Round((_mediaPlayer.Info.GetDurationMs()/1000),2).ToString();

		if (Input.GetKey ("down")) 
			UpdateDomeTransform(-0.25f, 0f);
		

		if (Input.GetKey("up"))
			UpdateDomeTransform(0.25f, 0f);
		

		if (Input.GetKey("left"))
			UpdateDomeTransform(0f, 0.25f);
		

		if (Input.GetKey("right"))
			UpdateDomeTransform(0f, -0.25f);
		
		
		if (Input.GetKeyDown ("c")) {
			UnityEngine.XR.InputTracking.Recenter ();
			//Valve.VR.OpenVR.System.ResetSeatedZeroPose ();
		} 

		if (Input.GetKeyDown ("return")){
			_mediaPlayer.Control.Stop ();
			_mediaPlayer.Control.Seek (0);
			oscOut.Send("stop");
			_isPlaying = false;
		}

		if (Input.GetKeyDown ("escape")){
			oscOut.Send ("stop");
			_isPlaying = false;
			SceneManager.LoadScene ("Intro Scene");
		}

		if (Input.GetKeyDown ("space")) {


			if (_isPlaying) {


				if (!_isPaused) {
					_mediaPlayer.Control.Pause ();
					oscOut.Send("pause");
					_isPaused = true;
				} 

				else if (_isPaused) {
					_mediaPlayer.Control.Play ();
					oscOut.Send("resume");
					_isPaused = false;
				}
					
			}

			else if (!_isPlaying) {
				_mediaPlayer.Control.Play ();
				oscOut.Send ("play");
				_isPlaying = true;
			}



		}
			
		if (_mediaPlayer.Control.IsFinished()) {
			_mediaPlayer.Control.Stop ();
			oscOut.Send ("stop");
		}
	}
		

	#endregion

	#region Public methods
	public void UpdateDomeTransform(float x, float y){
		dome.transform.Rotate(x, 0f, y, Space.Self);
	}
	#endregion
}
