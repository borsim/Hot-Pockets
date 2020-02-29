using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControlPanel : MonoBehaviour {

	private GameObject WTDrone;
	private Drone WTDroneScript;
	private GameObject WODrone;
	private Drone WODroneScript;
	private int currentDestinationID;
	private InputField inputField;



	void Start () {
		WTDrone = GameObject.Find("ThermalDrone");
		WTDroneScript = WTDrone.GetComponent<Drone>() as Drone;
		WODrone = GameObject.Find("NonThermalDrone");
		WODroneScript = WODrone.GetComponent<Drone>() as Drone;
		currentDestinationID = 567;
		inputField = GameObject.Find("InputField").GetComponent(typeof(InputField)) as InputField;
		inputField.onEndEdit.AddListener(delegate {saveInput(inputField); });
	}
	
	public void resetDrone(bool thermals) {
		if (thermals) {
			WTDroneScript.reset(currentDestinationID);
		} else {
			WODroneScript.reset(currentDestinationID);
		}
	}
	public void launchDrone(bool thermals) {
		if (thermals) {
			WTDroneScript.launch();
		} else {
			WODroneScript.launch();
		}
	}

	void saveInput(InputField input) {
  	currentDestinationID = int.Parse(input.text);
  }

}
