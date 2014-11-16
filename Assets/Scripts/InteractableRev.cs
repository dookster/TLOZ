using UnityEngine;
using System.Collections;

/**
 * Script for anything we can click on in the game, stuff to look at as well as items to pick up and characters to talk to
 */
public class InteractableRev : MonoBehaviour {

	public bool pickUp;

	public bool justLook;
	public string lookDescription;

	public float interactDistance = 1; // probably the same for all objects, but can be changed here. // Rev: Collision sphere? See below

	public TextMesh actionLine; // Rev: Happy to rename this - it's the constructed sentence that describes what left-clicking will do. =)
	public TextMesh actionLineShadow;

	public TextMesh sayChar; // Rev: Should probably make this private, hook up to PCMaja.
	public TextMesh sayCharShadow;

	// Use this for initialization
	void Start () {
		actionLine = GameObject.Find ("sayNeutral").GetComponent<TextMesh> ();
		actionLineShadow = GameObject.Find ("sayNeutralShadow").GetComponent<TextMesh> ();

		sayChar = GameObject.Find ("sayAbner").GetComponent<TextMesh> (); // Rev: NOTE! This will have to be changed when we get the Maja PC character!
		sayCharShadow = GameObject.Find ("sayAbnerShadow").GetComponent<TextMesh> (); // Rev: This too!
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnMouseOver () {
		if(actionLine != null) {
			actionLine.text = name;
			actionLineShadow.text = name;
		}
	}
	
	void OnMouseExit () {
		if (actionLine != null) {
			actionLine.text = null;
			actionLineShadow.text = null;
		}
	}

	public void Interact(){
		if(actionLine != null) { // Rev: Attempt to shift debug text into sayChar 3Dtext
			sayChar.text = "Beep, boop. Interacting";
			sayCharShadow.text = "Beep, boop. Interacting";
			GameFlow.instance.ResetReadingTime();
		}
	}

	public bool withinRange(Vector3 playerPosition){	// Kristian, does it make sense to replace this with a sphere collider test? More performant, visual.
		Vector3 thisPosition = new Vector3(transform.position.x, 0.0f, transform.position.z);
		playerPosition = new Vector3(playerPosition.x, 0.0f, playerPosition.z);
		return Vector3.Distance(thisPosition, playerPosition) <= interactDistance;
	}
}
