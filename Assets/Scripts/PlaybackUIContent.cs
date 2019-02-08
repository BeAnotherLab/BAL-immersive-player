using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaybackUIContent : MonoBehaviour
{

	public Text rotationText;

	private DisplaySelector dpSelector;
	private Transform dpTransform;
	public Transform cameraTransform;

    // Start is called before the first frame update
    void Start()
    {
		dpSelector = (DisplaySelector)FindObjectOfType (typeof(DisplaySelector));
		dpTransform = dpSelector.selectedDisplay.transform;
    }

    // Update is called once per frame
    void Update()
    {
		rotationText.text = "Pitch: " + dpTransform.rotation.eulerAngles.x.ToString () +
		"\nYaw: " + dpTransform.rotation.eulerAngles.y.ToString () +
		"\nRoll: " + cameraTransform.eulerAngles.z.ToString ();
    }
}
