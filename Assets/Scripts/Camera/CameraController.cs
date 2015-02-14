using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject character;
	public float cameraSize;

	private float scrollingTimer;
	private bool isZoomingOut;
	private bool isZoomingIn;
	private Vector3 previousCharacterPos;

	// Use this for initialization
	void Start () {
		scrollingTimer = 0;
		previousCharacterPos = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () 
	{

		cameraSize = transform.GetComponent<Camera>().orthographicSize;

		if (Mathf.Abs(character.transform.position.x - transform.position.x) > cameraSize * 2 * 0.5f) {
			Debug.Log("fjskl");
			Vector3 pos = transform.position;
			pos.x += character.transform.position.x - previousCharacterPos.x;
			transform.position = pos;
		}

		if (Input.GetAxis ("Mouse ScrollWheel") < 0) 
		{
			isZoomingIn = true;
		}

		if(Input.GetAxis ("Mouse ScrollWheel") > 0)
		{
			isZoomingOut = true;
		}

		if (isZoomingOut) 
		{
			scrollingTimer += 0.2f;

			transform.GetComponent<Camera>().orthographicSize += 0.2f;

			character.GetComponent<CharacterController>().changeSize(0.004f);

			if(scrollingTimer > 1)
			{
				isZoomingOut = false;

				scrollingTimer = 0;
			}
		}
		if (isZoomingIn) 
		{
			scrollingTimer += 0.2f;

			transform.GetComponent<Camera>().orthographicSize -= 0.2f;
			character.GetComponent<CharacterController>().changeSize(-0.004f);

			if(scrollingTimer > 1)
			{
				isZoomingIn = false;

				scrollingTimer = 0;
			}
		}
		previousCharacterPos = character.transform.position;
	}
}
