using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using RenderHeads.Media.AVProVideo;
using ScriptableObjectArchitecture;

public class ImmersiveVideoUIController : MonoBehaviour
{

	#region Public variables

	public Text rotationText;
	public Text elapsedTimeText;
	public Slider timeSlider;
    public DisplaySelector _displaySelector;
    public bool useNativeVideoPlugin = false;

	public static ImmersiveVideoUIController instance;

	[HideInInspector]
	public bool timeSliderIsInteracting;

	#endregion

	#region Private variables
	//private DisplaySelector _dpSelector;
	private Transform _dpTransform;
	private Transform _cameraTransform;
    private MediaPlayer mediaPlayer;
    private VideoPlayer videoPlayer;
    [SerializeField] private BoolVariable isPlaying, isPaused;

    #endregion

    #region Unity methods

    // Start is called before the first frame update
    void Start()
    {
        videoPlayer = _displaySelector.selectedDisplay.GetComponent<VideoPlayer>();
        mediaPlayer = _displaySelector.selectedDisplay.GetComponent<MediaPlayer>();
        _dpTransform = _displaySelector.selectedDisplay.transform;
		_cameraTransform = GameObject.FindGameObjectWithTag ("MainCamera").transform;
    }

    // Update is called once per frame
    void Update()
    {
		UpdateRotationText ();
    }
    
    private void UpdateTemporalControls()//temporal fix
    {
        if (isPlaying && !timeSliderIsInteracting)
            elapsedTimeText.text = ElapsedTime().ToString("F2") + " of " + TotalTime();

        if (!timeSliderIsInteracting)
            timeSlider.value = ElapsedTime() / TotalTime();
    }

    #endregion

    #region Private variables
    private float ElapsedTime()
    {
        if (useNativeVideoPlugin)
            return (videoPlayer.frame / videoPlayer.frameRate);
        else
            return mediaPlayer.Control.GetCurrentTimeMs() / 1000;
    }

    private float TotalTime()
    {
        if (useNativeVideoPlugin)
            return (videoPlayer.frameCount / videoPlayer.frameRate);
        else
            return mediaPlayer.Info.GetDurationMs() / 1000;
    }

    private int CurrentFrame()
    {
        if (useNativeVideoPlugin)
            return (int)videoPlayer.frame;
        else
            return (int)mediaPlayer.Control.GetCurrentTimeMs();
    }

    private int TotalFrames()
    {
        if (useNativeVideoPlugin)
            return (int)videoPlayer.frameCount;
        else
            return (int)mediaPlayer.Info.GetDurationMs();
    }
    #endregion

    #region Private methods

    private void UpdateRotationText(){
		rotationText.text = "Pitch: " + _dpTransform.rotation.eulerAngles.x.ToString () +
			"\nYaw: " + _dpTransform.rotation.eulerAngles.y.ToString () +
			"\nRoll: " + _cameraTransform.eulerAngles.z.ToString ();
	}

    #endregion

    #region Public methods

    public void InitializeVideoControls()
    {
        timeSliderIsInteracting = false;
        InvokeRepeating("UpdateTemporalControls", 0f, 0.2f);//temporal fix
    }

    public void OnSelect()
    {
        timeSliderIsInteracting = true;
    }


    public void OnStop()
    {
        timeSlider.value = 0f;

        if (useNativeVideoPlugin)
            videoPlayer.frame = 0;
        else
            mediaPlayer.Control.Seek(0);

        elapsedTimeText.text = "0 of " + TotalTime();
    }

    public void OnDiselect()
    {
        GoToFrame((int)(timeSlider.value* TotalFrames()));
        StartCoroutine(WaitForFrame());
    }
    #endregion

    #region Private methods
    private IEnumerator WaitForFrame()
    {
        yield return new WaitForSeconds(0.5f);
        timeSliderIsInteracting = false;
    }

    private void GoToFrame(int frameToSeek)
    {
        //oscOut.Send("stop");
        Debug.Log("Seek is not supported for assistant audio player");

        if (useNativeVideoPlugin)
            videoPlayer.frame = frameToSeek;
        else
            mediaPlayer.Control.Seek(frameToSeek);
    }

    #endregion
}
