using UnityEngine;
using System.Collections.Generic;

public class Drone : MonoBehaviour
{
		private float maxSpeed = 1.0f;
		private Vector3 gravity = new Vector3(0, -9.81f, 0);
		private const float airResistance = 0.1f;
		private const float droneWeight = 0.1f;
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
        //direction from previous cube to cube
    	Vector3 movementNormal = Vector3.Normalize(currentEnd.transform.position - currentStart.transform.position);
        //direction from drone to cube
        Vector3 positionNormal = Vector3.Normalize(currentEnd.transform.position - gameObject.transform.position);

        //Check if the calculated direction is over 180*
        bool nextZone = (Vector3.Dot(movementNormal, positionNormal) <= 0); 
    	if (nextZone && !finished) {
    		pathProgress += 1;
            if (pathProgress < path.Count) {
			    currentStart = currentEnd;
    			currentEnd = path[pathProgress];
    		}
            else {
    			finished = true;
                //Debug.Log("Finished!");
    		}
  		} 
  		// otherwise move in direction
  		else if (!finished){
  			// assuming: constant (drone maximum speed) speed (got in direction of end cube)
  			rb.velocity = positionNormal * maxSpeed;
  			// calculate air drag
  			Vector3 dragForce = -(rb.velocity * airResistance);
  			// calculcate gravity
  			Vector3 gravityForce = gravity * droneWeight;
  			// sum vectors, calculate thrust
  			Vector3 enginePowerExpended = dragForce + gravityForce + currentStart.wind + currentStart.thermal;

            if (enginePowerExpended.y > 0)
            {
                enginePowerExpended = new Vector3(enginePowerExpended.x, 0, enginePowerExpended.z);
            }

            powerUsed += (Time.deltaTime * enginePowerExpended.magnitude) * 20.0f;
  			upt.submitPowerUsage(powerUsed, usesThermals);
  			// rotate drone to show off
  			gameObject.transform.eulerAngles = -enginePowerExpended;

  		}	else rb.velocity = new Vector3(0, 0, 0);
    } 

    public void reset(int newDestID) {
        currentStart = weather.zonesByID[startingZoneID];
        gameObject.transform.position = currentStart.transform.position;
        path = pathingScript.getPathSequence(startingZoneID, newDestID, usesThermals);
        float pathCost = 0;
        for(int i = 0; i < path.Count-1; i++)
        {
            pathCost += edgeCost(path[i], path[i + 1], usesThermals);
        }
        Debug.Log(pathCost);

        pathProgress = 0;
        powerUsed = 0.0f;
        finished = true;
    }
    public void launch() {
    	finished = false;
        currentEnd = path[pathProgress];
    }

    public float edgeCost(Zone source, Zone dest, bool thermals)
    {
        // calculate air drag - Normalised as distance is taken into account later
        Vector3 dragForce = -(Vector3.Normalize(dest.transform.position - source.transform.position)) * (maxSpeed * airResistance);
        // calculcate gravity
        Vector3 gravityForce = gravity * droneWeight;
        // sum vectors
        Vector3 enginePowerExpended = dragForce + gravityForce;

        //NOT LOCAL WIND currentStart not updated during simulation
        if (thermals)
        {
            //add wind and thermals
            enginePowerExpended += source.wind + source.thermal;
        }

        //Debug.Log("Thermal magnitude");
        //Debug.Log(localThermal().magnitude);
        //Debug.Log("gravity magnitude");
        //Debug.Log((10.0f)*gravityForce.magnitude);
        //Debug.Log("localWind magnitude");
        //Debug.Log(localWind().magnitude);
        //Debug.Log("dragforce magnitude");
        //Debug.Log(dragForce.magnitude);

        if (enginePowerExpended.y > 0)
        {
            enginePowerExpended = new Vector3(enginePowerExpended.x, 0, enginePowerExpended.z);
        }

        float flyingTime = (dest.transform.position - source.transform.position).magnitude / maxSpeed;
        float cost = flyingTime * enginePowerExpended.magnitude;
        return cost * 10.0f;
    }
}