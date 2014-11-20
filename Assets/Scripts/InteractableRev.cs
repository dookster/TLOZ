using UnityEngine;
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

	//private TextMesh sayChar;
	//private TextMesh sayCharShadow;

	private PlayerMouseControl playMousCont;
	private Camera uiCamera;
	private Inventory inventory;

	public float readingTime = 1000.0f;

	public Vector3 invTargetRotation = new Vector3 (0.0f,0.0f,0.0f);
	public Vector3 invTargetScale = new Vector3(1.0f,1.0f,1.0f);
	public float invTargetYPos = 0.0f;

	public Vector3 invCollidSize = new Vector3 (0.1f,0.1f,0.1f);
	public Vector3 invCollidCent = new Vector3 (0.0f, 0.0f, 0.0f);

	private bool allowDrag;

	// Conversation stuff (no reason we can't theoretically start a conversation with an object)
	public TextMesh sayNPC; 
	public TextMesh sayNPCShadow;

	public MatchEvent[] events;

	/**
	 * Class representing a single pairing of items. Each interactable holds a list of these
	 */
	[System.Serializable]
	public class MatchEvent {
		public string itemName;						// Name of Interactable to match with
		public string comment;						// What the player says
		public bool pickUp;							// pick up this item
		public bool destroyThis;					// destroy (remove) this item after this interaction
		public bool destroyOther;					// destroy (remove) the item used on this
		public GameObject newItemInInventory; 		// new item (prefab) to create in inventory, set to null if no item comes of it
		public DialogueConversation conversation;	// Conversation prefab to start
	}

	// Use this for initialization
	void Start () {
		actionLine = GameObject.Find ("sayNeutral").GetComponent<TextMesh> ();
		actionLineShadow = GameObject.Find ("sayNeutralShadow").GetComponent<TextMesh> ();

		playMousCont = GameObject.Find ("Main Camera").GetComponent<PlayerMouseControl> ();

		uiCamera = GameObject.Find("InventoryCamera").GetComponent<Camera>();
		inventory = GameObject.Find ("Inventory").GetComponent<Inventory>();
	}
	
	// Update is called once per frame
	void Update () {
		if(readingTime < GameFlow.instance.readingSpeed){
			readingTime += Time.deltaTime;
		}
		
		if(readingTime >= GameFlow.instance.readingSpeed){
			hideText();
		}

	}

	public void ResetReadingTime () {
		readingTime = 0.0f;
		//Debug.Log ("Resetting timer for reading text speed");
	}

	public bool isTalking(){
		return readingTime < GameFlow.instance.readingSpeed;
	}

	void OnMouseDown() {
		if(tag == "Inventory"){
			allowDrag = true;
		}
		if (actionLine != null) {
			actionLine.text = null;
			actionLineShadow.text = null;
			playMousCont.CursorNormal();
		}
	}

	void OnMouseDrag() {
		collider.enabled = false;
		if(tag == "Inventory" && allowDrag){
			Vector3 worldPoint = uiCamera.ScreenToWorldPoint(Input.mousePosition);
			transform.position = new Vector3(worldPoint.x, worldPoint.y, 1);
		}
	}

	void OnMouseUp() {
		allowDrag = false;
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
		// Send a broadcast for this interaction, even if nothing happens, we can use this for debugging and for making quick
		// reactions in any other object that may want to know.
		//
		// The event name is both item names combined
		Messenger<string>.Broadcast("event", (otherItem.name + name));


		// See if we have any events for this item
		foreach(MatchEvent matchEvent in events){
			if(matchEvent.itemName.Equals(otherItem.name)){
				// Player comment
				GameFlow.instance.playerSay(matchEvent.comment);

				// Picking items up
				if(matchEvent.pickUp) inventory.addItem(gameObject);

				// Destroying items
				if(matchEvent.destroyThis) {
					gameObject.SetActive(false); // we just deactivate items for now
				}
				if(matchEvent.destroyOther) {
					inventory.removeItem(otherItem.gameObject);
				}

				// New inventory item
				if(matchEvent.newItemInInventory != null){
					GameObject newItem = Instantiate(matchEvent.newItemInInventory) as GameObject;
					inventory.addItem(newItem);
				}

				if(matchEvent.conversation != null){
					//DialogueConversation conversation = Instantiate(matchEvent.conversation) as DialogueConversation;
					//conversation.startConversation(this);

					matchEvent.conversation.startConversation(this);
				}

				return; // stop looking once we find a match, shouldn't have several events for one item
			}
		}

		// We don't have any events for this item, do something generic
		GameFlow.instance.playerSay("Uh... Hm... Huh?");
	}

	public void say(string text){
		if(sayNPC == null) return;
		sayNPC.text = text;
		sayNPCShadow.text = text;
		ResetReadingTime();
	}

	private void hideText(){
		if(sayNPC == null) return;
		sayNPC.text = "";
		sayNPCShadow.text = "";
	}

	public bool withinRange(Vector3 playerPosition){	// Kristian, does it make sense to replace this with a sphere collider test? More performant, visual.
		Vector3 thisPosition = new Vector3(transform.position.x, 0.0f, transform.position.z);
		playerPosition = new Vector3(playerPosition.x, 0.0f, playerPosition.z);
		return Vector3.Distance(thisPosition, playerPosition) <= interactDistance;
	}
}
