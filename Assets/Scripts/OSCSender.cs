/* 
 * 3D sound template for use with Brian Carty's Csound opcodes via OSC
 * Developed by Marte Roel, 2013
 * marteroel@gmail.com
 * */

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Scripts/OSCSender")]

/// <summary>
/// Simple OSC test communication script
/// </summary>

public class OSCSender : MonoBehaviour
{	
    private Osc oscHandler;

	public string remoteIp;
    public int sendToPort;

	public string audioNumber;

	private bool isPlaying = false;


    ~OSCSender()
    {
        if (oscHandler != null)
        {            
            oscHandler.Cancel();
        }
		
        // speed up finalization
        oscHandler = null;
        System.GC.Collect();
    }
	
	
	/// <summary>
    /// Start is called just before any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        UDPPacketIO udp = GetComponent<UDPPacketIO>();
        udp.init(remoteIp, sendToPort, 0);
         
	    oscHandler = GetComponent<Osc>();
        oscHandler.init(udp);
		
    }
	
	
	void Update()
	{

		OscMessage audioSettings = null;

		if (Input.GetKeyDown ("space") && isPlaying)
			audioSettings = Osc.StringToOscMessage ("/" + audioNumber + " pause");

		if(Input.GetKeyDown("space") && !isPlaying)
			audioSettings = Osc.StringToOscMessage("/" + audioNumber + " play");

		oscHandler.Send (audioSettings);

		if (isPlaying)
			isPlaying = false;
		else if (!isPlaying)
			isPlaying = true;
	}


 


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {    
    }
	
	
	void OnDisable()//OnDisable()
    {					

		OscMessage stopScene = null;

		stopScene = Osc.StringToOscMessage ("/" + audioNumber + " stop");
		oscHandler.Send (stopScene);

        Debug.Log("closing OSC UDP socket in OnDisable");
        oscHandler.Cancel();
        oscHandler = null;
    }	
}