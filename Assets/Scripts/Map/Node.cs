using UnityEngine;

// Holds info about a node in our 2d array
public class Node
{
    // Event for on tower death
    public Node parent;
    public float distanceFromBase = 0;
    public float distanceFromStart = 0;

    // Link to the object on this.
    public Unit occupyingUnit;

    public float fitnessScore
    {
        get
        {
            return distanceFromBase + distanceFromStart;
        }
    }

    public Vector3 realLocation = Vector3.zero;
    public Vector2 boardLocation = Vector2.zero;

    public bool occupied = false;
    public bool isGoal;

    public void onUnitDied(Unit unitThatDied)
    {
        occupied = false;
    }

    public int sortByFitness(Node a, Node b)
    {
        return a.fitnessScore.CompareTo(b.fitnessScore);
    }
    public int sortByDistanceFromBase(Node a, Node b)
    {
        return a.distanceFromBase.CompareTo(b.distanceFromBase);
    }
    public int sortByDistanceFromStart(Node a, Node b)
    {
        return a.distanceFromStart.CompareTo(b.distanceFromStart);
    }
}
