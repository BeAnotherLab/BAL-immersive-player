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

	void Awake(){
		oscOut = GetComponent<OSCTransmitter> ();
	}
    // Start is called before the first frame update
    void Start()
    {
		
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	#endregion

	#region Private metods
	public void PauseMessage(){
		var message = new OSCMessage(audioAddress);
		message.AddValue(OSCValue.String("pause"));
		oscOut.Send(message);
	}

	public void PlayMessage(){
		var message = new OSCMessage(audioAddress);
		message.AddValue(OSCValue.String("play"));
		oscOut.Send(message);
	}

	public void ResumeMessage(){
		var message = new OSCMessage(audioAddress);
		message.AddValue(OSCValue.String("resume"));
		oscOut.Send(message);
	}

	public void StopMessage(){
		var message = new OSCMessage(audioAddress);
		message.AddValue(OSCValue.String("stop"));
		oscOut.Send(message);
	}

	public void LoadAudio(string audioname) {
		var message = new OSCMessage(audioAddress);
		message.AddValue(OSCValue.String("audioname "));
		message.AddValue (OSCValue.String(audioname));
		oscOut.Send(message);
	}

	#endregion
}
