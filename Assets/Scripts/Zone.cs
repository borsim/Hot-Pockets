using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour
{
    public Vector3 thermal = new Vector3(0, 0, 0);
    public Vector3 wind = new Vector3(0, 0, 0);
    public float temperature = 0;
    public float tempTemperature = 0;
    public Vector3 tempWind = new Vector3(0, 0, 0);
    public GameObject[,,] neighbours = new GameObject[3, 3, 3];
    public int id;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addNeighbour(GameObject neighbour, int index)
    {
        neighbours[(index / 9) % 3, (index/3)%3, index%3] = neighbour;
    }

    public void assignId(int newId)
    {
        id = newId;
    }

    public void spreadHeat()
    {
        float l0 = temperature * 0.2f;
        float[] levels = {temperature * 0.00925f, temperature * 0.0025f, temperature * 0.0125f};
        float l4 = temperature * 0.005f;

        if (neighbours[(10 / 9) % 3, (10 / 3) % 3, 10 % 3] != null) neighbours[(10 / 9) % 3, (10 / 3) % 3, 10 % 3].GetComponent<Zone>().addHeat(l4);
        if (neighbours[(16 / 9) % 3, (16 / 3) % 3, 16 % 3] != null) neighbours[(16 / 9) % 3, (16 / 3) % 3, 16 % 3].GetComponent<Zone>().addHeat(l0);
        for (int i = 0; i < 7; i += 3)
        {
            float level = levels[i / 3];
            if (neighbours[(i / 9) % 3, (i / 3) % 3, i % 3] != null) neighbours[(i / 9) % 3, (i / 3) % 3, i % 3].GetComponent<Zone>().addHeat(level);
            if (neighbours[((i + 1) / 9) % 3, ((i + 1) / 3) % 3, (i + 1) % 3] != null) neighbours[((i + 1) / 9) % 3, ((i + 1) / 3) % 3, (i + 1) % 3].GetComponent<Zone>().addHeat(level);
            if (neighbours[((i + 2) / 9) % 3, ((i + 2) / 3) % 3, (i + 2) % 3] != null) neighbours[((i + 2) / 9) % 3, ((i + 2) / 3) % 3, (i + 2) % 3].GetComponent<Zone>().addHeat(level);
            if (neighbours[((i + 9) / 9) % 3, ((i + 9) / 3) % 3, (i + 9) % 3] != null) neighbours[((i + 9) / 9) % 3, ((i + 9) / 3) % 3, (i + 9) % 3].GetComponent<Zone>().addHeat(level);
            if (neighbours[((i + 11) / 9) % 3, ((i + 11) / 3) % 3, (i + 11) % 3] != null) neighbours[((i + 11) / 9) % 3, ((i + 11) / 3) % 3, (i + 11) % 3].GetComponent<Zone>().addHeat(level);
            if (neighbours[((i + 18) / 9) % 3, ((i + 18) / 3) % 3, (i + 18) % 3] != null) neighbours[((i + 18) / 9) % 3, ((i + 18) / 3) % 3, (i + 18) % 3].GetComponent<Zone>().addHeat(level);
            if (neighbours[((i + 19) / 9) % 3, ((i + 19) / 3) % 3, (i + 19) % 3] != null) neighbours[((i + 19) / 9) % 3, ((i + 19) / 3) % 3, (i + 19) % 3].GetComponent<Zone>().addHeat(level);
            if (neighbours[((i + 20) / 9) % 3, ((i + 20) / 3) % 3, (i + 20) % 3] != null) neighbours[((i + 20) / 9) % 3, ((i + 20) / 3) % 3, (i + 20) % 3].GetComponent<Zone>().addHeat(level);
        }
    }

    public void addHeat(float increase)
    {
        tempTemperature += increase;
    }

    public void finaliseHeat()
    {
        temperature /= 2;
        temperature += tempTemperature;
        thermal.y = temperature;
        tempTemperature = 0;
    }

    public void spreadWind()
    {
        if (wind.z > 0 && neighbours[(14 / 9) % 3, (14 / 3) % 3, 14 % 3] != null) neighbours[(14 / 9) % 3, (14 / 3) % 3, 14 % 3].GetComponent<Zone>().addWind(0.8f * wind.z, 'z');
        if (wind.z < 0 && neighbours[(12 / 9) % 3, (12 / 3) % 3, 12 % 3] != null) neighbours[(12 / 9) % 3, (12 / 3) % 3, 12 % 3].GetComponent<Zone>().addWind(0.8f * wind.z, 'z');
        if (wind.x > 0 && neighbours[(22 / 9) % 3, (22 / 3) % 3, 22 % 3] != null) neighbours[(22 / 9) % 3, (22 / 3) % 3, 22 % 3].GetComponent<Zone>().addWind(0.8f * wind.x, 'x');
        if (wind.x < 0 && neighbours[(4 / 9) % 3, (4 / 3) % 3, 4 % 3] != null) neighbours[(4 / 9) % 3, (4 / 3) % 3, 4 % 3].GetComponent<Zone>().addWind(0.8f * wind.x, 'x');
    }

    public void addWind(float increase, char dir)
    {
        switch (dir)
        {
            case 'x':
                tempWind.x = increase;
                break;
            case 'z':
                tempWind.z = increase;
                break;
        }
    }

    public void finaliseWind()
    {   
        if(tempWind != new Vector3(0, 0, 0))
        {
            wind = tempWind;
            tempWind = new Vector3(0, 0, 0);
        }
    }
}
