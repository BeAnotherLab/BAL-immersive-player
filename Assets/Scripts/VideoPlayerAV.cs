using UnityEngine;
using System.Collections;
using RenderHeads.Media.AVProVideo;
using UnityEngine.VR;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VideoPlayerAV : MonoBehaviour {
	 
	public MediaPlayer	_mediaPlayer;
	public GameObject loadingImage;
	private bool isPlaying = false;

	public Vector3 rotationStep1, rotationStep2, rotationStep3;
	public float rotationTime1, rotationTime2, rotationTime3;

	public GameObject dome;

	public Text currentTimeText;

	// Use this for initialization
	void Start () {
		isPlaying = false;
		UnityEngine.XR.InputTracking.Recenter();

		_mediaPlayer.OpenVideoFromFile (MediaPlayer.FileLocation.AbsolutePathOrURL, IntroSceneManager.videoPath, false); // "C:/Users/BeAnotherLab/Desktop/SittingTest1.mp4"
		currentTimeText.fontSize = 14;
		currentTimeText.color = Color.white;
	}

	// Update is called once per frame
	void Update () {

		//vecky = Vector3()
		currentTimeText.text = System.Math.Round((_mediaPlayer.Control.GetCurrentTimeMs()/1000), 2).ToString() + "  of  " + System.Math.Round((_mediaPlayer.Info.GetDurationMs()/1000),2).ToString();
			
		if (rotationStep1 != null) {
			if (_mediaPlayer.Control.GetCurrentTimeMs() >= rotationTime1 && _mediaPlayer.Control.GetCurrentTimeMs() <= (rotationTime1+50))
				dome.transform.eulerAngles = rotationStep1;
		}

		if (rotationStep2 != null) {
			if (_mediaPlayer.Control.GetCurrentTimeMs() >= rotationTime2 && _mediaPlayer.Control.GetCurrentTimeMs() <= (rotationTime2+50))
				dome.transform.eulerAngles = rotationStep2;
		}

		if (rotationStep3!= null) {
			if (_mediaPlayer.Control.GetCurrentTimeMs() >= rotationTime3 && _mediaPlayer.Control.GetCurrentTimeMs() <= (rotationTime3+50))
				dome.transform.eulerAngles = rotationStep3;
		}


		/*if (_mediaPlayer.Control.GetCurrentTimeMs() >= 3000 && _mediaPlayer.Control.GetCurrentTimeMs() <= 3200) {

			Vector3 mocos = new Vector3 (0, 0, 20);
			dome.transform.eulerAngles = mocos;
		
				Debug.Log ("se movio");
			}


		if (_mediaPlayer.Control.GetCurrentTimeMs () >= 5000 && _mediaPlayer.Control.GetCurrentTimeMs () <= 5050) {

			Vector3 mocos = new Vector3 (45, 100, 20);
			dome.transform.eulerAngles = mocos;

			Debug.Log ("se movio");
		}*/
		
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
