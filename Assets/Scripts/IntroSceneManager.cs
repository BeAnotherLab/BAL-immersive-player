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


	}

	// Update is called once per frame
	void Update () {
		
	}

	public void onOpenFile () {
		
		WriteResult(StandaloneFileBrowser.OpenFilePanel("Open File", Application.dataPath, "", false));
		audioName = null;

		initialTiltConfiguration = new Vector3(98, 0, 0);

		Debug.Log (videoPath);
		SceneManager.LoadScene ("Narrative");
	}

	public void onButton1 () {
		
		videoPath = "./StreamingAssets/" + "Jerry.mp4"; //settings are for Jona
		initialTiltConfiguration = new Vector3(98, 0, 0);//102, 0, 0

		dynamicTilt1 = new Vector3 (102, 0, 0); //106
		dynamicTiltTime1 = 88000;

		dynamicTilt2 = new Vector3 (108, 0, 0);///108
		dynamicTiltTime2 = 124000;

		dynamicTilt3 = new Vector3 (112, 0, 0);
		dynamicTiltTime3 = 178000;

		audioName = "pigeons";
		Debug.Log (audioName);

		SceneManager.LoadScene ("Narrative");
	}

	public void OnButton2() {
		videoPath = "./StreamingAssets/" + "Jonah.mp4";//settings are for Jerry

		initialTiltConfiguration = new Vector3(114, 0, 0);

		dynamicTilt1 = new Vector3 (119, 0, 0); 
		dynamicTiltTime1 = 61800;

		audioName = "phobia";

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

	}

	void OnDisable(){
		Debug.Log (videoPath);
	}
		

	/*
	public void OnCsvRead() {
		
		tiltConfigurations = csvReader.csvList;

		for (int i = 0; i < tiltConfigurations.Count; i++) {
			string[] tiltAngles = tiltConfigurations[i].Split (' ');
			Debug.Log (tiltAngles[0] + tiltAngles[1] + tiltAngles [2]);
		}


	}*/
		
		
}
