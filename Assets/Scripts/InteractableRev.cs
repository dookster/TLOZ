﻿using UnityEngine;
using System.Collections;

/**
 * Script for anything we can click on in the game, stuff to look at as well as items to pick up and characters to talk to
 */
public class InteractableRev : MonoBehaviour {

//	public bool pickUp;

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

	public MatchEvent[] events;

	/**
	 * Class representing a single pairing of items. Each interactable holds a list of these
	 */
	[System.Serializable]
	public class MatchEvent {
		public string itemName;					// Name of Interactable to match with
		public string comment;					// What the player says
		public bool pickUp;						// pick up this item
		public bool destroyThis;				// destroy (remove) this item after this interaction
		public bool destroyOther;				// destroy (remove) the item used on this
		public GameObject newItemInInventory; 	// new item (prefab) to create in inventory, set to null if no item comes of it
	}

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

	/**
	 * Player tries to use something on this item. 
	 * 
	 * Look through all this item's events and see if we have a match.	
	 * 
	 */
	public void Interact(InteractableRev otherItem){
		// See if we have any events for this item
		foreach(MatchEvent matchEvent in events){
			if(matchEvent.itemName.Equals(otherItem.name)){
				playerSay(matchEvent.comment);

				if(matchEvent.pickUp) inventory.addItem(gameObject);

				if(matchEvent.destroyThis) {
					gameObject.SetActive(false); // we just deactivate items for now
				}
				if(matchEvent.destroyOther) {
					inventory.removeItem(otherItem.gameObject);
				}

				if(matchEvent.newItemInInventory != null){
					GameObject newItem = Instantiate(matchEvent.newItemInInventory) as GameObject;
					inventory.addItem(newItem);
				}

				return; // stop looking once we find a match, shouldn't have several events for one item
			}
		}

		// We don't have any events for this item, do something generic
		playerSay("Uh... Hm... Huh?");


//		if(pickUp && otherItem == null){ // just use null for basic interaction or some sort of dummy item?
//			inventory.addItem(gameObject);
//		}
//
//		if(actionLine != null) { // Rev: Attempt to shift debug text into sayChar 3Dtext
//			sayChar.text = "Beep, boop. Interacting";
//			sayCharShadow.text = "Beep, boop. Interacting";
//			GameFlow.instance.ResetReadingTime();
//		}
	}

	private void playerSay(string text){
		sayChar.text = text;
		sayCharShadow.text = text;
		GameFlow.instance.ResetReadingTime();
	}

	public bool withinRange(Vector3 playerPosition){	// Kristian, does it make sense to replace this with a sphere collider test? More performant, visual.
		Vector3 thisPosition = new Vector3(transform.position.x, 0.0f, transform.position.z);
		playerPosition = new Vector3(playerPosition.x, 0.0f, playerPosition.z);
		return Vector3.Distance(thisPosition, playerPosition) <= interactDistance;
	}
}
