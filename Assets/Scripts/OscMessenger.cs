using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;

public class OscMessenger : MonoBehaviour {

	[Header("OSC Settings")]
	public OSCTransmitter oscOut;


	// Use this for initialization
	void Start () {

		if (!oscOut)
			oscOut = gameObject.AddComponent<OSCTransmitter> ();

		var message = new OSCMessage("audioName");
		//message.AddValue(OSCValue.String(audioName));
		oscOut.Send(message);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
