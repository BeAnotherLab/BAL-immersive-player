﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
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
    public Toggle playToggle;
    public DisplaySettings _displaySelector;//TODO remove and adapt to more modular call
    public bool useNativeVideoPlugin = false;
    public Text noAssistantVideoLabel;

	public static ImmersiveVideoUIController instance;

	#endregion

	#region Private variables
	private Transform _dpTransform;
	private Transform _cameraTransform;
    private MediaPlayer mediaPlayer, assistantMediaPlayer;
    private VideoPlayer videoPlayer;
    private bool timeSliderIsInteracting, useAssistantVideo, isPlaying;
    private string assistantVideoPath;
    [SerializeField] private BoolGameEvent selectionMenuOn, VideoMenuOn;
    [SerializeField] private GameEvent stopPlayback, startPlayback, pausePlayback, resumePlayback;

    #endregion

    #region Unity methods
    #endregion

    #region Private variables
    private string TimeLabel()
    {
        string _elapsedInMinutes = TimeSpan.FromSeconds(ElapsedTime()).Minutes.ToString("00") + ":" +
            TimeSpan.FromSeconds(ElapsedTime()).Seconds.ToString("00") + ":" +
            TimeSpan.FromSeconds(ElapsedTime()).Milliseconds.ToString("000");

        string _totalInMinutes = TimeSpan.FromSeconds(TotalTime()).Minutes.ToString("00") + ":" +
            TimeSpan.FromSeconds(TotalTime()).Seconds.ToString("00") + ":" +
            TimeSpan.FromSeconds(TotalTime()).Milliseconds.ToString("000");

        return _elapsedInMinutes + " of " + _totalInMinutes;
    }

    private string RotationLabel()
    {
        return "Pitch: " + _dpTransform.rotation.eulerAngles.x.ToString() +
            "\nYaw: " + _dpTransform.rotation.eulerAngles.y.ToString() +
            "\nRoll: " + _cameraTransform.eulerAngles.z.ToString();
    }

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
    private void UpdateControls()
    {
        elapsedTimeText.text = TimeLabel();
        rotationText.text = RotationLabel();

        if (!timeSliderIsInteracting)
            timeSlider.value = ElapsedTime() / TotalTime();
    }

    private IEnumerator WaitToInitControls()
    {
        yield return new WaitForSeconds(0.2f);//wait a moment for video to be loaded, not best way
        elapsedTimeText.text = TimeLabel();
        rotationText.text = RotationLabel();

        if (!timeSliderIsInteracting)
            timeSlider.value = 0;
    }

    private IEnumerator WaitForFrame()
    {
        yield return new WaitForSeconds(0.5f);
        timeSliderIsInteracting = false;
    }

    private IEnumerator ShowPathErrorLabelCoroutine()
    {
        Color fadeColor = new Color(0.77f,0.76f,0.87f,1);
        float timeAtStart = 0;

        while (timeAtStart < 5)
        {
            timeAtStart += Time.deltaTime;
            fadeColor.a = 1-(timeAtStart / 5f);
            noAssistantVideoLabel.color = fadeColor;

            yield return null;
        }

    }

    private void GoToFrame(int frameToSeek)
    {
        Debug.Log("Seek is not supported for assistant audio player");

        if (useNativeVideoPlugin)
            videoPlayer.frame = frameToSeek;
        else
            mediaPlayer.Control.Seek(frameToSeek);

        if (useAssistantVideo && assistantVideoPath != null)
            assistantMediaPlayer.Control.Seek(frameToSeek);
    }
    #endregion

    #region Public methods

    public void SetAssistantVideoSettings(bool setAssistantVideo)
    {
        useAssistantVideo = setAssistantVideo;
    }

    public void StartVideoControlUpdate()
    {
        InvokeRepeating("UpdateControls", 0f, 0.1f);//temporal fix
    }

    public void OnSelectTimeSlider()
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

        playToggle.isOn = false;

        isPlaying = false;
    }

    public void OnStart()
    {
        playToggle.isOn = true;
    }

    public void CallStopEvent()
    {
        stopPlayback.Raise();     
    }

    public void CallPlayPauseEvent(bool toggleOn)
    {
        Image playImage = playToggle.image;

        if (toggleOn)
        {
            if(!isPlaying) { 
                playImage.color = new Color(0f, 0f, 0f, 0f);
                startPlayback.Raise();
                isPlaying = true;
            }

            else
            {
                playImage.color = new Color(0f, 0f, 0f, 0f);
                resumePlayback.Raise();
            }

        }
        else {
            playImage.color = new Color(1f, 1f, 1f, 1f);
            pausePlayback.Raise();
        }
    }

    public void ShowPathErrorLabel()
    {
        StartCoroutine(ShowPathErrorLabelCoroutine());
    }

    public void OnInitializeVideoUI()//TODO, adapt for modularity
    {
        videoPlayer = _displaySelector.selectedDisplay.GetComponent<VideoPlayer>();
        mediaPlayer = _displaySelector.selectedDisplay.GetComponent<MediaPlayer>();
        assistantMediaPlayer = _displaySelector.fullsphere.GetComponent<MediaPlayer>();
        _dpTransform = _displaySelector.selectedDisplay.transform;
        _cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;

        playToggle.isOn = false;

        StartCoroutine(WaitToInitControls());
    }

    public void OnQuitVideoUI()
    {
        CancelInvoke();
    }

    public void BackToSelectionMenu()
    {
        selectionMenuOn.Raise(true);
        VideoMenuOn.Raise(false);
    }

    public void OnDiselectTimeSlider()
    {
        GoToFrame((int)(timeSlider.value* TotalFrames()));
        StartCoroutine(WaitForFrame());
    }

    public void SetAssistantVideoPath(string _path)
    {
        assistantVideoPath = _path;
    }
    #endregion
}
