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

	// Use this for initialization
	void Start () {
	
		sayChar = GameObject.Find ("sayAbner").GetComponent<TextMesh> (); // Rev: NOTE! This will have to be changed when we get the Maja PC character!
		sayCharShadow = GameObject.Find ("sayAbnerShadow").GetComponent<TextMesh> (); // Rev: This too!

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

	public void ResetReadingTime () {
		readingTime = 0.0f;
		Debug.Log ("Resetting timer for reading text speed");
		}
}
