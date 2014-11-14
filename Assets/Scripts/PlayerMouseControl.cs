using UnityEngine;
using System.Collections;

public class PlayerMouseControl : MonoBehaviour {
	
	public	GameObject		walkTo;
	
	public	NavMeshAgent	character01;
	
	public 	Camera 	cameraA;
	
	public	float	rayDistance	=	25.00f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		Ray ray = cameraA.ScreenPointToRay(Input.mousePosition);
		Debug.DrawRay(cameraA.transform.position, ray.direction * rayDistance, Color.red);
		
		RaycastHit hit;
		
		if(Physics.Raycast(ray, out hit)){
			
			if(Input.GetButtonDown ("Fire1")){
				
					walkTo.transform.position	= hit.point;
					
					character01.destination 	= hit.point;
				
				}
			
			if(hit.transform.tag == "Hotspot"){
		
				if (Input.GetButtonDown ("Fire1")){
			
					hit.transform.SendMessage("Use",SendMessageOptions.DontRequireReceiver);
					
					walkTo.transform.position	= hit.point;
				
					character01.destination 	= hit.point;
					
				}				
			}
		}		
	}
}
