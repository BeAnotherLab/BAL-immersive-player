using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class TemporalVideoControls : MonoBehaviour
{
	#region Public variables
	[HideInInspector]
	public bool sliderIsinteracting;

	public static TemporalVideoControls instance;
    #endregion

    #region Public variables
    private bool useNativeVideoPlugin;
    #endregion


    #region Unity methods
    void Awake()
    {
		if (instance == null)
			instance = this;
    }

    // Update is called once per frame
    private void UpdateTemporalControls()//temporal fix
    {
		if (ImmersiveVideoPlayer.instance.isPlaying && !sliderIsinteracting)
			ImmersiveVideoUIController.instance.UpdateTimeText(ImmersiveVideoPlayer.instance.ElapsedTime ().ToString("F2") + " of " + ImmersiveVideoPlayer.instance.TotalTime ());

		if(!sliderIsinteracting)
			ImmersiveVideoUIController.instance.timeSlider.value = ImmersiveVideoPlayer.instance.ElapsedTime() / ImmersiveVideoPlayer.instance.TotalTime ();
    }
    #endregion

    #region Public methods
    public void SetNativePluginSettings(bool enableNativeVideoPlugin)
    {
        useNativeVideoPlugin = enableNativeVideoPlugin;
    }

    public void InitializeVideo()
    {
        sliderIsinteracting = false;
        StartCoroutine(SetVideoDuration());
        InvokeRepeating("UpdateTemporalControls", 0f, 0.5f);//temporal fix
    }

    public void OnSelect() {
		sliderIsinteracting = true;
	}


	public void OnStop(){
		ImmersiveVideoUIController.instance.timeSlider.value = 0f;
		ImmersiveVideoUIController.instance.UpdateTimeText( "0 of " + ImmersiveVideoPlayer.instance.TotalTime ());
	}

	public void OnDiselect(){
		int frameToGoTo = (int)(ImmersiveVideoUIController.instance.timeSlider.value * ImmersiveVideoPlayer.instance.TotalFrames());
		ImmersiveVideoPlayer.instance.GoToFrame (frameToGoTo);
		StartCoroutine(WaitForFrame());
	}
	#endregion

	#region Private methods
	private IEnumerator WaitForFrame(){

		yield return new WaitForSeconds (0.5f);
		sliderIsinteracting = false;
	}

	private IEnumerator SetVideoDuration(){

        if(useNativeVideoPlugin)
		    while (!ImmersiveVideoPlayer.instance.ImmersiveContentIsReady ()) {
			    yield return null;
			    ImmersiveVideoUIController.instance.UpdateTimeText("...");
		    }

		ImmersiveVideoUIController.instance.UpdateTimeText( "0 of " + ImmersiveVideoPlayer.instance.TotalTime ());
	}
	#endregion
			
}
