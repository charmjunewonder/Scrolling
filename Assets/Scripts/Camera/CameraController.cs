using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject character;
	public float cameraSize;

	private float scrollingTimer;
	private bool isZoomingOut;
	private bool isZoomingIn;
	private Vector3 previousCharacterPos;

    private Vector3 targetPosition;
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
			Vector3 pos = transform.position;
			pos.x += character.transform.position.x - previousCharacterPos.x;
			transform.position = pos;
		}

        if (Mathf.Abs(character.transform.position.y - transform.position.y) > cameraSize  * 0.6f)
        {
            Vector3 pos = transform.position;
            pos.y += character.transform.position.y - previousCharacterPos.y;
            transform.position = pos;
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0 && transform.GetComponent<Camera>().orthographicSize > 10) 
		{
			isZoomingIn = true;
            
            Debug.Log(targetPosition);
		}

        if (Input.GetAxis("Mouse ScrollWheel") > 0 && transform.GetComponent<Camera>().orthographicSize < 50)
		{
			isZoomingOut = true;
            
            Debug.Log(targetPosition);
		}

        if (isZoomingOut && transform.GetComponent<Camera>().orthographicSize < 50) 
		{
			scrollingTimer += 0.2f;

			transform.GetComponent<Camera>().orthographicSize += 0.5f;

            character.GetComponent<CharacterController>().changeSize(0.01f);
            targetPosition = -(character.transform.position - transform.position) * 3 / cameraSize;
            transform.position += new Vector3(targetPosition.x, targetPosition.y, 0) / 6;

			if(scrollingTimer > 1)
			{
				isZoomingOut = false;

				scrollingTimer = 0;
			}
		}
        if (isZoomingIn && transform.GetComponent<Camera>().orthographicSize > 10) 
		{
			scrollingTimer += 0.2f;
            
			transform.GetComponent<Camera>().orthographicSize -= 0.5f;
            targetPosition = (character.transform.position - transform.position) * 3 / cameraSize;
            character.GetComponent<CharacterController>().changeSize(-0.01f);

            transform.position += new Vector3(targetPosition.x, targetPosition.y, 0) / 6;

			if(scrollingTimer > 1)
			{
				isZoomingIn = false;

				scrollingTimer = 0;
			}
		}
		previousCharacterPos = character.transform.position;
	}
}
