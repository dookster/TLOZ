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
	private Interactable interactable; // The interactable 'owning' this conversation

	// Use this for initialization
	void Start () {
		currentNode = this;
		loadChildren();
	}
	
	// Update is called once per frame
	void Update () {
		// We should probably handle as much input as possible in one place
		if(Input.GetKeyUp("1")){
			selectConversationOption(0);
		}
		if(Input.GetKeyUp("2")){
			selectConversationOption(1);
		}
		if(Input.GetKeyUp("3")){
			selectConversationOption(2);
		}
	}

	/**
	 * Select the conversation option with the given index
	 */
	public void selectConversationOption(int n){
		if(n < currentNode.children.Count){
			currentNode = currentNode.children[n];
			talk ();
		}
	}

	/**
	 * Calling this will attempt to 'say' the player's line then the 
	 * NPC's line. Each time pausing for a moment before going on. If
	 * the current node has only one child we skip to this and call talk 
	 * again.
	 * 
	 * If any line is missing or empty ("") it is skipped, this way we
	 * can string together several lines for one character.
	 */
	private void talk(){
		StartCoroutine(sayPlayerLine());
	}

	private IEnumerator sayPlayerLine() {
		if(currentNode.name != null && currentNode.name.Length > 0){
			GameFlow.instance.playerSay(currentNode.name);
			//yield return new WaitForSeconds(3); // should adjust time length, or do something else so we can skip lines on input
			while(GameFlow.instance.playerInteractable.isTalking())
				yield return null;
		}

		StartCoroutine(sayNPCLine());
	}

	private IEnumerator sayNPCLine() {
		if(currentNode.response != null && currentNode.response.Length > 0){
			interactable.say(currentNode.response);
			//yield return new WaitForSeconds(3); // should adjust time length, or do something else so we can skip lines on input
			while(interactable.isTalking())
				yield return null;
		}

		if(currentNode.name == "END"){
			// Conversation is done
			// How should we handle that?
		}
		else if(currentNode.children.Count > 1){
			showOptions();
		} else {
			currentNode = currentNode.children[0];
			talk();
		}
	}

	public void showOptions(){
		GameFlow.instance.conversationUI.showOptions(this);
	}

	public void startConversation(Interactable owner){
		interactable = owner;
		currentNode = this;

		// Start with the response? Or perhaps always have a single node after the root?
		StartCoroutine(sayNPCLine());
	}


}
