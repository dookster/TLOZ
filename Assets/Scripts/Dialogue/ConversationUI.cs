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

		if(Physics.Raycast(ray, out hit)){
			if (hit.transform.name == "option") {
				for(int n = 0 ; n < UILines.Length ; n++){
					if(UILines[n].transform.Equals(hit.transform)){
						currentConversation.selectConversationOption(n);
					}
				}
			}
		}
	}

	public void showOptions(DialogueConversation conversation){
		currentConversation = conversation;
		DialogueNode currentNode = conversation.currentNode;
		if(currentNode.children.Count > 0){
			int n = 0;
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

	public void hideOptions(){
		for(int n = 0 ; n < UILines.Length ; n++){
			TextMesh line = UILines[n];
			line.gameObject.SetActive(false);
		}
	}
}
