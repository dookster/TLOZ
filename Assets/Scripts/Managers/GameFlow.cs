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

	public float readingTime = 0.0f;
	public float readingSpeed = 2.0f;

	public TextMesh sayChar; // Rev: Should probably make this private, hook up to PCMaja.
	public TextMesh sayCharShadow;


	void OnEnable(){
		// Listen for any broadcasts of the type 'event'
		Messenger<string>.AddListener("event", HandleEvent);
	}

	void OnDestroy(){
		Messenger<string>.RemoveListener("event", HandleEvent);
	}

	// Use this for initialization
	void Start () {
	
		sayChar = GameObject.Find ("sayMaja").GetComponent<TextMesh> ();
		sayCharShadow = GameObject.Find ("sayMajaShadow").GetComponent<TextMesh> ();

	}
	
	// Update is called once per frame
	void Update () {
	
		deltaTimeModifier = Mathf.Lerp (deltaTimeModifier, dTimeTarget, dTimeTargetSpeed * Time.deltaTime);
		
		Mathf.Clamp (deltaTimeModifier, 0.0f, 1.0f);
		
		dTimeModified = Time.deltaTime * deltaTimeModifier;

		readingTime += Time.deltaTime;
		
		if(readingTime >= readingSpeed){ // Rev: Timer for character dialogue. Should be global in gameflow? Should be coroutine?
			sayChar.text = null;
			sayCharShadow.text = null;
		}
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
	}

	public void ResetReadingTime () {
		readingTime = 0.0f;
		Debug.Log ("Resetting timer for reading text speed");
	}



}
