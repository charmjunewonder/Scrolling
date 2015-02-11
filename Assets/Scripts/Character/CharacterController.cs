using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.A)) {
			Vector3 pos = transform.position;
			pos.x -= 0.2f;
			transform.position = pos; 
		}

		if (Input.GetKey (KeyCode.D)) {
			Vector3 pos = transform.position;
			pos.x += 0.2f;
			transform.position = pos;
			
		}

		if (Input.GetKey (KeyCode.W)) {
			Vector3 pos = transform.position;
			pos.y += 0.2f;
			transform.position = pos;
			
		}

		if (Input.GetKey (KeyCode.S)) {
			Vector3 pos = transform.position;
			pos.y -= 0.2f;
			transform.position = pos;
			
		}
	}
}
