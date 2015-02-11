using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject character;


	private float scrollingTimer;
	private bool isZoomingOut;
	private bool isZoomingIn;

	// Use this for initialization
	void Start () {
		scrollingTimer = 0;
	}
	
	// Update is called once per frame
	void Update () 
	{
		Vector3 pos = character.transform.position;
		pos.z = transform.position.z;
		transform.position = pos;

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
			scrollingTimer += 0.02f;

			transform.GetComponent<Camera>().orthographicSize += 0.02f;

			if(scrollingTimer > 1)
			{
				isZoomingOut = false;

				scrollingTimer = 0;
			}
		}
		if (isZoomingIn) 
		{
			scrollingTimer += 0.02f;

			transform.GetComponent<Camera>().orthographicSize -= 0.02f;

			if(scrollingTimer > 1)
			{
				isZoomingIn = false;

				scrollingTimer = 0;
			}
		}
	}
}
