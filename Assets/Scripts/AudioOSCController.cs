using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;

public class AudioOSCController : MonoBehaviour
{
    #region Private Vars
    private OSCTransmitter oscOut;
    private string audioAddress = "/audio/";
    #endregion

    #region Unity Methods
    void Awake() {
        oscOut = GetComponent<OSCTransmitter>();
    }

    // Not sending osc messages, but should find a solution to send 'stop' on quit.
    void OnApplicationQuit() {

    }
    #endregion

    #region Private metods
    private void Send(string m)
    {
        var message = new OSCMessage(audioAddress);
        message.AddValue(OSCValue.String(m));
        oscOut.Send(message);
    }

    private void SendOnAddress(string address, string m)
    {
        var message = new OSCMessage(audioAddress);
        message.AddValue(OSCValue.String(address));
        message.AddValue(OSCValue.String(m));
        oscOut.Send(message);
    }
    #endregion

    #region Public metods

    public void SetAssistantAudioPath(string _path)
    {
        if(_path != null)
            SendOnAddress("audioname/", _path);
        else
            SendOnAddress("audioname/", "no audio found");
    }

    public void PlayImmersiveContent()
    {
        Send("play");
    }

    public void StopImmersiveContent()
    {
        Send("stop");
    }

    public void PauseImmersiveContent()
    {
        Send("pause");
    }

    public void ResumeImmersiveContent()
    {
        Send("resume");
    }

        #endregion
    }
