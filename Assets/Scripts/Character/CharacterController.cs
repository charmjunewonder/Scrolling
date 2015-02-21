using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour {
    public GameObject scalableObject;
    public bool hasScalableObject;
    private float deltaLength;
    private GameObject camera;
    private int jumpCount = 0;
    private Vector2 prevol;

	// Use this for initialization
	void Start () {
        camera = GameObject.Find("Main Camera");
        prevol = Vector2.zero;

	}
	
	// Update is called once per frame
	void Update () {

        prevol = transform.rigidbody2D.velocity;  
		if (Input.GetKey (KeyCode.A)) {
			/*Vector3 pos = transform.position;
            pos.x -= 0.5f * camera.GetComponent<Camera>().orthographicSize / 50;
			transform.position = pos;*/
            transform.rigidbody2D.velocity = new Vector2(-30 * camera.GetComponent<Camera>().orthographicSize / 50,  transform.rigidbody2D.velocity.y);

		}

		if (Input.GetKey (KeyCode.D)) {
			/*Vector3 pos = transform.position;
            pos.x += 0.5f * camera.GetComponent<Camera>().orthographicSize / 50;
			transform.position = pos;
			*/
            transform.rigidbody2D.velocity = new Vector2(30 * camera.GetComponent<Camera>().orthographicSize / 50, transform.rigidbody2D.velocity.y);
		}

        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < 3)
        {
            Debug.Log(camera.GetComponent<Camera>().orthographicSize);
            transform.rigidbody2D.velocity = new Vector2(0, (50 + camera.GetComponent<Camera>().orthographicSize) / 2);
            jumpCount++;
        }

        /*if (hasScalableObject)
        {
            Vector3 pos = scalableObject.transform.position;
            pos.x = transform.position.x + deltaLength;
            scalableObject.transform.position = pos;
        }*/
	}

    public void recordDelta()
    {
        Vector3 pos = scalableObject.transform.position;
        deltaLength = pos.x - transform.position.x;
    }

	public void changeSize(float increment){
		Vector3 scale = transform.localScale * increment;
		scale.z = transform.localScale.z;
		transform.localScale = scale;
        //if (hasScalableObject)
        //{
        //    changeSize(scalableObject, increment);
        //}
	}

    public void changePosition(Vector2 groundPos, float scale)
    {
        Vector3 pos = transform.position;
        pos.y = groundPos.y + (pos.y - groundPos.y) * scale;
        transform.position = pos;
        //if (hasScalableObject)
        //{
        //    changePosition(scalableObject, groundPos, scale);
        //}
    }

    public void changeSize(GameObject ob, float increment)
    {
        Vector3 scale = ob.transform.localScale * increment;
        scale.z = ob.transform.localScale.z;
        ob.transform.localScale = scale;
    }

    public void changePosition(GameObject ob, Vector2 groundPos, float scale)
    {
        Vector3 pos = ob.transform.position;
        pos.y = groundPos.y + (pos.y - groundPos.y) * scale;
        float xLength = pos.x - transform.position.x;
        pos.x = transform.position.x + xLength * scale;
        ob.transform.position = pos;
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
            //transform.rigidbody2D.velocity = new Vector2(30 * camera.GetComponent<Camera>().orthographicSize / 50, transform.rigidbody2D.velocity.y);
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "FallingTrigger")
        {
            camera.GetComponent<CameraController>().fakeToBlack();
        }
        else if (coll.gameObject.tag == "UpTrigger")
        {
            camera.GetComponent<CameraController>().fakeToBlackUp();
        }
    }
}
