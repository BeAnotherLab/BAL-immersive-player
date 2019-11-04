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
	public string libraryFolderName, assistantVideoFolderName; //name of the folder inside Assets to look for files.
	public string fileFormat;
	public Vector3 initialRotation;
	public Toggle toggle360Video, toggleVideoFlip, toggleAssistantVideo;

	public static bool is360, useAssistantVideo;
	public static string videoPath, assistantVideoPath;
	public static Vector3 initialTiltConfiguration;
	public static string instructionsAudioName;
	#endregion



	#region Monobehavior methods

	void Start ()
    {
        InitialSettings();

        //Creates a button for each file.
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
        Debug.Log("file path " + videoPath + " and file name " + _fileName);

        if (videoPath != null) {
			instructionsAudioName = _fileName;

            if(toggleVideoFlip.isOn == true)
                initialRotation = new Vector3(90, 0, 180);

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

    public void SwitchSphereMode()
    {
        is360 = toggle360Video.isOn;

        if (toggle360Video.isOn)
            PlayerPrefs.SetInt("is360", 1);
        else
            PlayerPrefs.SetInt("is360", 0);

    }

    public void SwitchRotationMode()
    {
        if (toggleVideoFlip.isOn)
            PlayerPrefs.SetInt("isFlipped", 1);
        else
            PlayerPrefs.SetInt("isFlipped", 0);
    }

    public void SwitchAssistantVideo()
    {

        useAssistantVideo = toggleAssistantVideo.isOn;

        if (toggleAssistantVideo.isOn)
            PlayerPrefs.SetInt("assistantVideo", 1);
        else
            PlayerPrefs.SetInt("assistantVideo", 0);
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

    private void InitialSettings()
    {
        if (PlayerPrefs.GetInt("is360") == 1)
            toggle360Video.isOn = true;
        else
            toggle360Video.isOn = false;

        if (PlayerPrefs.GetInt("isFlipped") == 1)
            toggleVideoFlip.isOn = true;
        else
            toggleVideoFlip.isOn = false;

        if (PlayerPrefs.GetInt("assistantVideo") == 1)
            toggleAssistantVideo.isOn = true;
        else
            toggleAssistantVideo.isOn = false;

        is360 = toggle360Video.isOn;
        useAssistantVideo = toggleAssistantVideo.isOn;

        libraryFolderName = "./" + libraryFolderName + "/";
        assistantVideoFolderName = "./" + assistantVideoFolderName + "/";

    }

	private void ButtonBehavior(string _fileName){

		videoPath = libraryFolderName + _fileName + ".mp4";
		assistantVideoPath = assistantVideoFolderName + _fileName + ".mp4";

        if (toggleVideoFlip.isOn == true)
            initialRotation = new Vector3(90, 0, 180);

        initialTiltConfiguration = initialRotation;
		LoadVideoScene ();

        //VideoPlayer.url in ImmersiveVideoPLayer uses the _Data folder for references in standalone, while in general./ refers to the application path
        if (!Application.isEditor)
        {
                videoPath = Application.dataPath + videoPath;
                videoPath = videoPath.Replace("/Immersive Player Desktop_Data.", "");
        }
    }	


	private void LoadVideoScene(){
        SceneManager.LoadScene("Narrative");
	}

	#endregion
		
}
