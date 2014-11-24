using UnityEngine;
using System.Collections;

public class GameFlow : MonoBehaviour {

	// Creating Singleton variables
	private static GameFlow _instance;

	public static GameFlow instance{
		get{
			if(_instance == null){
				_instance = GameObject.FindObjectOfType<GameFlow>();
				// Tell Unity not to destroy this object when loading a new scene!
				DontDestroyOnLoad(_instance.gameObject);
			}
			return _instance;
		}
	}

	[SerializeField]	float	deltaTimeModifier = 1.0f;
	[Range(0.0f, 1.0f)]
	[SerializeField]	float	dTimeTarget = 1.0f;
	public	float	dTimeTargetSpeed = 1.0f;
	public	float	dTimeModified; // This should be a property!
	[SerializeField]	bool	pause = false;


	public float readingSpeed = 2.0f;
	public bool conversationClickThrough; // If true, wait for the player to click before advancing conversations

	public InteractableRev playerInteractable;

	public ConversationUI conversationUI;

	public bool inputPaused; // Set to true to keep the player from moving the character, e.g. in a conversation

	private TextMesh actionLine; // Rev: Happy to rename this - it's the constructed sentence that describes what left-clicking will do. =)
	private TextMesh actionLineShadow;

	private TextMesh 	debugStatus;
	private bool		debugStatusVisible = false;

	public float 	actionLineTime = 0.0f;
	public bool 	actionLineReset = false;

	public AudioClip[] audClips;

	private Inventory inv;
	public GameObject testInvItem;

	public GameObject spawnEmptyCup; // Rev: Spawn so you can keep giving coffee to Royo

	private GameObject faderCard;

	// Rev: Bools set by functions used to set up game events - bools are displayed in 3D Text.
	public bool		cutsceneIntro 		= false;
	public bool		eavesdropSetup 		= false;
	public bool		cutsceneEnvelope 	= false;
	public bool		sealedSetup			= false;
	public bool		cutsceneMortar		= false;
	public bool		whiskeySetup		= false;
	public bool		computerSetup		= false;
	public bool		cutsceneConfront	= false;
	public bool		finaleSetup			= false;
	public bool		cutsceneOutro		= false;
	public bool		cutsceneCredits		= false;
	
	void OnEnable(){
		// Listen for any broadcasts of the type 'event'
		Messenger<string>.AddListener("event", HandleEvent);
	}

	void OnDestroy(){
		Messenger<string>.RemoveListener("event", HandleEvent);
	}

	// Use this for initialization
	void Start () {
		//sayChar = GameObject.Find ("sayMaja").GetComponent<TextMesh> ();
		//sayCharShadow = GameObject.Find ("sayMajaShadow").GetComponent<TextMesh> ();
		playerInteractable = GameObject.Find ("Player").GetComponent<InteractableRev>();
		conversationUI = GameObject.Find("Conversation").GetComponent<ConversationUI>();

		actionLine = GameObject.Find ("sayNeutral").GetComponent<TextMesh> ();
		actionLineShadow = GameObject.Find ("sayNeutralShadow").GetComponent<TextMesh> ();

		debugStatus = GameObject.Find ("DebugStatus").GetComponent<TextMesh> ();

		inv = GameObject.Find ("Inventory").GetComponent<Inventory> ();

		faderCard = GameObject.Find ("FaderCard");

		iTween.CameraFadeAdd ();
	}
	
	// Update is called once per frame
	void Update () {
	
		deltaTimeModifier = Mathf.Lerp (deltaTimeModifier, dTimeTarget, dTimeTargetSpeed * Time.deltaTime);
		
		Mathf.Clamp (deltaTimeModifier, 0.0f, 1.0f);
		
		dTimeModified = Time.deltaTime * deltaTimeModifier;

		if (Input.GetKeyDown(KeyCode.RightBracket) && actionLine != null){ // Rev: Increases reading speed, prints status on action line
			readingSpeed += 0.5f;
			readingSpeed = Mathf.Clamp(readingSpeed, 0.5f, 8.0f);
			actionLine.text = "ReadingSpeed: " + readingSpeed;
			actionLineTime = 0.0f;
			actionLineReset = true;
		}

		if (Input.GetKeyDown(KeyCode.LeftBracket) && actionLine != null){ // Rev: Increases reading speed, prints status on action line.
			readingSpeed -= 0.5f;
			readingSpeed = Mathf.Clamp(readingSpeed, 0.5f, 8.0f);
			actionLine.text = "ReadingSpeed: " + readingSpeed;
			actionLineTime = 0.0f;
			actionLineReset = true;
		}

		if(actionLineTime < GameFlow.instance.readingSpeed){ // Rev: Timer to reset action line.
			actionLineTime += Time.deltaTime;
		}
		
		if(actionLineTime >= GameFlow.instance.readingSpeed && actionLineReset){
			hideActionLine();
			actionLineReset = false;
		}

		if(Input.GetKeyDown (KeyCode.KeypadEnter)){
			if(debugStatusVisible){
				debugStatus.text = null;
				debugStatusVisible = false;
			}else if (!debugStatusVisible){
				Debug.Log("Should be displaying Status...");
				debugStatusSetup();
				debugStatusVisible = true;
			}
		}

		if(Input.GetKeyDown (KeyCode.Keypad1))CutsceneIntro();
		if(Input.GetKeyDown (KeyCode.Keypad2))EavesdropSetup();
		if(Input.GetKeyDown (KeyCode.Keypad3))CutsceneEnvelope();
		if(Input.GetKeyDown (KeyCode.Keypad4))SealedSetup();
		if(Input.GetKeyDown (KeyCode.Keypad5))CutsceneMortar();
		if(Input.GetKeyDown (KeyCode.Keypad6))WhiskeySetup();
		if(Input.GetKeyDown (KeyCode.Keypad7))ComputerSetup();
		if(Input.GetKeyDown (KeyCode.Keypad8))CutsceneConfront();
		if(Input.GetKeyDown (KeyCode.Keypad9))FinaleSetup();
		if(Input.GetKeyDown (KeyCode.Keypad0))CutsceneOutro();
		if(Input.GetKeyDown (KeyCode.KeypadPeriod))CutsceneCredits();

	}

	private void hideActionLine(){
		if(actionLine == null) return;
		actionLine.text = "";
		actionLineShadow.text = "";
	}

	public void PlayAudioGeneric (int clipNum){
		audio.clip = audClips [clipNum];
		audio.Play ();
	}
	
	/**
	 * Super generic method, called whenever anything sends a broadcast with the name 'event'. Added here mainly to show
	 * how other scripts can subscribe to this broadcast if they need to. 
	 * 
	 * To send a broadcast use this line:
	 * 
	 * Messenger<string>.Broadcast("event", eventName);
	 * 
	 */
	private void HandleEvent(string eventName){
		Debug.Log("Detected event: " + eventName);

		if(eventName == "PlayerEmpty coffee cup and saucer" || eventName == "Empty coffee cup and saucerPlayer" || eventName == "PlayerCracked coffee cup" || eventName == "Cracked coffee cupPlayer"){
			PickUpEmptyCup();
		}

		if(eventName == "Coffee makerEmpty coffee cup and saucer" || eventName == "Empty coffee cup and saucerCoffee maker" || eventName == "Coffee makerHot cup of coffee with saucer" || eventName == "Hot cup of coffee with saucerCoffee maker"){
			MakingCoffee();
		}

		if(eventName == "Coffee makerCracked coffee cup" || eventName == "Cracked coffee cupCoffee maker"){
			animation.Play ("MakeCrackedCoffee");
		}

		if(eventName == "Sealed envelopeCoffee pot"){
			animation.Play ("OpeningTheEnvelope");
			Debug.Log ("Opening the envelope...");
		}


		if(eventName == "Hot cup of coffee with saucerCaptain Aiden Royo"){
			GameObject cup = GameObject.Instantiate(spawnEmptyCup) as GameObject;
			cup.name = cup.name.Replace ("(Clone)","");
		}

		if(eventName == "Captain Aiden RoyoCracked cup, full of hot coffee" || eventName == "Cracked cup, full of hot coffeeCaptain Aiden Royo"){

		}

	}

	public void playerSay(string text){
		playerInteractable.say(text);
		//sayChar.text = text;
		//sayCharShadow.text = text;
		//ResetReadingTime();
	}

	// Rev: Function that can be triggered by animation clip
	public void PauseInput(int isTrue){
		if (isTrue == 0){
			inputPaused = false;
		} else if (isTrue == 1){
			inputPaused = true;
		}
	}

	// Rev: Following is a list of functions for setting up minor repeating events in the game.

	public void MakingCoffee() {
		animation.Play("MakeCoffee");
	}

	public void PickUpEmptyCup(){
		audio.clip = audClips [2];
		audio.Play ();
	}


	// Rev: Following is a list of functions for setting up MAJOR events and cutscenes in the game.

	public void CutsceneIntro(){
		cutsceneIntro = true;
		debugStatusSetup();
	}

	public void EavesdropSetup(){
		eavesdropSetup = true;
		debugStatusSetup();
	}

	public void CutsceneEnvelope(){
		cutsceneEnvelope = true;
		debugStatusSetup();
	}

	public void SealedSetup(){
		GameObject newitem = GameObject.Instantiate (testInvItem) as GameObject;
		newitem.name = newitem.name.Replace ("(Clone)", "");
		inv.addItem (newitem);
		sealedSetup = true;
		debugStatusSetup();
	}

	public void CutsceneMortar(){
		cutsceneMortar = true;
		debugStatusSetup();
	}

	public void WhiskeySetup(){
		whiskeySetup = true;
		debugStatusSetup();
	}

	public void ComputerSetup(){
		computerSetup = true;
		debugStatusSetup();
	}

	public void CutsceneConfront(){
		cutsceneConfront = true;
		debugStatusSetup();
	}

	public void FinaleSetup(){
		finaleSetup = true;
		debugStatusSetup();
	}

	public void CutsceneOutro(){
		cutsceneOutro = true;
		debugStatusSetup();
	}

	public void CutsceneCredits(){
		cutsceneCredits = true;
		debugStatusSetup();
	}

	public void FadeToBlack (float time = 1.0f){
		iTween.FadeTo (faderCard, iTween.Hash ("amount", 1.0, "time", time, "easeType", "easeOutQuart"));
	}

	public void FadeToClear (float time = 1.0f){
		iTween.FadeTo (faderCard, iTween.Hash ("amount", 0.0, "time", time, "easeType", "easeOutQuart"));
	}
	
	public void debugStatusSetup () { // Rev: This updates the debug 3DText with the state bools set by the event functions.
		debugStatus.text = 	"1 Cutscene Intro: " + cutsceneIntro + "\n" +
							"2 Eavesdrop Setup: " + eavesdropSetup + "\n" +
							"3 Cutscene Envelope: " + cutsceneEnvelope + "\n" +
							"4 Sealed Setup: " + sealedSetup + "\n" +
							"5 Cutscene Mortar: " + cutsceneMortar + "\n" +
							"6 Whiskey Setup: " + whiskeySetup + "\n" +
							"7 Computer Setup: " + computerSetup + "\n" +
							"8 Cutscene Confront: " + cutsceneConfront + "\n" +
							"9 Finale Setup: " + finaleSetup + "\n" +
							"0 Cutscene Outro: " + cutsceneOutro + "\n" +
							". Cutscene Credits: " + cutsceneCredits;
	}
}
