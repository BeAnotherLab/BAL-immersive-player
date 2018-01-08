using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ButtonCreator : MonoBehaviour {

	public GameObject buttonPrefab;
	public Transform canvasParent;
	public string folderName; //name of the folder inside Assets to look for files.
	public string fileFormat; //format of files to look for. 

	private string path; 

	// Use this for initialization
	void Start () {

		path = Application.dataPath + "/" + folderName + "/";

			foreach (string file in System.IO.Directory.GetFiles(path, "*." + fileFormat)) {
			
				string fileName = Path.GetFileNameWithoutExtension (file);
				GameObject newButton = Instantiate (buttonPrefab, new Vector3 (0, 0, 0), Quaternion.identity);
				newButton.transform.SetParent (canvasParent, false);


				Text newButtonText = newButton.GetComponentInChildren<Text> () as Text;
				newButtonText.text = fileName;	

				Button buttonBehaviour = newButton.GetComponent<Button> ();
				buttonBehaviour.onClick.AddListener (() => Debug.Log (fileName));

		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
