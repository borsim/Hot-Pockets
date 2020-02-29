using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weather : MonoBehaviour
{
    public static int dimX = 3;
    public static int dimY = 3;
    public static int dimZ = 3;

    public GameObject cube;
    public GameObject[,,] cubes = new GameObject[dimX, dimY, dimZ];

    // Start is called before the first frame update
    void Start()
    {
        int id = 0;
        for (int i = 0; i < dimX; i++){
            for (int j = 0; j < dimY; j++){
                for (int k = 0; k < dimZ; k++){
                    cubes[i, j, k] = Instantiate(cube, new Vector3(i, j, k), Quaternion.identity);
                    cubes[i, j, k].GetComponent<Zone>().assignId(id);
                    id++;
                }
            }
        }

        for (int n = 0; n < dimX * dimY * dimZ; n++)
        {
            int index = 0;
            int a = (n / 9) % 3;
            int b = (n / 3) % 3;
            int c = n % 3;
            GameObject current = cubes[a, b, c];
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
                            current.GetComponent<Zone>().addNeighbour(cubes[i, j, k], index);
                        }
                        index++;
                    }
                }
            }
        }

        GameObject centre = cubes[1, 1, 1];
        for (int i = 0; i < dimX; i++)
        {
            for (int j = 0; j < dimX; j++)
            {
                for (int k = 0; k < dimX; k++)
                {
                    GameObject neighbour = centre.GetComponent<Zone>().neighbours[i, j, k];
                    if (neighbour != null)
                    {
                        int neighbourId = neighbour.GetComponent<Zone>().id;
                        Debug.Log(neighbourId);
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
