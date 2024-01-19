using System.Collections.Generic;
using UnityEngine;

// High level controller for the game itself.
public class GameBoard : MonoBehaviour
{
    public const int SIZE_OF_BOARD = 50;
    public Builder builder = new Builder();
    public Node[,] nodes;
    public PathFinder pathFinder = new PathFinder();

    // Towers by location
    public Dictionary<Vector2, Unit> towers = new Dictionary<Vector2, Unit>();

    public Vector3 baseLocation = new Vector3(4.5f, 0, -4.5f);
    public Vector3 startLocation = new Vector3(-4.5f, 0, 4.5f);

    // Object used to drag and place new towers
    public Drag dragObject;

    public Spawner spawner;
    public Base baseToDefend;

    private void Start()
    {
        reset();
    }

    private void Update()
    {
        // Spawn units and also update their pathing.
        spawner.updateAndSpawnUnits(this);

        // Handle building
        if (dragObject.enabled)
        {
            dragObject.doObjectDrag(this);
        }

        // Do targeting for towers
        foreach (KeyValuePair<Vector2, Unit> tower in towers)
        {
            tower.Value.acquireTarget(spawner.activeSpawns);
        }
    }


    // Get all neighbors of a specific node
    public List<Node> getNeighborsOf(Node node)
    {
        List<Node> returnList = new List<Node>();
        if (node == null)
        {
            return returnList;
        }

        int locationX = (int)node.boardLocation.x; 
        int locationY = (int)node.boardLocation.y; 

        if (locationX > 0)
        {
            returnList.Add(nodes[locationX - 1, locationY]);
            if (locationY > 0)
            {
                returnList.Add(nodes[locationX - 1, locationY - 1]);
            }
            if (locationY < SIZE_OF_BOARD - 1)
            {
                returnList.Add(nodes[locationX - 1, locationY + 1]);
            }
        }
        if (locationX < SIZE_OF_BOARD - 1)
        {

            returnList.Add(nodes[locationX + 1, locationY]);

            if (locationY > 0)
            {
                returnList.Add(nodes[locationX + 1, locationY - 1]);
            }
            if (locationY < SIZE_OF_BOARD - 1)
            {
                returnList.Add(nodes[locationX + 1, locationY + 1]);
            }
        }

        if (locationY > 0)
        {

            returnList.Add(nodes[locationX, locationY - 1]);
        }

        if (locationY < SIZE_OF_BOARD - 1)
        {

            returnList.Add(nodes[locationX, locationY + 1]);
        }

        return returnList;
    }

    // Reset game
    public void reset()
    {
        nodes = new Node[SIZE_OF_BOARD, SIZE_OF_BOARD];
        for (int i = 0; i < SIZE_OF_BOARD; i++)
        {
            for (int j = 0; j < SIZE_OF_BOARD; j++)
            {
                nodes[i, j] = new Node();
            }
        }

        if (spawner != null)
        {
            // destroy all spawner stuff
            spawner.clearBoard();
            GameObject.Destroy(spawner.gameObject);
        }

        if (towers != null)
        {
            foreach(KeyValuePair<Vector2, Unit> tower in towers)
            {
                GameObject.Destroy(tower.Value.gameObject);
            }

            towers.Clear();
        }

        baseToDefend.resetBase();
        builder.board = this;

        // Setup nodes for mapping
        Vector3 reusableVector = new Vector3(-4.5f, 0, 4.5f);
        for (int i = 0; i < SIZE_OF_BOARD; i++)
        {
            for (int j = 0; j < SIZE_OF_BOARD; j++)
            {
                // Setup distance for A* for later.
                nodes[i, j].distanceFromBase = Vector3.Distance(reusableVector, baseLocation);
                nodes[i, j].distanceFromStart = Vector3.Distance(reusableVector, startLocation);
                nodes[i, j].realLocation = reusableVector;

                // j, i for x,y
                nodes[i, j].boardLocation = new Vector2(i, j);
                reusableVector.x += (9f / SIZE_OF_BOARD);
            }

            // So nodes correspond with somewhere on the baord
            reusableVector.x = -4.5f;
            reusableVector.z -= (9f / SIZE_OF_BOARD);
        }

        // So we can make units. Should have multiple spawner locations
        spawner = Loader.loadObjectOfType<Spawner>("Prefabs/Map/Spawner");
        spawner = GameObject.Instantiate<Spawner>(spawner, transform);
        spawner.transform.localPosition = startLocation;

        // Mark a goal. Maybe make it arbitrary...
        nodes[SIZE_OF_BOARD - 1, SIZE_OF_BOARD - 1].isGoal = true;
        nodes[SIZE_OF_BOARD - 1, SIZE_OF_BOARD - 1].occupyingUnit = baseToDefend;

        // Create inital path
        pathFinder.setupPath(this);
    }
}

