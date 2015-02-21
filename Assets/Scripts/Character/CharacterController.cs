using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour {
    public GameObject scalableObject;
    public bool hasScalableObject;
    private float deltaLength;
    private GameObject camera;
    private int jumpCount = 0;
    private Vector2 prevol;
    private float topAngle;
    private float sideAngle;
    private bool[] flags;
	// Use this for initialization
	void Start () {
        camera = GameObject.Find("Main Camera");
        prevol = Vector2.zero;
        Vector2 size = GetComponent<BoxCollider2D>().size;
        size = Vector2.Scale(size, (Vector2)transform.localScale);
        topAngle = Mathf.Atan(size.x / size.y) * Mathf.Rad2Deg;
        sideAngle = 90.0f - topAngle;
        flags = new bool[4];
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

    void FixedUpdate()
    {
        Debug.Log("top " + flags[0] + ", right " + flags[1] + ", left " + flags[2] + ", bottom " + flags[3]);
        allowToScale();
        for (int i = 0; i < 4; i++)
        {
            flags[i] = false;
        }
    }

    public bool allowToScale()
    {
        if (flags[0] && flags[3])
        {
            Debug.Log("Vertical");
            return false;
        }
        if (flags[0] && flags[3])
        {
            Debug.Log("Horizontal");
            return false;
        }
        return true;
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
        for (int i = 0; i < coll.contacts.Length; i++)
        {
            Vector3 v = (Vector3)coll.contacts[i].point - transform.position;

            if (Vector3.Angle(v, transform.up) <= topAngle)
            {
                flags[0] = true;//top
            }
            else if (Vector3.Angle(v, transform.right) <= sideAngle)
            {
                flags[1] = true;//right
            }
            else if (Vector3.Angle(v, -transform.right) <= sideAngle)
            {
                flags[2] = true;//left
            }
            else
            {
                flags[3] = true;//bottom
            }
        }
    }

    void OnCollisionStay2D(Collision2D coll)
    {

        Debug.Log("Lenth: "+coll.contacts.Length);
        for (int i = 0; i < coll.contacts.Length; i++)
        {
            Vector3 v = (Vector3)coll.contacts[i].point - transform.position;

            if (Vector3.Angle(v, transform.up) <= topAngle)
            {
                flags[0] = true;//top
            }
            else if (Vector3.Angle(v, transform.right) <= sideAngle)
            {
                flags[1] = true;//right
            }
            else if (Vector3.Angle(v, -transform.right) <= sideAngle)
            {
                flags[2] = true;//left
            }
            else
            {
                flags[3] = true;//bottom
            }
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
