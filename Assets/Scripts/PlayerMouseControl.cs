using UnityEngine;
using System.Collections;

public class PlayerMouseControl : MonoBehaviour {
	
	public	GameObject		walkTo;
	
	public	NavMeshAgent	character01;
	
	public 	Camera 	cameraA;
	
	public	float	rayDistance	=	25.00f;

	private Interactable currentTarget; // If this isn't null the player should move towards it and interact when close enough

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		// If there's a target, interact with it if we're close enough
		if(currentTarget != null){
			if(currentTarget.withinRange(character01.transform.position)){
				if(currentTarget.pickUp){
					// Move to inventory
					Debug.Log ("Moving " + currentTarget.name + " to inventory");
					currentTarget.gameObject.SetActive(false);
				} else {
					// Let the target do whatever it does when interacting (e.g. start conversation with npc?)
					currentTarget.Interact();
				}

				// Target reached, remove it
				currentTarget = null;
			}
		}

		// Handle click input
		Ray ray = cameraA.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		Debug.DrawRay(cameraA.transform.position, ray.direction * rayDistance, Color.red);

		if(Physics.Raycast(ray, out hit)){					
			// Is it something we can interact with?

			if (hit.transform.tag == "Interactable"){
				Interactable interactable = hit.transform.GetComponent("Interactable") as Interactable;

				if(interactable == null) Debug.LogError("Something has the Interactable tag, but not the script");

				if (Input.GetButtonDown ("Fire1")){
					if(interactable.justLook){
						// Don't move, just look at it
						Debug.Log ("I'm looking at " + interactable.name + " and it's " + interactable.lookDescription);
					} else {
						// Target and move towards it
						currentTarget = interactable;
						walkTo.transform.position	= hit.point;
						character01.destination 	= hit.point;
					}
				}
							
			} else if (Input.GetButtonDown ("Fire1")){
				// Clicked on 'nothing', clear target and walks towards it
				currentTarget = null;

				walkTo.transform.position	= hit.point;			
				character01.destination 	= hit.point;
			}

		}		
	}

	                         
}
