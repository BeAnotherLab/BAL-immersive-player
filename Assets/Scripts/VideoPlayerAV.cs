using UnityEngine;
using System.Collections;
using RenderHeads.Media.AVProVideo;
using UnityEngine.VR;
using UnityEngine.UI;

public class VideoPlayerAV : MonoBehaviour {
	 
	public MediaPlayer	_mediaPlayer;
	public GameObject loadingImage;
	private bool isPlaying = false;

	public GameObject dome;

	public Text currentTimeText;

	// Use this for initialization
	void Start () {
		isPlaying = false;
		InputTracking.Recenter();

		currentTimeText.fontSize = 14;
		currentTimeText.color = Color.white;
	}

	// Update is called once per frame
	void Update () {

		currentTimeText.text = (_mediaPlayer.Control.GetCurrentTimeMs()/1000).ToString() + " of " + (_mediaPlayer.Info.GetDurationMs()/1000).ToString();
			/*
		if (_mediaPlayer.Control.GetCurrentTimeMs () >= 3000) {
			dome.transform.Rotate(120, 150, 235);
			Debug.Log ("mi colita");
		}*/
		if(Input.GetKeyDown("c"))
			InputTracking.Recenter();

		if (Input.GetKeyDown ("return")){
			_mediaPlayer.Control.Stop ();
			_mediaPlayer.Control.Seek (0);
			isPlaying = false;
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
				//Application.LoadLevel (0);
		}

	}


}
