using UnityEngine;
using UnityEngine.Time;
using System.Collections;

public class Drone : MonoBehaviour
{
		private float enginePower = 1.0f;
		private float maxSpeed = 10.0f;
		private const Vector3 gravity = new Vector3(0, -9.81f, 0);
		private const float airResistance = 0.1;
		private const float droneWeight = 1.0f;



		private float powerUsed;
		private GameObject currentStart;
		private GameObject currentEnd;
		private RigidBody rb;
		private Boolean finished;
		//private  PathingScript pathingScript;


    void Start() {
        powerUsed = 0.0f;
        rb = gameObject.GetComponent<RigidBody>() as RigidBody;
        finished = false;
        //pathingScript = gameObject.GetComponent<PathingScript>() as PathingScript;
    }

    void Update() {

    	Vector3 movementNormal = Vector3.Normalize(new Vector3(currentEnd.transform.position - currentStart.transform.position));
    	Vector3 positionNormal = Vector3.Normalize(new Vector3(gameObject.transform.position - currentEnd.transform.position));
    	Boolean nextCube = (movementNormal == positionNormal);
    	if (nextCube && !finished) {
    		currentStart = currentEnd;
    		currentEnd = pathingScript.getNextCube();
    		if (currentEnd == new Vector3(0, 0, 0)) finished = true;
  		} 
  		// otherwise move in direction
  		else if (!finished){
  			// assuming: constant (drone maximum speed) speed
  			rb.velocity = movementNormal * maxSpeed;
  			// calculate air drag
  			dragForce = rb.velocity * airResistance;
  			// calculcate gravity
  			gravityForce = gravity * droneWeight;
  			// sum vectors, calculate thrust
  			enginePowerExpended = dragForce + gravityForce + localWind() + localThermal();

  			powerUsed = Time.deltaTime() * enginePowerExpended.magnitude;
  			// rotate drone to show off
  			gameObject.transform.eulerAngles = -enginePowerExpended;

  		}	
    }

    Vector3 localWind() {

    }
    Vector3 localThermal() {

    }

}