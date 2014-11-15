using UnityEngine;
using System.Collections;

/**
 * Script for anything we can click on in the game, stuff to look at as well as items to pick up and characters to talk to
 */
public class InteractableRev : MonoBehaviour {

	public bool pickUp;

	public bool justLook;
	public string lookDescription;

	public float interactDistance = 1; // probably the same for all objects, but can be changed here.

	public TextMesh hoverText;
	public TextMesh hoverTextShadow;

	// Use this for initialization
	void Start () {
		// hoverText = GetComponentInChildren<TextMesh>() as TextMesh; // Yep, that's pretty fucking smart. =) Have to disable for my proposal, though...
		if(hoverText != null){
			hoverText.text = name; // THIS IS ALSO CLEVER! Nice one, Kristian!
			hoverTextShadow.text = name;
			hoverText.renderer.enabled = false;
			hoverTextShadow.renderer.enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void Interact(){
		Debug.Log ("Beep, boop. Interacting");
	}

	public bool withinRange(Vector3 playerPosition){
		Vector3 thisPosition = new Vector3(transform.position.x, 0, transform.position.z);
		playerPosition = new Vector3(playerPosition.x, 0, playerPosition.z);
		return Vector3.Distance(thisPosition, playerPosition) <= interactDistance;
	}

	void OnMouseOver () {
		if(hoverText != null) hoverText.renderer.enabled = true;
		if(hoverText != null) hoverTextShadow.renderer.enabled = true;
	}

	void OnMouseExit () {
		if(hoverText != null) hoverText.renderer.enabled = false;
		if(hoverText != null) hoverTextShadow.renderer.enabled = false;
	}

}
