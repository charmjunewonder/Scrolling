using UnityEngine;
using System.Collections;

public class SideCollider : MonoBehaviour {

    public bool isDetached = false;

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Scalable") return;
        isDetached = true;
    }

    void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Scalable") return;

        isDetached = true;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        isDetached = false;
    }
}
