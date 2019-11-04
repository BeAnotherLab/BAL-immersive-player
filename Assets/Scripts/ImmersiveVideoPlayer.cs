using UnityEngine;
using System.Collections;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class ImmersiveVideoPlayer : MonoBehaviour {

	#region public variables

	public Transform cameraParentTransform;
	public AudioSource audioSource;

	[HideInInspector]
	public bool isPlaying = false;
	[HideInInspector]
	public bool isPaused = false;

	public static ImmersiveVideoPlayer instance;

	#endregion

	#region private variables

	private AudioOSCController oscOut;
	private GameObject _display;
	private VideoPlayer _videoPlayer;
    private VideoPlayer _assistantVideoPlayer;


	private float _currentRotationX, _currentRotationY;
	private string _instructionsAudioName;

	#endregion


	#region monobehavior methods

	void Awake () {
		if (instance == null)
			instance = this;

		XRDevice.SetTrackingSpaceType (TrackingSpaceType.Stationary);
		oscOut = (AudioOSCController)FindObjectOfType(typeof(AudioOSCController));
		_display = DisplaySelector.instance.gameObject;
	}

	void Start () {

		_videoPlayer = DisplaySelector.instance.selectedDisplay.GetComponent<VideoPlayer>();
		_videoPlayer.playOnAwake = false;

		_videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
		_videoPlayer.SetTargetAudioSource (0, audioSource);

		_instructionsAudioName = VideoPlayerSettings.instructionsAudioName;
		oscOut.SendOnAddress("audioname/", _instructionsAudioName);

		if (VideoPlayerSettings.videoPath != null) {
				_videoPlayer.url = VideoPlayerSettings.videoPath;
		}

        if (VideoPlayerSettings.useAssistantVideo)
        {
            _assistantVideoPlayer = AssistantVideoPlayer.instance.assistantVideoObject.GetComponent<VideoPlayer>();
            _assistantVideoPlayer.playOnAwake = false;
            //_assistantVideoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
            //_assistantVideoPlayer.SetTargetAudioSource(0, audioSource);

            if (VideoPlayerSettings.assistantVideoPath != null)
                _assistantVideoPlayer.url = VideoPlayerSettings.assistantVideoPath;
        }

        _videoPlayer.Prepare ();
		_videoPlayer.loopPointReached += EndReached;
		//Valve.VR.OpenVR.Compositor.SetTrackingSpace(Valve.VR.ETrackingUniverseOrigin.TrackingUniverseSeated);
        

	}

	void Update(){

	}

	#endregion

	#region Public methods

	public int CurrentFrame () {
		return (int)_videoPlayer.frame;
	}

	public int TotalFrames() {
		return (int) _videoPlayer.frameCount;
	}

	public float ElapsedTime() {
		return (_videoPlayer.frame / _videoPlayer.frameRate);
	}

	public float TotalTime(){
		return (_videoPlayer.frameCount / _videoPlayer.frameRate);
	}

	public void UpdateProjectorTransform(float pitch, float yaw, float roll){
		_display.transform.Rotate(pitch, 0f, 0f, Space.Self);
		_display.transform.Rotate(0f, yaw, 0f, Space.World); 

		cameraParentTransform.transform.rotation *= Quaternion.AngleAxis (roll, cameraParentTransform.GetChild (0).forward);
	}

	public void PlayImmersiveContent(){
		StartCoroutine(PrepareToPlayImmersiveContent ());
	}

	public void GoToFrame(int frameToSeek){
		oscOut.Send ("stop");
		Debug.Log ("Stoped assistant Audio Player, seek is not supported");
		_videoPlayer.frame = frameToSeek;
	}

	public void StopImmersiveContent(){
		_videoPlayer.frame = 0;
		_videoPlayer.Pause ();
		oscOut.Send("stop");
		isPlaying = false;

        if (VideoPlayerSettings.useAssistantVideo)
            _assistantVideoPlayer.Pause();
	}

	public void PauseImmersiveContent(){
		_videoPlayer.Pause ();
		oscOut.Send("pause");
		isPaused = true;

        if (VideoPlayerSettings.useAssistantVideo)
            _assistantVideoPlayer.Pause();
    }

	public void ResumeImmersiveContent(){
		_videoPlayer.Play ();
		oscOut.Send("resume");
		isPaused = false;

        if (VideoPlayerSettings.useAssistantVideo)
            _assistantVideoPlayer.Play();
    }

	public void BackToMenu(){
		Resources.UnloadUnusedAssets ();
		SceneManager.LoadScene ("Menu");
	}

	public bool ImmersiveContentIsReady(){
		return _videoPlayer.isPrepared;
	}
		

	public void CalibrateAllTransforms(){
		UnityEngine.XR.InputTracking.Recenter ();
		_display.transform.rotation = Quaternion.Euler(VideoPlayerSettings.initialTiltConfiguration.x, VideoPlayerSettings.initialTiltConfiguration.y, 0f);
		cameraParentTransform.transform.rotation = Quaternion.Euler (0f, 0f, VideoPlayerSettings.initialTiltConfiguration.z);
		//Debug.Log ("the inital transform is " + VideoPlayerSettings.initialTiltConfiguration);
	}

	#endregion

	#region Private methods
	private void EndReached(UnityEngine.Video.VideoPlayer vp){
		SceneManager.LoadScene ("Menu");
		oscOut.Send ("stop");
	}
		
	private IEnumerator PrepareToPlayImmersiveContent() {

        if (VideoPlayerSettings.useAssistantVideo)
            while (!_videoPlayer.isPrepared && !_assistantVideoPlayer.isPrepared)
            {
                yield return null;
            }

        else 
            while (!_videoPlayer.isPrepared) {
			    yield return null;
		    }

		_videoPlayer.EnableAudioTrack (0, true);
		_videoPlayer.Play ();
		oscOut.Send ("play");
		isPlaying = true;

        if (VideoPlayerSettings.useAssistantVideo)
            _assistantVideoPlayer.Play();

    }
	#endregion
}
