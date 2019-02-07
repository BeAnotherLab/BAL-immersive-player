using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class TemporalVideoControls : MonoBehaviour
{
	#region Public variables
	public Text elapsedTimeText;
	public Slider timeSlider;

	[HideInInspector]
	public bool sliderIsinteracting;
	#endregion

	#region Private vars
	private ImmersiveVideoPlayer _videoPlayer;
	private VideoPlayer vp;
	#endregion

	#region Unity methods
    void Awake()
    {
		_videoPlayer = (ImmersiveVideoPlayer)FindObjectOfType(typeof(ImmersiveVideoPlayer));
		vp = (VideoPlayer)FindObjectOfType (typeof(VideoPlayer));
    }

	void Start(){
		sliderIsinteracting = false;
		StartCoroutine (SetVideoDuration ());
	}

    // Update is called once per frame
    void Update()
    {
		if (_videoPlayer.isPlaying && !sliderIsinteracting)
			elapsedTimeText.text = _videoPlayer.ElapsedTime ().ToString("F2") + " of " + _videoPlayer.TotalTime ();

		if(!sliderIsinteracting)
			timeSlider.value = _videoPlayer.ElapsedTime() / _videoPlayer.TotalTime ();
    }
	#endregion

	#region Public methods
	public void OnSelect() {
		sliderIsinteracting = true;
	}

	public void OnDiselect(){
		int frameToGoTo = (int)(timeSlider.value * _videoPlayer.TotalFrames());
		_videoPlayer.GoToFrame (frameToGoTo);
		StartCoroutine(WaitForFrame());
	}
	#endregion


	#region Private methods
	private IEnumerator WaitForFrame(){

		yield return new WaitForSeconds (0.5f);

		sliderIsinteracting = false;
	}

	private IEnumerator SetVideoDuration(){
		while (!_videoPlayer.ImmersiveContentIsReady ()) {
			yield return null;
			elapsedTimeText.text = "...";
		}

		elapsedTimeText.text = "0 of " + _videoPlayer.TotalTime ();
	}
	#endregion
			
}
