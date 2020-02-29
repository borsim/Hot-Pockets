using UnityEngine;
using System.Collections.Generic;

public class Drone : MonoBehaviour
{
		private float maxSpeed = 20.0f;
		private Vector3 gravity = new Vector3(0, -9.81f, 0);
		private const float airResistance = 0.1f;
		private const float droneWeight = 10.0f;
		public const int startingZoneID = 7;
		public const int baseDestinationZoneID = 567;


		public bool usesThermals = true;
		private float powerUsed;
		private Zone currentStart;
		private Zone currentEnd;
		private Rigidbody rb;
		private bool finished = true;
		private  PathingScript pathingScript;
		private List<Zone> path;
		private int pathProgress;
		private UIPowerTracker upt;
		private Weather weather;


    void Start() {
        powerUsed = 0.0f;
        rb = gameObject.GetComponent<Rigidbody>() as Rigidbody;
        finished = true;
        upt = GameObject.Find("Canvas").GetComponent<UIPowerTracker>() as UIPowerTracker;
        weather = GameObject.Find("Weather").GetComponent<Weather>() as Weather;
        pathingScript = gameObject.GetComponent<PathingScript>() as PathingScript;
        currentStart = weather.zonesByID[startingZoneID];
        currentEnd = weather.zonesByID[baseDestinationZoneID];
        path = new List<Zone>();
        pathProgress = 0;
    }

    void Update() {
    	Vector3 movementNormal = Vector3.Normalize(currentEnd.transform.position - currentStart.transform.position);
    	Vector3 positionNormal = Vector3.Normalize(gameObject.transform.position - currentEnd.transform.position);
    	bool nextZone = (movementNormal == positionNormal);
    	if (nextZone && !finished) {
    		pathProgress += 1;
    		if (pathProgress < path.Count) {
					currentStart = currentEnd;
    			currentEnd = path[pathProgress];
    		}	else {
    			finished = true;
    		}
  		} 
  		// otherwise move in direction
  		else if (!finished){
  			// assuming: constant (drone maximum speed) speed
  			rb.velocity = movementNormal * maxSpeed;
  			// calculate air drag
  			Vector3 dragForce = -(rb.velocity * airResistance);
  			// calculcate gravity
  			Vector3 gravityForce = gravity * droneWeight;
  			// sum vectors, calculate thrust
  			Vector3 enginePowerExpended = dragForce + gravityForce + localWind() + localThermal();

  			powerUsed += Time.deltaTime * enginePowerExpended.magnitude;
  			upt.submitPowerUsage(powerUsed, usesThermals);
  			// rotate drone to show off
  			gameObject.transform.eulerAngles = -enginePowerExpended;

  		}	else rb.velocity = new Vector3(0, 0, 0);
    } 

    public void reset(int newDestID) {
    	currentStart = weather.zonesByID[startingZoneID];
    	gameObject.transform.position = currentStart.transform.position;
      path = pathingScript.getPathSequence(startingZoneID, newDestID);
      pathProgress = 0;
      powerUsed = 0.0f;
      finished = true;
    }
    public void launch() {
    	finished = false;
    }

    public float edgeCost(Zone source, Zone dest) {
  			// calculate air drag
  			Vector3 dragForce = -(Vector3.Normalize(dest.transform.position - source.transform.position)) * (maxSpeed * airResistance);
  			// calculcate gravity
  			Vector3 gravityForce = gravity * droneWeight;
  			// sum vectors, calculate thrust
  			Vector3 enginePowerExpended = dragForce + gravityForce + localWind() + localThermal();
  			float flyingTime = (source.transform.position - dest.transform.position).magnitude / maxSpeed;
  			float cost = flyingTime * enginePowerExpended.magnitude;
  			return cost;
    }

    private Vector3 localWind() {
    	return currentStart.wind;
    }
    private Vector3 localThermal() {
    	return currentStart.thermal;
    }

}