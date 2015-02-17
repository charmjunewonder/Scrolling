using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject character;
	public float cameraSize;
	public float ground = -45.51134f;
	private float scrollingTimer;
	private bool isZoomingOut;
	private bool isZoomingIn;
	private Vector3 previousCharacterPos;

    private Vector3 targetPosition;

    private bool scrollingLock;
	// Use this for initialization
	void Start () {
        scrollingLock = false;
		scrollingTimer = 0;
		previousCharacterPos = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () 
	{

		cameraSize = transform.GetComponent<Camera>().orthographicSize;

		if (Mathf.Abs(character.transform.position.x - transform.position.x) > cameraSize * 2 * 0.5f) {
			Vector3 pos = transform.position;
			pos.x += character.transform.position.x - previousCharacterPos.x;
			transform.position = pos;
		}

        if (Mathf.Abs(character.transform.position.y - transform.position.y) > cameraSize  * 0.8f)
        {
            Vector3 pos = transform.position;
            pos.y += character.transform.position.y - previousCharacterPos.y;
            transform.position = pos;
        }
        //Debug.Log("fjsldk " + (transform.position - character.transform.position) * (1- 0.5f / cameraSize));
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && transform.GetComponent<Camera>().orthographicSize > 10 && !scrollingLock) 
		{
			isZoomingIn = true;
            scrollingLock = true;
            //targetPosition = (transform.position - character.transform.position) * (1 - 3f / cameraSize) + character.transform.position;
            
		}

        if (Input.GetAxis("Mouse ScrollWheel") < 0 && transform.GetComponent<Camera>().orthographicSize < 50 && !scrollingLock)
		{
			isZoomingOut = true;
            scrollingLock = true;
            //targetPosition = (transform.position - character.transform.position) * (1 + 0.5f / cameraSize) + character.transform.position;
            
		}

        if (isZoomingOut && transform.GetComponent<Camera>().orthographicSize < 50)
        {
            //Debug.Log(targetPosition);
			scrollingTimer += 0.2f;

			transform.GetComponent<Camera>().orthographicSize *= 1.0f/0.95f;

            character.GetComponent<CharacterController>().changeSize(1f/0.95f);
            Vector3 pos = character.transform.position;
            pos.y = ground + (pos.y - ground) * 1.0f/0.95f;
            character.transform.position = pos;

            targetPosition.x = (transform.position.x - character.transform.position.x) * 1.0f/0.95f + character.transform.position.x;
            targetPosition.y = (transform.position.y - ground) * 1.0f/0.95f + ground;

            transform.position = new Vector3(targetPosition.x, targetPosition.y, -10);
			Debug.Log("Out: " + transform.position + " " + character.transform.position);
            if (scrollingTimer > 1 || transform.GetComponent<Camera>().orthographicSize >= 50)
			{
				isZoomingOut = false;
                scrollingLock = false;
				scrollingTimer = 0;
			}
		}
        if (isZoomingIn && transform.GetComponent<Camera>().orthographicSize > 10)
        {
            //Debug.Log(targetPosition);
			scrollingTimer += 0.2f;
            
			transform.GetComponent<Camera>().orthographicSize *= 0.95f;
            Vector3 pos = character.transform.position;
            pos.y = ground + (pos.y - ground) * 0.95f;
            character.transform.position = pos;
            character.GetComponent<CharacterController>().changeSize(0.95f);
            targetPosition.x = (transform.position.x - character.transform.position.x) * 0.95f + character.transform.position.x;
            targetPosition.y = (transform.position.y - ground) * 0.95f + ground;

            transform.position = new Vector3(targetPosition.x, targetPosition.y, -10);
			Debug.Log("In: " + transform.position + " " + character.transform.position);
            if (scrollingTimer > 1 || transform.GetComponent<Camera>().orthographicSize <= 10)
			{
				isZoomingIn = false;
                scrollingLock = false;
				scrollingTimer = 0;
			}
		}
		previousCharacterPos = character.transform.position;
	}
}
