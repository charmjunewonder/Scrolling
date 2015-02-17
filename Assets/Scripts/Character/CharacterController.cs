using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour {

    private GameObject camera;
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


        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.rigidbody2D.velocity = new Vector3(0, 50, 0);
        }
	}

	public void changeSize(float increment){
		Vector3 scale = transform.localScale * increment;
		scale.z = transform.localScale.z;
		transform.localScale = scale;
	}
}
