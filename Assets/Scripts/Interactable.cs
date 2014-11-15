using UnityEngine;
using System.Collections;

/**
 * Script for anything we can click on in the game, stuff to look at as well as items to pick up and characters to talk to
 */
public class Interactable : MonoBehaviour {

	public bool pickUp;

	public bool justLook;
	public string lookDescription;

	public float interactDistance = 1; // probably the same for all objects, but can be changed here.

	private TextMesh hoverText;

	// Use this for initialization
	void Start () {
		hoverText = GameObject.Find("HoverText").GetComponentInChildren<TextMesh>() as TextMesh;
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
		hoverText.text = name;
	}

	void OnMouseExit () {
		hoverText.text = "";
	}

}
