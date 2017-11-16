using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFB;
using UnityEngine.SceneManagement;

public class IntroSceneManager : MonoBehaviour {

	public static string videoPath;

	// Use this for initialization

	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		
	}

	public void onOpenFile () {
			WriteResult(StandaloneFileBrowser.OpenFilePanel("Open File", Application.dataPath, "", true));
	}

	public void WriteResult(string[] paths) {
		if (paths.Length == 0) {
			return;
		}

		videoPath = "";

		foreach (var p in paths) {
			videoPath += p; //videoPath += p + "\n";
		}
			

		videoPath = videoPath.Replace("\\", "/"); //changing \ slash to / slash

		Debug.Log (videoPath);
		SceneManager.LoadScene ("Narrative");
	}
		
}
