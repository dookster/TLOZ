using UnityEngine;
using System.Collections;

public class OptionText : MonoBehaviour {

	public TextMesh mainText;
	public TextMesh shadowText;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseEnter(){
		// Highlight the text when we mouse over
		mainText.color = new Color(mainText.color.r, mainText.color.g, mainText.color.b, 0.5f);
		shadowText.color = new Color(shadowText.color.r, shadowText.color.g, shadowText.color.b, 0.5f);
	}

	void OnMouseExit(){
		mainText.color = new Color(mainText.color.r, mainText.color.g, mainText.color.b, 1);
		shadowText.color = new Color(shadowText.color.r, shadowText.color.g, shadowText.color.b, 1);
	}

	public void setText(string t){
		mainText.text = t;
		shadowText.text = t;

		// Reset color
		mainText.color = new Color(mainText.color.r, mainText.color.g, mainText.color.b, 1);
		shadowText.color = new Color(shadowText.color.r, shadowText.color.g, shadowText.color.b, 1);
	}

}
