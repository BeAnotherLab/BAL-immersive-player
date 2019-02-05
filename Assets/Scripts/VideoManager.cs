using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;


public class VideoManager : MonoBehaviour {
	
	private VideoPlayer _videoPlayer;

	private bool isPlaying = false;


	void Awake() {
		_videoPlayer = gameObject.GetComponent<VideoPlayer> ();
	}

	// Use this for initialization
	void Start () {
		
		_videoPlayer.playOnAwake = false;
		_videoPlayer.url = ("./Narratives/Jonah.mp4");
		_videoPlayer.Play ();

	}

	// Update is called once per frame
	void Update () {

	}
}
