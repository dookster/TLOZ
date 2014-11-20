using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Inventory : MonoBehaviour {

	public Transform anchor;
	public float itemDistance = 0.5f;
	public List<GameObject> items = new List<GameObject>();



	// Use this for initialization
	void Start () {



	}
	
	// Update is called once per frame
	void Update () {

	}

	public void addItem(GameObject item){
		item.transform.parent = anchor;
		InteractableRev interact = item.GetComponent<InteractableRev> (); // Rev: Cache the item's InteractableRev component for speed
		BoxCollider interactCollider = item.GetComponent<BoxCollider> (); // Rev: Cache the item's BoxColider component for speed
		item.transform.localPosition = new Vector3(0 + itemDistance * items.Count, interact.invTargetYPos, -1.0f);
		item.transform.localScale = interact.invTargetScale;
		item.transform.localEulerAngles = interact.invTargetRotation;
		if(interactCollider){ 									// Rev: If we've found a box collider for the item...
//			interactCollider.center = interact.invCollidCent; 	// Rev: ...apply new center and size settings in the InteractableRev to the collider.
//			interactCollider.size = interact.invCollidSize;
			Destroy(interactCollider);
			item.gameObject.AddComponent("BoxCollider");
		}
		item.layer = LayerMask.NameToLayer("UI");
		item.tag = "Inventory";
		items.Add(item);
		//settleItems();
	}

	public void removeItem(GameObject item){
		// Move/destroy item?
		item.SetActive(false);

		items.Remove(item);
		settleItems();
	}

	/**
	 * After adding, moving or removing an item, set all items into their place
	 */
	public void settleItems(){
		for(int n = 0 ; n < items.Count ; n++){
			float yPos = items[n].GetComponent<InteractableRev>().invTargetYPos; // Rev: Get customised y position
			iTween.MoveTo(items[n], iTween.Hash("x", 0 + itemDistance * n, "y", yPos, "z", -1.0f, "time", 0.5f, "islocal", true));
		}
	}

	public void settleItemsWithoutAnimation(){
		for(int n = 0 ; n < items.Count ; n++){
			float yPos = items[n].GetComponent<InteractableRev>().invTargetYPos;
			items[n].transform.localPosition = new Vector3(0 + itemDistance * n, yPos, -1.0f);
		}
	}

}
