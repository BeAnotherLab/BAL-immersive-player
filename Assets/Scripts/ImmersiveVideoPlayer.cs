using UnityEngine;
using System.Collections;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.XR;
using RenderHeads.Media.AVProVideo;
using ScriptableObjectArchitecture;

public class ImmersiveVideoPlayer : MonoBehaviour {

	#region public variables

	public Transform cameraParentTransform;
	public AudioSource audioSource;

    public GameObject _display;

    [HideInInspector]
	public bool isPlaying = false;
	[HideInInspector]
	public bool isPaused = false;

	public static ImmersiveVideoPlayer instance;

    #endregion

    #region private variables

    private bool useNativeVideoPlugin, useAssistantVideo;
    private AudioOSCController oscOut;
    private Vector3 initialTransform;
	private VideoPlayer _videoPlayer;
    private VideoPlayer _assistantVideoPlayer;
    private MediaPlayer _mediaPlayer;
    [SerializeField] private BoolGameEvent _selectionMenuOn, _videoControlOff;


    private float _currentRotationX, _currentRotationY;
	private string _instructionsAudioName;

	#endregion


	#region monobehavior methods

	void Awake () {
		if (instance == null)
			instance = this;

		XRDevice.SetTrackingSpaceType (TrackingSpaceType.Stationary);
		oscOut = (AudioOSCController)FindObjectOfType(typeof(AudioOSCController));
	}

	void Update(){

	}

    #endregion

    #region Public methods
    
    public void SetNativeVideoPLuginSettings(bool enableNativeVideoPlugin)
    {
        useNativeVideoPlugin = enableNativeVideoPlugin;
    }

    public void SetAssistantVideoSettings(bool setAssistantVideo)
    {
        useAssistantVideo = setAssistantVideo;
    }

    public void InitializeVideo()  {

        if (useNativeVideoPlugin)
        { //Unity video player

            _videoPlayer = DisplaySelector.instance.selectedDisplay.GetComponent<VideoPlayer>();
            _videoPlayer.playOnAwake = false;

            _videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
            _videoPlayer.SetTargetAudioSource(0, audioSource);

            if (VideoPlayerSettings.videoPath != null)
                _videoPlayer.url = VideoPlayerSettings.videoPath;

            _videoPlayer.Prepare();
            _videoPlayer.loopPointReached += EndReached;
        }

        else
        { //Media player

            _mediaPlayer = _display.GetComponent<DisplaySelector>().selectedDisplay.GetComponent<MediaPlayer>();

            if (VideoPlayerSettings.videoPath != null)
                _mediaPlayer.OpenVideoFromFile(MediaPlayer.FileLocation.AbsolutePathOrURL, VideoPlayerSettings.videoPath, false);
        }

        _instructionsAudioName = VideoPlayerSettings.instructionsAudioName;
        oscOut.SendOnAddress("audioname/", _instructionsAudioName);

        if (useAssistantVideo)
        {
            _assistantVideoPlayer = AssistantVideoPlayer.instance.assistantVideoObject.GetComponent<VideoPlayer>();
            _assistantVideoPlayer.playOnAwake = false;
            _assistantVideoPlayer.audioOutputMode = VideoAudioOutputMode.Direct;
            //_assistantVideoPlayer.SetTargetAudioSource(0, audioSource);
            _assistantVideoPlayer.SetDirectAudioMute(0, true);

            if (VideoPlayerSettings.assistantVideoPath != null)
                _assistantVideoPlayer.url = VideoPlayerSettings.assistantVideoPath;

            _assistantVideoPlayer.Prepare();
            _assistantVideoPlayer.loopPointReached += EndReached;


        }

        CalibrateAllTransforms();
        // Valve.VR.OpenVR.Compositor.SetTrackingSpace(Valve.VR.ETrackingUniverseOrigin.TrackingUniverseSeated);
    }

    public int CurrentFrame () {
        if (useNativeVideoPlugin)
            return (int)_videoPlayer.frame;
        else
            return (int)_mediaPlayer.Control.GetCurrentTimeMs();
	}

	public int TotalFrames() {
        if (useNativeVideoPlugin)
            return (int) _videoPlayer.frameCount;
        else
            return (int)_mediaPlayer.Info.GetDurationMs();

    }

	public float ElapsedTime() {
        if (useNativeVideoPlugin)
            return (_videoPlayer.frame / _videoPlayer.frameRate);
        else
            return _mediaPlayer.Control.GetCurrentTimeMs()/1000;
    }

	public float TotalTime(){
        if (useNativeVideoPlugin)
            return (_videoPlayer.frameCount / _videoPlayer.frameRate);
        else
            return _mediaPlayer.Info.GetDurationMs()/1000;
	}

    public void SetInitialTransform(Vector3 rotation)
    {
        //initialTransform = rotation;
    }

	public void UpdateProjectorTransform(Vector3 rotation){// float pitch, float yaw, float roll
		//_display.transform.Rotate(pitch, 0f, 0f, Space.Self);
		//_display.transform.Rotate(0f, yaw, 0f, Space.World); 

		//cameraParentTransform.transform.rotation *= Quaternion.AngleAxis (roll, cameraParentTransform.GetChild (0).forward);

        _display.transform.Rotate(rotation, Space.Self);
        _display.transform.Rotate(rotation, Space.World);

        cameraParentTransform.transform.rotation *= Quaternion.AngleAxis(rotation.z, cameraParentTransform.GetChild(0).forward);
    }

	public void PlayImmersiveContent(){
		StartCoroutine(PrepareToPlayImmersiveContent ());
	}

	public void GoToFrame(int frameToSeek){
		oscOut.Send ("stop");

        Debug.Log ("Stoped assistant Audio Player, seek is not supported");

        if (useNativeVideoPlugin)
            _videoPlayer.frame = frameToSeek;

        else
            _mediaPlayer.Control.Seek(frameToSeek);
	}

	public void StopImmersiveContent(){
        if (useNativeVideoPlugin) {
            _videoPlayer.frame = 0;
            _videoPlayer.Pause();
        }

        else {
            _mediaPlayer.Stop();
            _mediaPlayer.Control.Seek(0f);
             }

        oscOut.Send("stop");
		isPlaying = false;



        if (useAssistantVideo)
            _assistantVideoPlayer.Pause();
	}

	public void PauseImmersiveContent(){
        if (useNativeVideoPlugin)
            _videoPlayer.Pause ();
        else
            _mediaPlayer.Pause();

        oscOut.Send("pause");
		isPaused = true;

        if (useAssistantVideo)
            _assistantVideoPlayer.Pause();
    }

	public void ResumeImmersiveContent(){
        if(useNativeVideoPlugin)
            _videoPlayer.Play ();
        else
            _mediaPlayer.Play();

        oscOut.Send("resume");
		isPaused = false;

        if (useAssistantVideo)
            _assistantVideoPlayer.Play();
    }

	public void BackToMenu(){
        _selectionMenuOn.Raise(true);
        _videoControlOff.Raise(false);
    }

	public bool ImmersiveContentIsReady(){
		return _videoPlayer.isPrepared;
	}
		

	public void CalibrateAllTransforms(){
		UnityEngine.XR.InputTracking.Recenter ();
        _display.transform.rotation = Quaternion.Euler(initialTransform);//initialTransform.x, initialTransform.y, 0f);
        cameraParentTransform.transform.rotation = Quaternion.Euler (0f, 0f, initialTransform.z);
        Debug.Log ("the inital transform is " + initialTransform);
    }

    public void UpdateInitialTransform(Vector3 projectorTransform)
    {
        initialTransform = projectorTransform;
    }
	#endregion

	#region Private methods
	private void EndReached(UnityEngine.Video.VideoPlayer vp){
        _selectionMenuOn.Raise(true);
        _videoControlOff.Raise(false);
        oscOut.Send ("stop");
	}

    private IEnumerator PrepareToPlayImmersiveContent() {


        if (useAssistantVideo) {
            while (!_assistantVideoPlayer.isPrepared)
            {
                yield return null;
                Debug.Log("waiting for assistant video");
            }
        }       

        //else 
        if (useNativeVideoPlugin) { 
            while (!_videoPlayer.isPrepared) {
			        yield return null;
		        }

		    _videoPlayer.EnableAudioTrack (0, true);
		    _videoPlayer.Play ();
        }

        else
        {
            while (_mediaPlayer != null && _mediaPlayer.TextureProducer != null && _mediaPlayer.TextureProducer.GetTextureFrameCount() <= 0)
            {
                yield return null;
            }

            _mediaPlayer.Play();
        }

        oscOut.Send ("play");
		isPlaying = true;       

        if (useAssistantVideo)
           _assistantVideoPlayer.Play();

    }
	#endregion
}
