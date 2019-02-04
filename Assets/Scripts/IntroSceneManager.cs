using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFB;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using System.IO;
using UnityEngine.UI;

public class IntroSceneManager : MonoBehaviour {

	#region public variables
	public GameObject buttonPrefab;
	public Transform canvasParent;
	public string folderName; //name of the folder inside Assets to look for files.
	public string fileFormat;
	public int verticalTiltInit;

	public static string videoPath;
	public static Vector3 initialTiltConfiguration;
	public static string audioName;
	#endregion


	#region private variables
	private string _path; 

	#endregion


	#region monobehavior methods
	// Use this for initialization
	void Start () {
		
		_path = "./" + folderName + "/";

		foreach (string file in System.IO.Directory.GetFiles(_path, "*." + fileFormat)) {
			CreateButtonWithBehavior (file);
		}

	}

	// Update is called once per frame
	void Update () {
		
	}

	#endregion


	#region public methods
	public void OnOpenFile () {
		
		WriteResult(StandaloneFileBrowser.OpenFilePanel("Open File", Application.dataPath, "", false));
		audioName = null;

		initialTiltConfiguration = new Vector3(-verticalTiltInit, 0, 0);

		SceneManager.LoadScene ("Narrative");
	}

	public void CreateButtonWithBehavior(string file){
		string fileName = Path.GetFileNameWithoutExtension (file);
		GameObject newButton = Instantiate (buttonPrefab, new Vector3 (0, 0, 0), Quaternion.identity);
		newButton.transform.SetParent (canvasParent, false);

		Text newButtonText = newButton.GetComponentInChildren<Text> () as Text;
		newButtonText.text = fileName;	

		Button buttonBehaviour = newButton.GetComponent<Button> ();

		buttonBehaviour.onClick.AddListener (() => { 
			videoPath = _path+fileName + ".mp4";
			audioName = fileName; 
			SceneManager.LoadScene("Narrative");});
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


#endregion
		
		
}
