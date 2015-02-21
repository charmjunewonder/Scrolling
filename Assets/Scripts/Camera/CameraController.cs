using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject character;
	public float cameraSize;
	public float ground = -45.51134f;
    public GameObject blackPlane;
    public GameObject UI_Cube_Number;
    public GameObject UI_Cube;
    public int cubeNumber = 0;
    public int cubeTotalNumber = 5;

	private float scrollingTimer;
	private bool isZoomingOut;
	private bool isZoomingIn;
	private Vector3 previousCharacterPos;
    
    private Vector3 targetPosition;
    
    public Texture aTexture;

    private bool scrollingLock;
	// Use this for initialization
	void Start () {
        scrollingLock = false;
		scrollingTimer = 0;
		previousCharacterPos = Vector3.zero;
        blackPlane.renderer.material.SetColor("_Color", new Color(0, 0, 0, 0));
        UI_Cube_Number.GetComponent<TextMesh>().text = " ";
        UI_Cube_Number.GetComponent<TextMesh>().text = "0/5";
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
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && transform.GetComponent<Camera>().orthographicSize > 10 && !scrollingLock) 
		{
			isZoomingIn = true;
            scrollingLock = true;
		}

        if (Input.GetAxis("Mouse ScrollWheel") < 0 && transform.GetComponent<Camera>().orthographicSize < 50 && !scrollingLock)
		{
			isZoomingOut = true;
            scrollingLock = true;
		}

        if (isZoomingOut && transform.GetComponent<Camera>().orthographicSize < 50)
        {
			scrollingTimer += 0.2f;

			transform.GetComponent<Camera>().orthographicSize *= 1.0f/0.95f;

            character.GetComponent<CharacterController>().changeSize(1f/0.95f);
            character.GetComponent<CharacterController>().changePosition(groundPos, 1f / 0.95f);


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

            character.GetComponent<CharacterController>().changePosition(groundPos, 0.95f);
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
        updateUI();
		previousCharacterPos = character.transform.position;
	}

    private void updateUI(){
        float size = transform.GetComponent<Camera>().orthographicSize;
        UI_Cube_Number.transform.localPosition = new Vector3(-size, size, 5);
        UI_Cube.transform.localPosition = new Vector3(-size, size, 5);
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
        pos.x = 686.8f;
        pos.y = -666.1f;
        character.transform.position = pos;
        blackPlane.SetActive(false);
        blackPlane.renderer.material.SetColor("_Color", new Color(0, 0, 0, 0));
    }

    public void fakeToBlackUp()
    {
        blackPlane.renderer.material.SetColor("_Color", new Color(0, 0, 0, 0));
        blackPlane.SetActive(true);
        StartCoroutine(fakeUp());
    }

    IEnumerator fakeUp()
    {
        float num = 0;
        for (int i = 0; i < 50; i++)
        {
            num += 0.02f;
            blackPlane.renderer.material.SetColor("_Color", new Color(0, 0, 0, num));
            yield return new WaitForSeconds(0.02f);
        }
        Vector3 pos = character.transform.position;
        pos.x = 555;
        pos.y = -49f;
        character.transform.position = pos;
        blackPlane.SetActive(false);
        blackPlane.renderer.material.SetColor("_Color", new Color(0, 0, 0, 0));
    }

    void OnGUI() {
        int defaultWidth = 1600;
        float widthRatio = Screen.width * 1.0f/ defaultWidth;
        float width = aTexture.width*0.1f*widthRatio;
        float height = aTexture.height*0.1f*widthRatio;
        GUI.DrawTexture(new Rect(Screen.width * 0.03f, Screen.height * 0.03f, width, height), aTexture);
        GUIStyle style = new GUIStyle();
        style.fontSize = Mathf.FloorToInt(30*widthRatio);
        style.normal.textColor = Color.white;
        GUI.Label(new Rect(Screen.width * 0.07f, Screen.height * 0.035f, 50*widthRatio, 50*widthRatio), 
                cubeNumber + "/" + cubeTotalNumber, style);
    }
}
