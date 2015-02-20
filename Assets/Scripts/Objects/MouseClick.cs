using UnityEngine;
using System.Collections;

public class MouseClick : MonoBehaviour {
    private GameObject UI_Cube_Number;
    private GameObject camera;
    private GameObject charactor;
    private Quaternion defaultRotation;
    private Vector3 endPosition;
    private GameObject cube;
    private int count = 0;
	// Use this for initialization
	void Start () 
    {
        UI_Cube_Number = GameObject.Find("Cube_Number");
        camera = GameObject.Find("Main Camera");
        charactor = GameObject.Find("Character");
        endPosition = new Vector2(-83, 44);
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

            if (hit.collider.gameObject.tag == "Scalable")
            {
                
                if (count == 0) {
                    defaultRotation = hit.collider.gameObject.transform.rotation;
                    count++;
                    Debug.Log("yes");
                }
                Renderer renderer = hit.collider.gameObject.GetComponent<Renderer>();
                float height = renderer.bounds.size.x;
                Vector2 pos = hit.collider.gameObject.transform.position;
                pos.y = pos.y + height / 2;
                hit.collider.gameObject.transform.position = pos;
                hit.collider.gameObject.transform.rotation = new Quaternion(0, 0, 0, 1);

            }

        }
        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit == null || hit.collider == null)
                return;

            if (hit.collider.gameObject.tag == "Scalable")
            {
                //Debug.Log("yes");

                charactor.GetComponent<CharacterController>().hasScalableObject = true;
                charactor.GetComponent<CharacterController>().scalableObject = hit.collider.gameObject;

                float deltaLength = hit.collider.gameObject.transform.position.x - charactor.transform.position.x;

                if (Mathf.Abs(deltaLength) < 1.5f)
                {
                    hit.collider.gameObject.transform.parent = charactor.transform;
                }
                //charactor.GetComponent<CharacterController>().recordDelta();

                //hit.collider.gameObject.rigidbody2D.Sleep();
                Destroy(hit.collider.gameObject.rigidbody2D);

            }

        }
        else
        {
            //Debug.Log("no");
            charactor.GetComponent<CharacterController>().hasScalableObject = false;
            if (charactor.GetComponent<CharacterController>().scalableObject != null)
            {
                charactor.GetComponent<CharacterController>().scalableObject.transform.parent = null;
                if (charactor.GetComponent<CharacterController>().scalableObject.rigidbody2D == null)
                {
                    charactor.GetComponent<CharacterController>().scalableObject.AddComponent<Rigidbody2D>();
                    charactor.GetComponent<CharacterController>().scalableObject.rigidbody2D.gravityScale = 10;
                    charactor.GetComponent<CharacterController>().scalableObject.rigidbody2D.mass = 10;
                }
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
