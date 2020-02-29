using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour
{
    public Vector3 thermal = new Vector3(0, 0, 0);
    public Vector3 wind = new Vector3(0, 0, 0);
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
}
