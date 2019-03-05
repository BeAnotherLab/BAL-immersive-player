using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFB;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using System.IO;
using UnityEngine.UI;

public class VideoPlayerSettings : MonoBehaviour {

	#region public variables
	public GameObject buttonPrefab;
	public Transform canvasParent;
	public string libraryFolderName; //name of the folder inside Assets to look for files.
	public string fileFormat;
	public Vector3 initialRotation;
	public Toggle toggle360Video;

	public static bool is360;
	public static string videoPath;
	public static Vector3 initialTiltConfiguration;
	public static string instructionsAudioName;
	#endregion



	#region monobehavior methods
	// Use this for initialization
	void Start () {
		
        libraryFolderName = "./" + libraryFolderName + "/";

        foreach (string file in System.IO.Directory.GetFiles(libraryFolderName, "*." + fileFormat))
			CreateButton (file);

	}

	#endregion


	#region Public methods


	public void LoadFile () {

        string lastBrowsedDirectory;

        if (PlayerPrefs.GetString("lastBrowsedDirectory") != null)
            lastBrowsedDirectory = PlayerPrefs.GetString("lastBrowsedDirectory");
        else
            lastBrowsedDirectory = Application.dataPath;

        WriteResult(StandaloneFileBrowser.OpenFilePanel("Open File", lastBrowsedDirectory,"", false));
        PlayerPrefs.SetString("lastBrowsedDirectory", System.IO.Path.GetDirectoryName(videoPath));

        string _fileName = Path.GetFileNameWithoutExtension(videoPath);

        if (videoPath != null) {
			instructionsAudioName = _fileName;
			initialTiltConfiguration = initialRotation;		
			LoadVideoScene ();
		}
	}

	public void CreateButton(string file){
		
		string _fileName = Path.GetFileNameWithoutExtension (file);
		GameObject newButton = Instantiate (buttonPrefab, new Vector3 (0, 0, 0), Quaternion.identity);
		newButton.transform.SetParent (canvasParent, false);

		Text newButtonText = newButton.GetComponentInChildren<Text> () as Text;
		newButtonText.text = _fileName;	

		Button buttonBehaviour = newButton.GetComponent<Button> ();

		buttonBehaviour.onClick.AddListener (() => { ButtonBehavior(_fileName);});
	}


	public void WriteResult(string[] paths) {
		if (paths.Length == 0)
			return;

		videoPath = "";

		foreach (var p in paths) 
			videoPath += p; //videoPath += p + "\n";

		videoPath = videoPath.Replace("\\", "/"); //changing \ slash to / slash

	}
	#endregion


	#region Private methods
	private void ButtonBehavior(string _fileName){
		videoPath = libraryFolderName + _fileName + ".mp4";
		instructionsAudioName = _fileName; 
		initialTiltConfiguration = initialRotation;
		LoadVideoScene ();
	}	


	private void LoadVideoScene(){
		is360 = toggle360Video.isOn;
		SceneManager.LoadScene("Narrative");
	}

	#endregion
		
}
