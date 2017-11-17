using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFB;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class IntroSceneManager : MonoBehaviour {

	public static string videoPath;

	// Use this for initialization

	void Start () {

		XRSettings.showDeviceView = false;
		//XRSettings.enabled = false;
	}

	// Update is called once per frame
	void Update () {
		
	}

	public void onOpenFile () {
			WriteResult(StandaloneFileBrowser.OpenFilePanel("Open File", Application.dataPath, "", false));
	}

	public void onButton1 () {
		videoPath = "C:/Users/BeAnotherLab/Desktop/HannenFinal.mp4";
		//StartCoroutine (LoadDevice("Oculus"));
		SceneManager.LoadScene ("Narrative");
	}

	public void OnButton2() {
		videoPath = "C:/Users/BeAnotherLab/Desktop/PascalFinal.mp4";
		SceneManager.LoadScene ("Narrative");
	}

	public void OnButton3() {
		videoPath = "C:/Users/BeAnotherLab/Desktop/SittingTest1.mp4";
		SceneManager.LoadScene ("Narrative");
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
		//StartCoroutine (LoadDevice("Oculus"));
		SceneManager.LoadScene ("Narrative");
	}

	IEnumerator LoadDevice(string newDevice) {

		XRSettings.LoadDeviceByName(newDevice);

		yield return null;
		XRSettings.enabled = true;
		UnityEngine.XR.InputTracking.Recenter();
	}
		
}
