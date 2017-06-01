using UnityEngine;
using System.Collections;

public class BinauralAudioTrigger : MonoBehaviour {

    public AudioSource left;
    public AudioSource right;

    //private bool isPlaying = false;

	// Use this for initialization
	void Start () {
        AudioSource[] audios = GetComponents<AudioSource>();
        left = audios[0];
        right = audios[1];
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("space"))
        {

			Debug.Log ("should be playing now");
            //isPlaying = true;
            left.Play();
            right.Play();


        }

       /* if (Input.GetKeyDown("space") && isPlaying)
        {
            isPlaying = true;
        }*/
		
    }
}
