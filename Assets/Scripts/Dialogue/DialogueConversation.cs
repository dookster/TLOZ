using UnityEngine;
using System.Collections;
using UnityEditor; // Disable this before building final game

/**
 * Root node of a conversation
 */
public class DialogueConversation : DialogueNode{

	public GUIStyle editorStyle; // Gui style for gizmos in the editor
	public GUIStyle editorStyle2;

	public DialogueNode currentNode;
	private InteractableRev interactable; // The interactable 'owning' this conversation

	// Use this for initialization
	void Start () {
		currentNode = this;
		loadChildren();
	}
	
	// Update is called once per frame
	void Update () {
		// We should probably handle as much input as possible in one place
		if(Input.GetKeyUp("1")){
			if(currentNode.children.Count > 0){
				currentNode = currentNode.children[0];
				interactable.playerSay(currentNode.name);
				interactable.say(currentNode.response);
				currentNode.showOptions();
			}
		}
		if(Input.GetKeyUp("2")){
			if(currentNode.children.Count > 1){
				currentNode = currentNode.children[1];
				interactable.playerSay(currentNode.name);
				interactable.say(currentNode.response);
				currentNode.showOptions();
			}
		}
		if(Input.GetKeyUp("3")){
			if(currentNode.children.Count > 2){
				currentNode = currentNode.children[2];
				interactable.playerSay(currentNode.name);
				interactable.say(currentNode.response);
				currentNode.showOptions();
			}
		}
	}

	public void startConversation(InteractableRev owner){
		interactable = owner;
		if(currentNode == null) currentNode = this;

		// Start with the response? Or perhaps always have a single node after the root?
		interactable.say(currentNode.response);
		currentNode.showOptions();
	}


}
