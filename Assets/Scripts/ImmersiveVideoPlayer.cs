using UnityEngine;
using System.Collections;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class ImmersiveVideoPlayer : MonoBehaviour {

	#region public variables

	[HideInInspector]
	public bool isPlaying = false;
	[HideInInspector]
	public bool isPaused = false;
	#endregion

	#region private variables
	private AudioOSCController oscOut;
	private GameObject _display;
	private VideoPlayer _videoPlayer;

	private float _currentRotationX, _currentRotationY;
	private string _audioName;
	#endregion


	#region monobehavior methods
	void Awake () {
		XRDevice.SetTrackingSpaceType (TrackingSpaceType.Stationary);
		_display = FindObjectOfType<DisplaySelector>().gameObject;
		oscOut = (AudioOSCController)FindObjectOfType(typeof(AudioOSCController));
	}

	void Start () {

		_videoPlayer = _display.GetComponent<DisplaySelector>().selectedDisplay.GetComponent<VideoPlayer>();
		_videoPlayer.playOnAwake = false;

		_audioName = VideoPlayerSettings.audioName;
		oscOut.SendOnAddress("audioname/", _audioName);

		if (VideoPlayerSettings.videoPath != null) {
			if (Application.isEditor) {
				_videoPlayer.url = VideoPlayerSettings.videoPath;

			} 
			else {
				//There must be a better way to do this, VideoPlayer.url uses the _Data folder for references while in general ./ refers to the application path 
				string _videoPath = Application.dataPath + VideoPlayerSettings.videoPath;
				Debug.Log (_videoPath);
				_videoPath = _videoPath.Replace ("/Immersive Player Desktop_Data.", "");
				Debug.Log (_videoPath);
				_videoPlayer.url = _videoPath;
			}
		}
		_videoPlayer.Prepare ();
		_videoPlayer.loopPointReached += EndReached;
		//Valve.VR.OpenVR.Compositor.SetTrackingSpace(Valve.VR.ETrackingUniverseOrigin.TrackingUniverseSeated);

		_display.transform.Rotate (VideoPlayerSettings.initialTiltConfiguration);
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

	public void UpdateProjectorTransform(float x, float y){
		_display.transform.Rotate(x, 0f, y, Space.Self);
	}

	public void PlayImmersiveContent(){
		StartCoroutine(PrepareToPlayImmersiveContent ());
	}

	public void GoToFrame(int frameToSeek){
		//_videoPlayer.Stop ();
		oscOut.Send ("stop");
		//Debug.Log ("Stoped Audio Player, seek is not supported");
		_videoPlayer.frame = frameToSeek;
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
		SceneManager.LoadScene ("Menu");
	}

	public bool ImmersiveContentIsReady(){
		return _videoPlayer.isPrepared;
	}

	#endregion

	#region Private methods
	private void EndReached(UnityEngine.Video.VideoPlayer vp){
		Debug.Log ("video is over");
		SceneManager.LoadScene ("Menu");
		oscOut.Send ("stop");
	}
		

	private IEnumerator PrepareToPlayImmersiveContent() {

		while (!_videoPlayer.isPrepared) {
			yield return null;
		}

		_videoPlayer.Play ();
		oscOut.Send ("play");
		isPlaying = true;

	}

	#endregion
}
