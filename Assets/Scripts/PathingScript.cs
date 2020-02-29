using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathingScript : MonoBehaviour {



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//Returns the shortest distance between two nodes - all edge weights are 1
  private float dijkstra(Zone source, Zone destination)
  {
  	int numCubes = 20*20*20;
  	float[] distances = new float[numCubes];
  	DijkstraNode dSource = new DijkstraNode(1);
  	DijkstraNode dDestination = new DijkstraNode(234);
  	for (int i=0; i<numCubes; i++)
  	{
  		distances[i] = Mathf.Infinity;
  	}
  	distances[dSource.identifier] = 0;
  	//This uses two new classes: DijkstraNode is simply a pair of identifier and distance
  	//DijkstraNodeComparator sorts DijkstraNodes by distance
  	//so that the priority queue always returns the closest node
  	List<DijkstraNode> dijkstraNodes = new List<DijkstraNode>();
  	//Start from the source node
  	DijkstraNode currentNode = dSource;

  	//while (!currentNode.equals(dDestination))
  	while (currentNode.identifier != dDestination.identifier)
  	{
  		//Get all edges from the current node
  		Edge[] newEdges = new Edge[26];
  		////////////// TODO get current available edges
  		// currentCube.getEdges();
  		//Convert edge destinations to DijkstraNodes with the updated distance
  		foreach (Edge e in newEdges) {
  			DijkstraNode newDNode = new DijkstraNode(e.endIdentifier);
  			// TODO add edge distance from model
  			newDNode.distance = currentNode.distance + 1;

  			//Check if node in question has already been added into the search tree with a
  			//longer route. If yes, replace it with a shorter one
  			if (dijkstraNodes.Contains(newDNode) && newDNode.distance < distances[newDNode.identifier]) {
  				//equals() has been defined for DNodes based on identifier attribute
  				dijkstraNodes.Remove(newDNode);
  				distances[newDNode.identifier] = newDNode.distance;

  				dijkstraNodes.Add(newDNode);
  			}
  			else if (distances[newDNode.identifier] == Mathf.Infinity) {
  				distances[newDNode.identifier] = newDNode.distance;
  				dijkstraNodes.Add(newDNode);
  			}
  		}

  		//Pop the head node **TODO**
  		//Sort list
  		dijkstraNodes.Sort(new DijkstraNodeComparator());
  		currentNode = dijkstraNodes[0];
  		dijkstraNodes.Remove(currentNode);
  		//currentNode = dijkstraNodes.Poll();
  	}

  	//Once the destionation node is currentNode, return the distance
  	return currentNode.distance;
  }

  public class DijkstraNode : System.IEquatable<DijkstraNode>{
		public float distance;
		public int identifier;

		public DijkstraNode(int initIdentifier) {
			identifier = initIdentifier;
			distance = 0.0f;
		}
		public bool Equals(DijkstraNode other) {
        if (other == null) return false;
        return (this.identifier.Equals(other.identifier));
    }
	}
	public class Edge {
		public float distance;
		public int startIdentifier;
		public int endIdentifier;

		public Edge(int IstartIdentifier, int IendIdentifier,  float initDistance) {
			startIdentifier = IstartIdentifier;
			endIdentifier = IendIdentifier;
			distance = initDistance;
		}
	}

	public class DijkstraNodeComparator : IComparer<DijkstraNode> {
		public int Compare(DijkstraNode a, DijkstraNode b) {
			return (int)Mathf.Round(a.distance - b.distance);
		}
	}
}
