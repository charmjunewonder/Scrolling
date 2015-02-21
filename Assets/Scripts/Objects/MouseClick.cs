using UnityEngine;
using System.Collections;

public class MouseClick : MonoBehaviour {
    private GameObject UI_Cube_Number;
    private GameObject camera;
    private GameObject charactor;
    private GameObject house;
    private Quaternion defaultRotation;
    private Vector3 endPosition;
    private GameObject cube;
    private int count = 0;
    private bool isHouse;
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

                StartCoroutine(GoToHell());
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
        float timer = 0;
        while (timer <= 1)
        {
            cube.transform.localPosition = Vector3.Lerp(cube.transform.localPosition, endPosition, timer);
            timer += 0.01f;
            if (timer < 0.261f && timer > 0.259f)
            {
                string s = UI_Cube_Number.GetComponent<TextMesh>().text;
                string snumber = s.Substring(0, 1);
                int number = int.Parse(snumber) + 1;
                UI_Cube_Number.GetComponent<TextMesh>().text = number + s.Substring(1);
            }
            yield return new WaitForSeconds(0.02f);
        }
        Destroy(cube);
    }
}
