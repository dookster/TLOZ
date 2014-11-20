using UnityEngine;
using System.Collections;

public class PlayerMouseControl : MonoBehaviour {
	
	public	GameObject		walkTo;
	
	public	NavMeshAgent	character01;
	public  Interactable playerInteractable; // interactable 'dummy' item to pass to items when we're not dragging anything, lets them choose to be picked up etc.

	public 	Camera 	cameraA;
	public	Camera	inventoryCamera; // Rev: Should set this up in Start, GameObject.Find etc.
	
	public	float	rayDistance	=	25.00f;

	private bool	haveClickedInv = false;

	// Rev: Change InteractableRev to Interactable if everything seems stable
	[SerializeField] // Rev: Serialized in order for me to follow the behavior in the inspector.
	private Interactable	currentWorldTarget; // If this isn't null the player should move towards it and interact when close enough
	[SerializeField] 
	private Interactable currentItemToUse; // Item we're currently dragging around from the inventory
	private Inventory		inventory;

	public Texture2D cursorTextureNormal; // Rev: Initial attempt to set up custom cursor, could be useful for dynamic cursor
	public Texture2D cursorTextureHighlight;
	public CursorMode cursorMode = CursorMode.Auto;
	public Vector2 hotSpot;

	public Vector2 mouseScreenPos; // Rev: Left this public to easily make screen measurements from inspector

	private InventoryCamera inventoryCam;

	// Use this for initialization
	void Start () {
		inventory = GameObject.Find ("Inventory").GetComponent<Inventory> () as Inventory; // Rev: Why 'as Inventory'? Unfamiliar!

		hotSpot = new Vector2(cursorTextureNormal.width / 2,cursorTextureNormal.height / 2);
		Cursor.SetCursor (cursorTextureNormal, hotSpot, cursorMode); // Rev: More initial custom cursor code.

		inventoryCam = GameObject.Find ("InventoryCamera").GetComponent<InventoryCamera> ();

		currentItemToUse = playerInteractable;
	}
	
	// Update is called once per frame
	void Update () {

		// KT: Show the inventory when pointer is in top 1/8th of the screen

		if (Input.mousePosition.y < Screen.height - Screen.height / 8){
			inventoryCam.OpenInventory();
		}else if (Input.mousePosition.y > Screen.height / 8){
			inventoryCam.CloseInventory();
		}

		/*
		 * Clicking on stuff is ordered by tags. 
		 * 
		 * "Level": 		Static background stuff, walls, floors etc. Clicking on it will try to move the PC there
		 * "Interactable":	Stuff we can use, talk to or pick up 
		 * "Inventory": 	Stuff in the inventory, interactables that are picked up switch their tag to this (and back if put down of course)
		 * 
		 */ 

		// If there's a target, interact with it if we're close enough
		if(currentWorldTarget != null){
			if(currentWorldTarget.withinRange(character01.transform.position)){ // Rev: Sphere collider?
				// Let the target do whatever it does when interacting, picking up is handled by the items themselves
				currentWorldTarget.Interact(currentItemToUse);

				// Target reached, remove it from the target variable
				currentWorldTarget = null;
			}
		}

		
		// Handle click input
		Ray ray = cameraA.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		// Click input on UI
		Ray uiRay = inventoryCamera.ScreenPointToRay (Input.mousePosition);

		Debug.DrawRay(cameraA.transform.position, ray.direction * rayDistance, Color.red);

		// Clicking in the game view
		if(Physics.Raycast(ray, out hit)){
			// Interactables

			if (hit.transform.tag == "Interactable"){
				Interactable interactable = hit.transform.GetComponent("InteractableRev") as Interactable; // Rev: Is this casting just to be certain?

				if(interactable == null) Debug.LogError("Something has the Interactable tag, but not the script");

				if (Input.GetButtonDown ("Fire1")){
					currentItemToUse = playerInteractable;
					if(interactable.justLook){
						// Don't move, just look at it 
						// Rev: An issue here - sometimes we want the PC to APPROACH the interactable before saying something. Other times, we want them to walk and talk.
						Debug.Log ("I'm looking at " + interactable.name + " and it's " + interactable.lookDescription); // Rev: Should probably reduce this to description, for flexibility
					} else {
						// Target and move towards it
						currentWorldTarget = interactable;
						walkTo.transform.position	= hit.point;
						character01.destination 	= hit.point;
					}
				}

				// Dropping an item on something in the world
				if(Input.GetButtonUp("Fire1") && currentItemToUse != null){
					currentWorldTarget = interactable;
					walkTo.transform.position	= hit.point;
					character01.destination 	= hit.point;
					inventory.settleItemsWithoutAnimation();
				}
			}

			// Clicking UI stuff
			else if (Physics.Raycast (uiRay, out hit)){
				if(Input.GetButton ("Fire1") && hit.transform.tag == "Inventory" && !haveClickedInv){
					// Clicking something in the inventory?
					// KT: moving the item during click&drag is handled in the Interactable class, 
					//     here we handle interaction when dropping an item onto another

					currentItemToUse = hit.transform.gameObject.GetComponent<Interactable>();
					haveClickedInv = true;
				}
			} else if (Input.GetButtonDown ("Fire1")){
				// Clicked on 'nothing', clear target and walks towards it
				currentWorldTarget = null;
				currentItemToUse = playerInteractable;
				
				walkTo.transform.position	= hit.point;			
				character01.destination 	= hit.point;
			}
		}

		if (Input.GetButtonUp ("Fire1")){
			haveClickedInv = false;
			//currentItemToUse = null;
		}
	}

	public void CursorHighlight(){
		Cursor.SetCursor (cursorTextureHighlight, hotSpot, cursorMode); // Rev: More initial custom cursor code
	}

	public void CursorNormal() {
		Cursor.SetCursor (cursorTextureNormal, hotSpot, cursorMode); // Rev: More initial custom cursor code
	}
}
