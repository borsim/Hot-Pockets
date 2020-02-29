using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weather : MonoBehaviour
{
    public static int dimX = 15;
    public static int dimY = 15;
    public static int dimZ = 15;
    public GameObject zone;
    public GameObject[,,] zones = new GameObject[dimX, dimY, dimZ];
    public GameObject[] sources = new GameObject[3];

    // Start is called before the first frame update
    void Start()
    {
        int id = 0;
        for (int i = 0; i < dimX; i++){
            for (int j = 0; j < dimY; j++){
                for (int k = 0; k < dimZ; k++){
                    zones[i, j, k] = Instantiate(zone, new Vector3(i, j, k), Quaternion.identity);
                    zones[i, j, k].GetComponent<Zone>().assignId(id);
                    id++;
                }
            }
        }

        for (int n = 0; n < dimX * dimY * dimZ; n++)
        {
            int index = 0;
            int a = (n / (dimX*dimY)) % dimX;
            int b = (n / dimY) % dimY;
            int c = n % dimZ;
            GameObject current = zones[a, b, c];
            for (int i = a-1; i < a+2; i++)
            {
                for (int j = b-1; j < b+2; j++)
                {
                    for (int k = c-1; k < c+2; k++)
                    {
                        if (i < 0 || i >= dimX 
                            || j < 0 || j >= dimY 
                            || k < 0 || k >= dimZ
                            || (i == a && j == b && k == c))
                        {
                            current.GetComponent<Zone>().addNeighbour(null, index);
                        }
                        else
                        {
                            current.GetComponent<Zone>().addNeighbour(zones[i, j, k], index);
                        }
                        index++;
                    }
                }
            }
        }
        
        generateHeat();
        generateWind(0);
    }

    // Update is called once per frame
    void Update()
    {
        spreadHeat();
    }

    void generateHeat()
    {
        for (int i = 0; i < 3; i++)
        {
            int x = Random.Range(0, dimX);
            int y = Random.Range(0, dimY/4);
            int z = Random.Range(0, dimZ);
            zones[x, y, z].GetComponent<Zone>().temperature = 40;
            sources[i] = zones[x, y, z];
        }
    }

    void spreadHeat()
    {
        foreach (GameObject source in sources)
        {
            source.GetComponent<Zone>().temperature = 40;
            source.GetComponent<Zone>().spreadHeat();
        }

        for (int i = 0; i < dimX; i++)
        {
            for (int j = 0; j < dimY; j++)
            {
                for (int k = 0; k < dimZ; k++)
                {
                    zones[i, j, k].GetComponent<Zone>().spreadHeat();
                }
            }
        }

        for (int i = 0; i < dimX; i++)
        {
            for (int j = 0; j < dimY; j++)
            {
                for (int k = 0; k < dimZ; k++)
                {
                    zones[i, j, k].GetComponent<Zone>().finaliseHeat();
                    float alpha = 0f + zones[i, j, k].GetComponent<Zone>().temperature;
                    if (alpha > 0.5) alpha = 0.5f;
                    zones[i, j, k].GetComponent<MeshRenderer>().material.color = new Color(1.0f, 0f, 0f, alpha);
                    if (zones[i, j, k].GetComponent<Zone>().wind != new Vector3(0, 0, 0)) zones[i, j, k].GetComponent<MeshRenderer>().material.color = new Color(0f, 0f, 1.0f);
                }
            }
        }
    }

    void generateWind(int direction)
    {
        for (int i = 0; i < dimX; i++)
        {
            for (int j = 0; j < dimY; j++)
            {
                switch (direction)
                {
                    case 0:
                        zones[i, j, 0].GetComponent<Zone>().wind.z = 1;
                        break;
                    case 1:
                        zones[0, i, j].GetComponent<Zone>().wind.x = 1;
                        break;
                    case 2:
                        zones[i, j, dimX].GetComponent<Zone>().wind.z = -1;
                        break;
                    case 3:
                        zones[dimX, i, j].GetComponent<Zone>().wind.x = -1;
                        break;
                }
            }
        }
    }
}
