using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFB;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class IntroSceneManager : MonoBehaviour {

	public static string videoPath;

	public static Vector3 initialTiltConfiguration;
	public static Vector3 dynamicTilt1;
	public static Vector3 dynamicTilt2;
	public static Vector3 dynamicTilt3;

	public static int dynamicTiltTime1;
	public static int dynamicTiltTime2;
	public static int dynamicTiltTime3;

	public static string audioName;

	List<string> tiltConfigurations = new List<string>();


	// Use this for initialization

	void Start () {

		XRSettings.showDeviceView = false;
		//XRSettings.enabled = false;
		//LoadDevice("Oculus");

	}

	// Update is called once per frame
	void Update () {
		
	}

	public void onOpenFile () {
			WriteResult(StandaloneFileBrowser.OpenFilePanel("Open File", Application.dataPath, "", false));
	}

	public void onButton1 () {
		
		videoPath = "./Assets/StreamingAssets/" + "Jerry.mp4"; //settings are for Jona
		initialTiltConfiguration = new Vector3(90, 0, 0);//117, 0, 0
		audioName = "pigeons";
		Debug.Log (audioName);

		//StartCoroutine (LoadDevice("Oculus"));
		SceneManager.LoadScene ("Narrative");
	}

	public void OnButton2() {
		videoPath = "./Assets/StreamingAssets/" + "Jonah.mp4";//settings are for Jerry
		initialTiltConfiguration = new Vector3(90, 0, 0);

		dynamicTilt1 = new Vector3 (95, 0, 0); 
		dynamicTiltTime1 = 61800;

		audioName = "phobia";

		SceneManager.LoadScene ("Narrative");


	}

	public void OnButton3() {
		videoPath = "./Assets/StreamingAssets/" + "Jose.mp4";
		initialTiltConfiguration = new Vector3(-45, 0, 0);

		audioName = "ironing";

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
		//UnityEngine.XR.InputTracking.Recenter();
	}

	public void OnCsvRead() {
		
		tiltConfigurations = csvReader.csvList;

		for (int i = 0; i < tiltConfigurations.Count; i++) {
			string[] tiltAngles = tiltConfigurations[i].Split (' ');
			Debug.Log (tiltAngles[0] + tiltAngles[1] + tiltAngles [2]);
		}


	}
		
		
}
