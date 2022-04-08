using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFB;
using UnityEngine.XR;
using System.IO;
using UnityEngine.UI;
using ScriptableObjectArchitecture;

public class VideoPlayerSettings : MonoBehaviour {

    #region public variables
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private Transform canvasParent;
    [SerializeField] private string libraryFolderName, assistantVideoFolderName; //name of the folder inside Assets to look for files.
    [SerializeField] private string fileFormat;
    [SerializeField] private Vector3 initialRotation;
    [SerializeField] private Toggle toggle360Video, toggleVideoFlip, toggleAssistantVideo, toggleNativeVideoPlugin, toggleStereo;
    [SerializeField] private GameEvent _loadVideo;
    [SerializeField] private BoolGameEvent _selectionMenuOn, _videoControlOff;

    //public BoolGameEvent enable360, enableAssistantVideo, enableNativeVideoPlugin, enableVideoFlip, enableStereo;
    public Vector3GameEvent initialProjectorRotation;
	public static string videoPath, assistantVideoPath;
	//public static Vector3 initialTiltConfiguration;
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
                
            LoadVideo ();
		}
	}

    public void InitialRotation(bool flip)
    {
        if (flip)
            initialProjectorRotation.Raise(new Vector3(90, 0, 180));
        else
            initialProjectorRotation.Raise(initialRotation);
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

    private void InitialSettings()
    {

        libraryFolderName = "./" + libraryFolderName + "/";
        assistantVideoFolderName = "./" + assistantVideoFolderName + "/";//add ./ before when not in standalone
    }

	private void ButtonBehavior(string _fileName){

        instructionsAudioName = _fileName;

        videoPath = libraryFolderName + _fileName + ".mp4";
		assistantVideoPath = assistantVideoFolderName + _fileName + ".mp4";

		LoadVideo ();

        //VideoPlayer.url in ImmersiveVideoPLayer uses the _Data folder for references in standalone, while in general./ refers to the application path
        if (!Application.isEditor)
        {
                videoPath = Application.dataPath + videoPath;
                videoPath = videoPath.Replace("/Immersive Player Desktop_Data.", "");
                assistantVideoPath = Application.dataPath + assistantVideoPath;
                assistantVideoPath = assistantVideoPath.Replace("/Immersive Player Desktop_Data.", "");
        }
    }

    private int BoolToInt(bool val)
    {
        if (val)
            return 1;
        else
            return 0;
    }

    private bool IntToBool(int val)
    { 
        if (val == 1)
            return true;
        else
            return false;
    }


    private void LoadVideo(){
        _selectionMenuOn.Raise(false);
        _videoControlOff.Raise(true);
        _loadVideo.Raise();  
	}

	#endregion
		
}
