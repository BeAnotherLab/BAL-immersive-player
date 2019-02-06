using UnityEngine;
using System.Collections;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class ImmersiveVideoPlayer : MonoBehaviour {

	#region public variables
	public VideoPlayer _videoPlayer;
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
	void Awake () {
		XRDevice.SetTrackingSpaceType (TrackingSpaceType.Stationary);
		//_videoPlayer = gameObject.GetComponent<VideoPlayer> ();
	}

	void Start () {

		_videoPlayer.loopPointReached += EndReached;
		_videoPlayer.playOnAwake = false;
		StartCoroutine (InitializeImmersiveContent ());

		_audioName = VideoPlayerSettings.audioName;
		oscOut.SendOnAddress("audioname/", _audioName);

		if (VideoPlayerSettings.videoPath != null)
			_videoPlayer.url = VideoPlayerSettings.videoPath;
		
		//Valve.VR.OpenVR.Compositor.SetTrackingSpace(Valve.VR.ETrackingUniverseOrigin.TrackingUniverseSeated);

		dome.transform.Rotate (VideoPlayerSettings.initialTiltConfiguration);
	}


	// Update is called once per frame
	void Update () {
		/*
		currentTimeText.text = System.Math.Round((_videoPlayer.Control.GetCurrentTimeMs()/1000), 2).ToString() + "  of  " 
			+ System.Math.Round((_videoPlayer.Info.GetDurationMs()/1000),2).ToString();*/

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
			_videoPlayer.Stop ();
			_videoPlayer.frame = 0;
			oscOut.Send("stop");
			_isPlaying = false;
		}

		if (Input.GetKeyDown ("escape")){
			oscOut.Send ("stop");
			_isPlaying = false;
			SceneManager.LoadScene ("Intro Scene");
		}

		if (Input.GetKeyDown ("space")) {


			if (!_isPlaying) {
				StartCoroutine (InitializeImmersiveContent ());
			}

			else if (_isPlaying) {


				if (!_isPaused) {
					_videoPlayer.Pause ();
					oscOut.Send("pause");
					_isPaused = true;
				} 

				else if (_isPaused) {
					_videoPlayer.Play ();
					oscOut.Send("resume");
					_isPaused = false;
				}

			}


		}
			
	}
		

	#endregion

	#region Public methods
	public void UpdateDomeTransform(float x, float y){
		dome.transform.Rotate(x, 0f, y, Space.Self);
	}
	#endregion

	#region Private methods
	private void EndReached(UnityEngine.Video.VideoPlayer vp){
		Debug.Log ("video is over");
		vp.Stop ();
		oscOut.Send ("stop");
	}

	private IEnumerator InitializeImmersiveContent(){
		
		_videoPlayer.Prepare ();
		while (!_videoPlayer.isPrepared) {
			yield return null;
		}
		_videoPlayer.Play ();
		oscOut.Send ("play");
		_isPlaying = true;
	}

	#endregion
}
