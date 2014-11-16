﻿using UnityEngine;
using System.Collections;

/**
 * Script for anything we can click on in the game, stuff to look at as well as items to pick up and characters to talk to
 */
public class InteractableRev : MonoBehaviour {

	public bool pickUp;

	public bool justLook;
	public string lookDescription;

	public float interactDistance = 1; // probably the same for all objects, but can be changed here. // Rev: Collision sphere? See below

	private TextMesh actionLine; // Rev: Happy to rename this - it's the constructed sentence that describes what left-clicking will do. =)
	private TextMesh actionLineShadow;

	private TextMesh sayChar;
	private TextMesh sayCharShadow;

	private PlayerMouseControl playMousCont;
	private Camera uiCamera;
	private Inventory inventory;

	public Vector3 invTargetRotation = new Vector3 (0.0f,0.0f,0.0f);
	public Vector3 invTargetScale = new Vector3(1.0f,1.0f,1.0f);
	public float invTargetYPos = 0.0f;

	// Use this for initialization
	void Start () {
		actionLine = GameObject.Find ("sayNeutral").GetComponent<TextMesh> ();
		actionLineShadow = GameObject.Find ("sayNeutralShadow").GetComponent<TextMesh> ();

		sayChar = GameObject.Find ("sayAbner").GetComponent<TextMesh> (); // Rev: NOTE! This will have to be changed when we get the Maja PC character!
		sayCharShadow = GameObject.Find ("sayAbnerShadow").GetComponent<TextMesh> (); // Rev: This too!

		playMousCont = GameObject.Find ("Main Camera").GetComponent<PlayerMouseControl> ();

		uiCamera = GameObject.Find("InventoryCamera").GetComponent<Camera>();
		inventory = GameObject.Find ("Inventory").GetComponent<Inventory>();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnMouseDown() {
		if (actionLine != null) {
			actionLine.text = null;
			actionLineShadow.text = null;
			playMousCont.CursorNormal();
		}
	}

	void OnMouseDrag() {
		collider.enabled = false;
		if(tag == "Inventory"){
			Vector3 worldPoint = uiCamera.ScreenToWorldPoint(Input.mousePosition);
			transform.position = new Vector3(worldPoint.x, worldPoint.y, 1);
		}
	}

	void OnMouseUp() {
		inventory.settleItems();
		collider.enabled = true;
	}

	void OnMouseOver () {
		if(actionLine != null) {
			actionLine.text = name;
			actionLineShadow.text = name;
			playMousCont.CursorHighlight();
		}
	}
	
	void OnMouseExit () {
		if (actionLine != null) {
			actionLine.text = null;
			actionLineShadow.text = null;
			playMousCont.CursorNormal();
		}
	}

	public void Interact(){
		if(actionLine != null) { // Rev: Attempt to shift debug text into sayChar 3Dtext
			sayChar.text = "Beep, boop. Interacting";
			sayCharShadow.text = "Beep, boop. Interacting";
			GameFlow.instance.ResetReadingTime();
		}
	}

	/**
	 * Check if this item matches with another item dropped on it, returning true if something should happen
	 */
	public bool checkItemMatch(InteractableRev otherItem){
		// How do we handle this? Individual scripts for each interactable?

		// Add code here for handling player 

		return true; // just pretend everything can do anything for now
	}

	public bool withinRange(Vector3 playerPosition){	// Kristian, does it make sense to replace this with a sphere collider test? More performant, visual.
		Vector3 thisPosition = new Vector3(transform.position.x, 0.0f, transform.position.z);
		playerPosition = new Vector3(playerPosition.x, 0.0f, playerPosition.z);
		return Vector3.Distance(thisPosition, playerPosition) <= interactDistance;
	}
}
