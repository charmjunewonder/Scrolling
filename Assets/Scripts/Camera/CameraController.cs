using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject character;
	public float cameraSize;
	public float ground = -45.51134f;
    public GameObject blackPlane;

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
        blackPlane.renderer.material.SetColor("_Color", new Color(0, 0, 0, 0));
	}
	
	// Update is called once per frame
	void Update () 
	{

		cameraSize = transform.GetComponent<Camera>().orthographicSize;

        Vector2 groundPos = new Vector2(0, ground);
        int layerMask = 1 << 8;
        RaycastHit2D hit = Physics2D.Raycast(character.transform.position, new Vector2(0, -1), 100, layerMask);
        if (hit.collider != null)
        {
            groundPos = hit.point;
            //Debug.DrawLine(hit.point, hit.normal, new Color(1, 0, 0), 10);
        }
        Debug.Log(groundPos);

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
            pos.y = groundPos.y + (pos.y - groundPos.y) * 1.0f / 0.95f;
            character.transform.position = pos;

            targetPosition.x = (transform.position.x - character.transform.position.x) * 1.0f/0.95f + character.transform.position.x;
            targetPosition.y = (transform.position.y - groundPos.y) * 1.0f / 0.95f + groundPos.y;

            transform.position = new Vector3(targetPosition.x, targetPosition.y, -10);
			//Debug.Log("Out: " + transform.position + " " + character.transform.position);
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
            pos.y = groundPos.y + (pos.y - groundPos.y) * 0.95f;
            character.transform.position = pos;
            character.GetComponent<CharacterController>().changeSize(0.95f);
            targetPosition.x = (transform.position.x - character.transform.position.x) * 0.95f + character.transform.position.x;
            targetPosition.y = (transform.position.y - groundPos.y) * 0.95f + groundPos.y;

            transform.position = new Vector3(targetPosition.x, targetPosition.y, -10);
			//Debug.Log("In: " + transform.position + " " + character.transform.position);
            if (scrollingTimer > 1 || transform.GetComponent<Camera>().orthographicSize <= 10)
			{
				isZoomingIn = false;
                scrollingLock = false;
				scrollingTimer = 0;
			}
		}
		previousCharacterPos = character.transform.position;
	}

    public void fakeToBlack()
    {
        blackPlane.renderer.material.SetColor("_Color", new Color(0, 0, 0, 0));
        blackPlane.SetActive(true);
        StartCoroutine(fake());
    }

    IEnumerator fake()
    {
        float num = 0;
        for (int i = 0; i < 50; i++)
        {
            num += 0.02f;
            blackPlane.renderer.material.SetColor("_Color", new Color(0, 0, 0, num));
            yield return new WaitForSeconds(0.02f);
        }
        Vector3 pos = character.transform.position;
        pos.x = 541.8f;
        pos.y = -946.1f;
        character.transform.position = pos;
        blackPlane.SetActive(false);
        blackPlane.renderer.material.SetColor("_Color", new Color(0, 0, 0, 0));
    }
}
