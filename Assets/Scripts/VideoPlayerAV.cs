using UnityEngine;
using System.Collections;
using RenderHeads.Media.AVProVideo;
using UnityEngine.VR;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class VideoPlayerAV : MonoBehaviour {
	 
	public MediaPlayer	_mediaPlayer;
	public GameObject loadingImage;
	private bool isPlaying = false;

	//public Vector3 rotationStep1, rotationStep2, rotationStep3;
	///public float rotationTime1, rotationTime2, rotationTime3;

	public GameObject dome;

	public Text currentTimeText;

	// Use this for initialization
	void Start () {

		isPlaying = false;

		if (IntroSceneManager.videoPath != null)
			_mediaPlayer.OpenVideoFromFile (MediaPlayer.FileLocation.AbsolutePathOrURL, IntroSceneManager.videoPath, false); // "C:/Users/BeAnotherLab/Desktop/SittingTest1.mp4"

		dome.transform.eulerAngles = IntroSceneManager.initialTiltConfiguration;

		currentTimeText.fontSize = 14;
		currentTimeText.color = Color.white;
	}

	// Update is called once per frame
	void Update () {

		//vecky = Vector3()
		currentTimeText.text = System.Math.Round((_mediaPlayer.Control.GetCurrentTimeMs()/1000), 2).ToString() + "  of  " + System.Math.Round((_mediaPlayer.Info.GetDurationMs()/1000),2).ToString();

		if (IntroSceneManager.dynamicTiltTime1 != 0) {

			if (_mediaPlayer.Control.GetCurrentTimeMs () >= IntroSceneManager.dynamicTiltTime1 && _mediaPlayer.Control.GetCurrentTimeMs () <= (IntroSceneManager.dynamicTiltTime1 + 50))
				dome.transform.eulerAngles = IntroSceneManager.dynamicTilt1;
		}

		if (IntroSceneManager.dynamicTiltTime2 != 0) {
			if (_mediaPlayer.Control.GetCurrentTimeMs () >= IntroSceneManager.dynamicTiltTime2 && _mediaPlayer.Control.GetCurrentTimeMs () <= (IntroSceneManager.dynamicTiltTime2 + 50))
				dome.transform.eulerAngles = IntroSceneManager.dynamicTilt2;
		}

		if (IntroSceneManager.dynamicTiltTime3 != 0) {
			if (_mediaPlayer.Control.GetCurrentTimeMs () >= IntroSceneManager.dynamicTiltTime3 && _mediaPlayer.Control.GetCurrentTimeMs () <= (IntroSceneManager.dynamicTiltTime3 + 50))
				dome.transform.eulerAngles = IntroSceneManager.dynamicTilt3;
		}

		if (IntroSceneManager.dynamicTiltTime4 != 0) {
			if (_mediaPlayer.Control.GetCurrentTimeMs () >= IntroSceneManager.dynamicTiltTime4 && _mediaPlayer.Control.GetCurrentTimeMs () <= (IntroSceneManager.dynamicTiltTime4 + 50))
				dome.transform.eulerAngles = IntroSceneManager.dynamicTilt4;
		}


		
		if(Input.GetKeyDown("c"))
			UnityEngine.XR.InputTracking.Recenter();

		if (Input.GetKeyDown ("return")){
			_mediaPlayer.Control.Stop ();
			_mediaPlayer.Control.Seek (0);
			isPlaying = false;
		}

		if (Input.GetKeyDown ("escape")){
			SceneManager.LoadScene ("Intro Scene");
		}

		if (Input.GetKeyDown ("space")) {

			if (!isPlaying) {
				_mediaPlayer.Control.Play ();
				isPlaying = true;
			}


			else if (isPlaying) {
				_mediaPlayer.Control.Pause ();
				isPlaying = false;
			}

		}

		if (_mediaPlayer.Control.IsFinished()) {
			SceneManager.LoadScene ("Intro Scene");
		}
	}


		
}
