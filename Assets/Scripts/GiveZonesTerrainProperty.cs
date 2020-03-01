using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GiveZonesTerrainProperty : MonoBehaviour
{
    public Rigidbody rb;

    void Start() {
        rb = gameObject.GetComponent<Rigidbody>() as Rigidbody;
        rb.isKinematic = false;
        rb.isKinematic = true;
    }

    void OnCollisionEnter(Collision collision) {
        Zone collidedZone = collision.gameObject.GetComponent<Zone>() as Zone;
        if (collidedZone) {
            collidedZone.terrain = true;
        }
    }

}
