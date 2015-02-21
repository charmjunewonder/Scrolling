using UnityEngine;
using System.Collections;

public class MouseClick : MonoBehaviour {
    public Texture cubeTexture;
    public int cubeNumber = 0;
    public int cubeTotalNumber = 5;

    private GameObject UI_Cube_Number;
    private GameObject camera;
    private GameObject charactor;
    private GameObject house;
    private Quaternion defaultRotation;
    private Vector3 endPosition;
    private GameObject cube;
    private int count = 0;
    private bool isHouse;
    private bool duringPickUp = false;
    private Vector2 cubePos;
    private float timer = 0;

	// Use this for initialization
	void Start () 
    {
        UI_Cube_Number = GameObject.Find("Cube_Number");
        camera = GameObject.Find("Main Camera");
        charactor = GameObject.Find("Character");
        house = GameObject.Find("House");
        endPosition = new Vector2(-83, 44);
        isHouse = false;
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit == null || hit.collider == null)
                return;

            if (hit.collider.gameObject.tag == "Cube")
            {
                cube = hit.collider.gameObject;
                cube.transform.parent = camera.transform;
                duringPickUp = true;
                cubePos = camera.camera.WorldToScreenPoint(cube.transform.position); ;
                Destroy(cube);

                //StartCoroutine(GoToHell());
            }

        }
        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit == null || hit.collider == null)
                return;

            if (hit.collider.gameObject.tag == "Scalable")
            {
                if (hit.collider.gameObject.name == "HouseClickCollider")
                {
                    //GameObject house = hit.collider.gameObject.transform.parent.gameObject;
                    charactor.GetComponent<CharacterController>().hasScalableObject = true;
                    charactor.GetComponent<CharacterController>().scalableObject = house;
                    house.layer = 0;

                    float deltaLength = house.transform.position.x - charactor.transform.position.x;
                    house.transform.parent = charactor.transform;
                    isHouse = true;
                }
                else
                {
                    charactor.GetComponent<CharacterController>().hasScalableObject = true;
                    charactor.GetComponent<CharacterController>().scalableObject = hit.collider.gameObject;

                    float deltaLength = hit.collider.gameObject.transform.position.x - charactor.transform.position.x;

                    hit.collider.gameObject.transform.parent = charactor.transform;
                }
            }


        }
        else
        {
            if (isHouse)
            {
                Debug.Log("@@@");
                house.layer = LayerMask.NameToLayer("Ground");
                isHouse = false;
            }
            charactor.GetComponent<CharacterController>().hasScalableObject = false;
            if (charactor.GetComponent<CharacterController>().scalableObject != null)
            {
                charactor.GetComponent<CharacterController>().scalableObject.transform.parent = null;
            }

        }


	}

    IEnumerator GoToHell()
    {
        // might use GUI to draw the cube
        float timer = 0;
        Vector3 endPos = camera.camera.ScreenToWorldPoint(new Vector3(57, 635, 5));
        Debug.Log(endPos);

        while (timer <= 1)
        {

            endPos = camera.camera.ScreenToWorldPoint(new Vector3(57, 635, 5));

            cube.transform.position = Vector3.Lerp(cube.transform.position, endPos, timer);
            timer += 0.01f;

            yield return new WaitForSeconds(0.02f);
        }
        Destroy(cube);
    }
    void OnGUI()
    {
        int defaultWidth = 1600;
        float widthRatio = Screen.width * 1.0f / defaultWidth;
        float width = cubeTexture.width * 0.1f * widthRatio;
        float height = cubeTexture.height * 0.1f * widthRatio;
        GUI.DrawTexture(new Rect(Screen.width * 0.03f, Screen.height * 0.03f, width, height), cubeTexture);
        GUIStyle style = new GUIStyle();
        style.fontSize = Mathf.FloorToInt(30 * widthRatio);
        style.normal.textColor = Color.white;
        GUI.Label(new Rect(Screen.width * 0.07f, Screen.height * 0.035f, 50 * widthRatio, 50 * widthRatio),
                cubeNumber + "/" + cubeTotalNumber, style);

        if (duringPickUp)
        {
            Vector2 endPos = new Vector2(Screen.width * 0.07f - width / 2, Screen.height - Screen.height * 0.035f - height / 2);
            cubePos = Vector2.Lerp(cubePos, endPos, timer);
            timer += 0.01f;

            if (timer < 0.261f && timer > 0.259f)
            {
                cubeNumber++;
            }
            if (timer > 0.4f)
            {
                timer = 0;
                duringPickUp = false;
            }
            GUI.DrawTexture(new Rect(cubePos.x - width / 2, Screen.height - cubePos.y - height / 2, width, height), cubeTexture);

        }
    }

}
