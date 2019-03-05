﻿using UnityEngine;
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
			if (Application.isEditor) {
				_videoPlayer.url = VideoPlayerSettings.videoPath;

			} 
			else {
				//There must be a better way to do this, VideoPlayer.url uses the _Data folder for references while in general ./ refers to the application path 
				string _videoPath = Application.dataPath + VideoPlayerSettings.videoPath;
				_videoPath = _videoPath.Replace ("/Immersive Player Desktop_Data.", "");
				_videoPlayer.url = _videoPath;
			}
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

		while (!_videoPlayer.isPrepared) {
			yield return null;
		}

		_videoPlayer.EnableAudioTrack (0, true);
		_videoPlayer.Play ();
		oscOut.Send ("play");
		isPlaying = true;

	}
	#endregion
}
