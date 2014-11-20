using UnityEngine;
using System.Collections;

public class ConversationUI : MonoBehaviour {

	public Camera uiCamera;
	public TextMesh[] UILines;

	private DialogueConversation currentConversation;

	// Use this for initialization
	void Start () {
		uiCamera = GameObject.Find("actionLineCamera").GetComponent<Camera>() ;
	}
	
	// Update is called once per frame
	void Update () {
		Ray ray = uiCamera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		// All this can be handled better!

		if(Physics.Raycast(ray, out hit)){									// Rev: If the raycast hits something...
			if (hit.transform.name == "option") {							// ...and the object has a transform named 'option'...
				for(int n = 0 ; n < UILines.Length ; n++){ 					// ... go through the UILines array...
					if(UILines[n].transform.Equals(hit.transform)){ 		// ...and if the current inspected textmesh's transform in the array is equal to the current hit...
						currentConversation.selectConversationOption(n); 	// ...trigger the Select function of the current conversation with the selected textmesh.
					}
				}
			}
		}
	}

	public void showOptions(DialogueConversation conversation){				// Rev: Make the parameter the current conversation, cache a reference to the current conv. DialogueNode
		currentConversation = conversation;
		DialogueNode currentNode = conversation.currentNode;
		if(currentNode.children.Count > 0){									// Rev: If currentNode has children, iterate through and set each textmesh to position and SetActive.
			int n = 0;														// Rev: Each textmesh text content is set to the name of the node
			foreach(DialogueNode node in currentNode.children){
				TextMesh line = UILines[n];
				line.gameObject.SetActive(true);
				line.text = node.name;
				line.transform.position = Vector3.zero;
				line.transform.Translate(0, 0.6f * n, 0);
				n++;
			}
		}
	}

	public void hideOptions(){												// Rev: Iterate through current UILines, disable gameobject
		for(int n = 0 ; n < UILines.Length ; n++){
			TextMesh line = UILines[n];
			line.gameObject.SetActive(false);
		}
	}
}
