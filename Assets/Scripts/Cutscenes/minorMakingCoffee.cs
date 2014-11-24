using UnityEngine;
using System.Collections;

public class minorMakingCoffee : MonoBehaviour {

	// private AudioSource audSource;
	public	AudioClip[] audClips;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Brewing () {
		Debug.Log ("Brewing coffee...!");
		audio.clip = audClips [0];
		audio.Play ();
	}

	void Pouring () {
		Debug.Log ("Pouring coffee...");
		audio.clip = audClips [1];
		audio.Play ();
	}

	void TakeCupOfCoffee () {
		Debug.Log ("Taking cup...");
		audio.clip = audClips [2];
		audio.Play ();
	}
}
