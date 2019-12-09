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

	// Not sending osc messages, but should find a solution to send 'stop' on quit.
	void OnApplicationQuit(){
		
	}
	#endregion

	#region Public metods

	public void Send(string m) {
		var message = new OSCMessage(audioAddress);
		message.AddValue(OSCValue.String(m));
		oscOut.Send(message);
	}

	public void SendOnAddress(string address, string m) {
		var message = new OSCMessage (audioAddress);
		message.AddValue (OSCValue.String (address));
		message.AddValue (OSCValue.String (m));
		oscOut.Send (message);
	}
	#endregion
}
