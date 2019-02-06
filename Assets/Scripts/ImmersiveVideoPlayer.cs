using UnityEngine;
using System.Collections;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class ImmersiveVideoPlayer : MonoBehaviour {

	#region public variables
	public GameObject dome;
	public Text currentTimeText;
	public AudioOSCController oscOut;

	[HideInInspector]
	public bool isPlaying = false;
	[HideInInspector]
	public bool isPaused = false;
	#endregion

	#region private variables
	private GameObject _display;
	private VideoPlayer _videoPlayer;

	private float _currentRotationX, _currentRotationY;
	private string _audioName;
	#endregion


	#region monobehavior methods
	void Awake () {
		XRDevice.SetTrackingSpaceType (TrackingSpaceType.Stationary);
		//_videoPlayer = gameObject.GetComponent<VideoPlayer> ();
		_display = FindObjectOfType<VideoDisplaySelector>().gameObject;
	}

	void Start () {

		_videoPlayer = _display.GetComponent<VideoDisplaySelector>().selectedDisplay.GetComponent<VideoPlayer>();

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

	#endregion

	#region Public methods
	public void UpdateProjectorTransform(float x, float y){
		dome.transform.Rotate(x, 0f, y, Space.Self);
	}

	public void PlayImmersiveContent(){
		StartCoroutine (InitializeImmersiveContent ());
		Debug.Log ("should start playing");
	}

	public void StopImmersiveContent(){
		_videoPlayer.Stop ();
		oscOut.Send("stop");
		isPlaying = false;
	}

	public void PauseImmersiveContent(){
		_videoPlayer.Pause ();
		oscOut.Send("pause");
		isPaused = true;
	}

	public void ResumeImmersiveContent(){
		_videoPlayer.Play ();
		oscOut.Send("resume");
		isPaused = false;
	}

	public void BackToMenu(){
		Resources.UnloadUnusedAssets ();
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
		isPlaying = true;
	}

	#endregion
}
