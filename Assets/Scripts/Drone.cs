using UnityEngine;
using System.Collections;

public class Drone : MonoBehaviour
{
		private float enginePower = 1.0f;
		private float maxSpeed = 10.0f;
		private Vector3 gravity = new Vector3(0, -9.81f, 0);
		private const float airResistance = 0.1f;
		private const float droneWeight = 1.0f;


		public bool usesThermals = true;
		private float powerUsed;
		private GameObject currentStart;
		private GameObject currentEnd;
		private Rigidbody rb;
		private bool finished;
		//private  PathingScript pathingScript;
		private UIPowerTracker upt;


    void Start() {
        powerUsed = 0.0f;
        rb = gameObject.GetComponent<Rigidbody>() as Rigidbody;
        finished = false;
        upt = GameObject.Find("Canvas").GetComponent<UIPowerTracker>() as UIPowerTracker;
        //pathingScript = gameObject.GetComponent<PathingScript>() as PathingScript;
    }

    void Update() {

    	Vector3 movementNormal = Vector3.Normalize(currentEnd.transform.position - currentStart.transform.position);
    	Vector3 positionNormal = Vector3.Normalize(gameObject.transform.position - currentEnd.transform.position);
    	bool nextCube = (movementNormal == positionNormal);
    	if (nextCube && !finished) {
    		currentStart = currentEnd;
    		//currentEnd = pathingScript.getNextCube();
    		if (currentEnd.transform.position == new Vector3(0, 0, 0)) finished = true;
  		} 
  		// otherwise move in direction
  		else if (!finished){
  			// assuming: constant (drone maximum speed) speed
  			rb.velocity = movementNormal * maxSpeed;
  			// calculate air drag
  			Vector3 dragForce = rb.velocity * airResistance;
  			// calculcate gravity
  			Vector3 gravityForce = gravity * droneWeight;
  			// sum vectors, calculate thrust
  			Vector3 enginePowerExpended = dragForce + gravityForce + localWind() + localThermal();

  			powerUsed = Time.deltaTime * enginePowerExpended.magnitude;
  			upt.submitPowerUsage(powerUsed, usesThermals);
  			// rotate drone to show off
  			gameObject.transform.eulerAngles = -enginePowerExpended;

  		}	
    }

    Vector3 localWind() {
    	return new Vector3(1,1,1);
    }
    Vector3 localThermal() {
    	return new Vector3(1,1,1);
    }

}