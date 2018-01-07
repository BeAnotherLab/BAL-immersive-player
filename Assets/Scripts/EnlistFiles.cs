using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class EnlistFiles : MonoBehaviour {

	string path = "C:/Users/BeAnotherLab/Desktop/";

	//DirectoryInfo dir = new DirectoryInfo(myPath)

	// Use this for initialization
	void Start () {
		foreach (string file in System.IO.Directory.GetFiles(path)) {
			Debug.Log (file);
		}

			
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
