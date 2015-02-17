using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour {

    private GameObject camera;
    private int jumpCount = 0;
	// Use this for initialization
	void Start () {
        camera = GameObject.Find("Main Camera");
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.A)) {
			/*Vector3 pos = transform.position;
            pos.x -= 0.5f * camera.GetComponent<Camera>().orthographicSize / 50;
			transform.position = pos;*/
            transform.rigidbody2D.velocity = new Vector3(-30 * camera.GetComponent<Camera>().orthographicSize / 50,  transform.rigidbody2D.velocity.y , 0);
		}

		if (Input.GetKey (KeyCode.D)) {
			/*Vector3 pos = transform.position;
            pos.x += 0.5f * camera.GetComponent<Camera>().orthographicSize / 50;
			transform.position = pos;
			*/
            transform.rigidbody2D.velocity = new Vector3(30 * camera.GetComponent<Camera>().orthographicSize / 50, transform.rigidbody2D.velocity.y, 0);
		}

        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < 3)
        {
            transform.rigidbody2D.velocity = new Vector3(0, 50, 0);
            jumpCount++;
        }
	}

	public void changeSize(float increment){
		Vector3 scale = transform.localScale * increment;
		scale.z = transform.localScale.z;
		transform.localScale = scale;
	}

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "FallingTrigger")
        {
            //Debug.Log("fsjkld");
        }
        else
        {
            jumpCount = 0;
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "FallingTrigger")
        {
            camera.GetComponent<CameraController>().fakeToBlack();
        }
    }
}
