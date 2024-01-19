using System.Collections.Generic;
using UnityEngine;

// A* path finding. Not perfect but it's getting the job done.
public class PathFinder
{
    public List<Node> path = new List<Node>();
    public List<GameObject> uiPath = new List<GameObject>();

    public List<Node> open = new List<Node>();
    public List<Node> closed = new List<Node>();
    public bool isBlocked = false;
    public void setupPath(GameBoard map)
    {

        open.Clear();
        closed.Clear();

        Node currentNode = map.nodes[0,0];

        bool finishFound = false;
        open.Add(currentNode);
        isBlocked = false;
        while (open.Count > 0 && !finishFound)
        {
            open.Sort(currentNode.sortByFitness);
            currentNode = open[0];
            open.Remove(currentNode);
            closed.Add(currentNode);
            if (currentNode.isGoal)
            {
                finishFound = true;
            }
            else
            {
                List<Node> neighbors = map.getNeighborsOf(currentNode);
                neighbors.Sort(currentNode.sortByFitness);
                for (int i = 0; i < neighbors.Count; i++)
                {  
                    if (neighbors[i].occupied || closed.Contains(neighbors[i]))
                    {
                        continue;
                    }
                    else if (neighbors[i].distanceFromBase < currentNode.distanceFromBase || !open.Contains(neighbors[i]))
                    {
                        neighbors[i].parent = currentNode;
                        if (!open.Contains(neighbors[i]))
                        {
                            open.Add(neighbors[i]);
                        }
                    }
            
                }
            }
        }
        if (!finishFound)
        {
            isBlocked = true;
        }
        else
        {
            for (int i = 0; i < uiPath.Count; i++)
            {
                GameObject.Destroy(uiPath[i]);
            }
            uiPath.Clear();
            path.Clear();

            // Enable this code and link a gameobject to draw to see the path being made!
            //  GameObject reusableObject;
            while (currentNode.parent != null)
            {
                //reusableObject = GameObject.Instantiate(map.path.gameObject, map.transform);
                //reusableObject.transform.localPosition = currentNode.realLocation;
                //uiPath.Add(reusableObject);
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }
            path.Reverse();
        }
        
    }

    
}

