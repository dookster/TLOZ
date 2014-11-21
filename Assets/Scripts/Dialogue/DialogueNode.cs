using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEditor; // Disable this before building final game

/**
 * Represents a single connection in a conversation. 
 * 
 * The name of the gameobject is the sentence the player can choose to say, the variable
 * 'response' is what the character who owns the conversation will reply. 
 * 
 * All subsequent dialogue options are children of this object's transform.
 * 
 * !! NB !! Uncomment the OnDrawGizmos method and the 'using UnityEditor' lines before building game.
 * 
 */
[ExecuteInEditMode]
public class DialogueNode : MonoBehaviour {
	
	public string response;

	private GUIStyle style;
	private GUIStyle style2;

	public List<DialogueNode> children = new List<DialogueNode>();
	public DialogueNode parentNode;

	// Use this for initialization
	void Start () {
		// Find style in parents
		style = GetComponentInParent<DialogueConversation>().editorStyle;
		style2 = GetComponentInParent<DialogueConversation>().editorStyle2;

		loadChildren();
	}

	public void loadChildren(){
		// Get a reference to the parent if it's a DialogueNode
		if(transform.parent != null){
			parentNode = transform.parent.GetComponent<DialogueNode>();
		}
		
		// Get a list of all children
		for(int n = 0 ; n < transform.childCount ; n++){
			children.Add(transform.GetChild(n).GetComponent<DialogueNode>());
		}
	}

//	public void showOptions(){
//		int n = 1;
//		foreach(DialogueNode node in children){
//			Debug.Log("\n" + n + ": " + node.name);
//			n++;
//		}
//	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnDrawGizmos() {
		if(style == null) style = GetComponentInParent<DialogueConversation>().editorStyle;
		if(style2 == null) style2 = GetComponentInParent<DialogueConversation>().editorStyle2;
		Vector3 labelPos = new Vector3(transform.position.x - transform.localScale.x/4, transform.position.y , transform.position.z);

		if(parentNode != null) Handles.DrawDottedLine(transform.position, parentNode.transform.position, 5);

		Handles.Label(labelPos, "\n\n"+ "  Response:                                                     '\n  " + response + " \n .", style2);
		Handles.Label(labelPos, " * " + gameObject.name, style);

	}



}

