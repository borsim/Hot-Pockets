using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPowerTracker : MonoBehaviour {

	private Text WOPowerNumber;
	private Text WTPowerNumber;
	private RectTransform WOPowerBar;
	private RectTransform WTPowerBar;


	private float MAX_POWER_NUMBER = 500.0f;
	private const float MAX_BAR_HEIGHT = 200.0f;


	// Use this for initialization
	void Start () {
		WTPowerBar = (RectTransform)(GameObject.Find("TrackerFillerWT").GetComponent(typeof(Image)) as Image).transform;
		WOPowerBar = (RectTransform)(GameObject.Find("TrackerFillerWO").GetComponent(typeof(Image)) as Image).transform;
		WTPowerNumber = GameObject.Find("PowerWTNumber").GetComponent<Text>() as Text;
		WOPowerNumber = GameObject.Find("PowerWONumber").GetComponent<Text>() as Text;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void submitPowerUsage(float powerUsedThisRun, bool thermals) {
        if (powerUsedThisRun * 1.2f >= MAX_POWER_NUMBER) MAX_POWER_NUMBER += 100;
		if (thermals) {
			WTPowerNumber.text = (Mathf.Round(powerUsedThisRun)).ToString();
			WTPowerBar.sizeDelta = new Vector2(80f, (powerUsedThisRun / MAX_POWER_NUMBER) * MAX_BAR_HEIGHT);
		} else {
			WOPowerNumber.text = (Mathf.Round(powerUsedThisRun)).ToString();
			WOPowerBar.sizeDelta = new Vector2(80f, (powerUsedThisRun / MAX_POWER_NUMBER) * MAX_BAR_HEIGHT);
		}
	}

}
