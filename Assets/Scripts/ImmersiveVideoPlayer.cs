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

	public static ImmersiveVideoPlayer instance;
    public bool useNativeVideoPlugin;

    #endregion

    #region private variables

    private bool useAssistantVideo;
    private AudioOSCController oscOut;
    private Vector3 initialTransform;
	private VideoPlayer _videoPlayer;
    private VideoPlayer _assistantVideoPlayer;
    private MediaPlayer _mediaPlayer;
    [SerializeField] private BoolGameEvent selectionMenuOn, videoControlOn;
    [SerializeField] private GameEvent videoIsReady;
    [SerializeField] private BoolVariable isPlaying, isPaused;

    private float _currentRotationX, _currentRotationY;
	private string immersiveVideoPath, assistantVideoPath, assistantAudioPath;

	#endregion


	#region monobehavior methods

	void Awake () {
		//if (instance == null)
			//instance = this;
		XRDevice.SetTrackingSpaceType (TrackingSpaceType.Stationary);//maybe move elsewhere
		oscOut = (AudioOSCController)FindObjectOfType(typeof(AudioOSCController));//same
        isPlaying.Value = false;
        isPaused.Value = false;
    }

    #endregion

    #region Public methods

    public void SetImmersiveVideoPath(string _path)
    {
        immersiveVideoPath = _path;
    }

    public void SetAssistantVideoPath(string _path)
    {
        assistantVideoPath = _path;
    }

    public void SetAssistantAudioPath(string _path)
    {
        assistantAudioPath = _path;
    }


    public void SetNativeVideoPLuginSettings(bool enableNativeVideoPlugin)
    {
        useNativeVideoPlugin = enableNativeVideoPlugin;
    }

    public void SetAssistantVideoSettings(bool setAssistantVideo)
    {
        useAssistantVideo = setAssistantVideo;
    }

    public void InitializeVideo()  {

        if (useNativeVideoPlugin) InitializeNativeVideoPluginContent();
        else InitializeAVPlayerContent();

        oscOut.SendOnAddress("audioname/", assistantAudioPath);

        if (useAssistantVideo)
        {
            _assistantVideoPlayer = AssistantVideoPlayer.instance.assistantVideoObject.GetComponent<VideoPlayer>();
            _assistantVideoPlayer.playOnAwake = false;
            _assistantVideoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
            _assistantVideoPlayer.SetTargetAudioSource(0, audioSource);

            if (assistantVideoPath != null) { 
                _assistantVideoPlayer.url = assistantVideoPath;

                _assistantVideoPlayer.Prepare();
                _assistantVideoPlayer.loopPointReached += EndReached;
            }
        }

        CalibrateAllTransforms();
        // Valve.VR.OpenVR.Compositor.SetTrackingSpace(Valve.VR.ETrackingUniverseOrigin.TrackingUniverseSeated);
    }

    public void SetInitialTransform(Vector3 rotation)
    {
        //initialTransform = rotation;
    }

	public void UpdateProjectorTransform(Vector3 rotation){// float pitch, float yaw, float roll

        _display.transform.Rotate(rotation, Space.Self);
        _display.transform.Rotate(rotation, Space.World);

        cameraParentTransform.transform.rotation *= Quaternion.AngleAxis(rotation.z, cameraParentTransform.GetChild(0).forward);
    }

	public void PlayImmersiveContent(){
		StartCoroutine(PrepareToPlayImmersiveContent ());
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
		isPlaying.Value = false;

        if (useAssistantVideo)
        {
            _assistantVideoPlayer.frame = 0;
            _assistantVideoPlayer.Pause();
        }
    }

	public void PauseImmersiveContent(){
        if (useNativeVideoPlugin)
            _videoPlayer.Pause ();
        else
            _mediaPlayer.Pause();

        oscOut.Send("pause");
		isPaused.Value = true;

        if (useAssistantVideo)
            _assistantVideoPlayer.Pause();
    }

	public void ResumeImmersiveContent(){
        if(useNativeVideoPlugin)
            _videoPlayer.Play ();
        else
            _mediaPlayer.Play();

        oscOut.Send("resume");
		isPaused.Value = false;

        if (useAssistantVideo)
            _assistantVideoPlayer.Play();
    }

	public void HideSelectionMenu(){
        selectionMenuOn.Raise(false);
        videoControlOn.Raise(true);
    }

    public void ShowSelectionMenu()
    {
        selectionMenuOn.Raise(true);
        videoControlOn.Raise(false);
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

    private void InitializeNativeVideoPluginContent()//Unity video player
    {
        _videoPlayer = DisplaySelector.instance.selectedDisplay.GetComponent<VideoPlayer>();
        _videoPlayer.playOnAwake = false;

        _videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        _videoPlayer.SetTargetAudioSource(0, audioSource);

        if (immersiveVideoPath != null)
            _videoPlayer.url = immersiveVideoPath;

        _videoPlayer.Prepare();
        _videoPlayer.loopPointReached += EndReached;
    }

    private void InitializeAVPlayerContent()//AvPro Media Player     
    {
        _mediaPlayer = _display.GetComponent<DisplaySelector>().selectedDisplay.GetComponent<MediaPlayer>();

        if (immersiveVideoPath != null)
            _mediaPlayer.OpenVideoFromFile(MediaPlayer.FileLocation.AbsolutePathOrURL, immersiveVideoPath, false);
    }
    private void EndReached(UnityEngine.Video.VideoPlayer vp){
        ShowSelectionMenu();
        oscOut.Send ("stop");
	}

    private IEnumerator PrepareToPlayImmersiveContent() {

        if (useAssistantVideo) {
            while (!_assistantVideoPlayer.isPrepared)
            {
                yield return null;
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
		isPlaying.Value = true;
        videoIsReady.Raise();

        if (useAssistantVideo)
           _assistantVideoPlayer.Play();

    }
	#endregion
}
