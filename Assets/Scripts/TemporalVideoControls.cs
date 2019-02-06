using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class TemporalVideoControls : MonoBehaviour
{

	public Text elapsedTimeText;
	public Slider timeSlider;

	private ImmersiveVideoPlayer _videoPlayer;
	private bool sliderIsinteracting;
	private VideoPlayer vp;

    void Awake()
    {
		_videoPlayer = (ImmersiveVideoPlayer)FindObjectOfType(typeof(ImmersiveVideoPlayer));
		vp = (VideoPlayer)FindObjectOfType (typeof(VideoPlayer));
    }

	void Start(){
		sliderIsinteracting = false;
	}

    // Update is called once per frame
    void Update()
    {
		if (_videoPlayer.isPlaying)
			elapsedTimeText.text = _videoPlayer.ElapsedTime ().ToString("F2") + " of " + _videoPlayer.TotalTime ();
		else
			elapsedTimeText.text = "...";

		if(!sliderIsinteracting)
			timeSlider.value = _videoPlayer.ElapsedTime() / _videoPlayer.TotalTime ();
    }

	public void OnSelect() {
		sliderIsinteracting = true;
	}

	public void OnDiselect(){
		int frameToGoTo = (int)(timeSlider.value * _videoPlayer.TotalFrames());
		_videoPlayer.GoToFrame (frameToGoTo);
		Debug.Log ("Going to frame " + frameToGoTo + " out of " + _videoPlayer.TotalFrames());
		StartCoroutine(WaitForFrame());
	}

	private IEnumerator WaitForFrame(){

		yield return new WaitForSeconds (1f);

		sliderIsinteracting = false;
	}
			
}
