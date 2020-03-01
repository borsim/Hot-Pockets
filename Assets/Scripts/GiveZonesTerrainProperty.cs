using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GiveZonesTerrainProperty : MonoBehaviour
{
    public Rigidbody rb;

    void Start() {
        rb = gameObject.GetComponent<Rigidbody>() as Rigidbody;
        //rb.isKinematic = false;
        //rb.isKinematic = true;
    }

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Collision!!");
        Zone collidedZone = collision.gameObject.GetComponent<Zone>() as Zone;
        if (collidedZone)
        {
            collidedZone.terrain = true;
            Debug.Log("Terrain!");
        }
    }
}
