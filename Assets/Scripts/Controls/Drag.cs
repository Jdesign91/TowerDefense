using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour
{
    public Unit draggedUnit;
    RaycastHit hitLocation;
    public Vector2 size = Vector2.zero;

    public void trackDragable(Unit draggable, Vector2 newSize)
    {
        gameObject.SetActive(true);
        size = newSize;
        draggable.transform.localPosition = Vector3.zero;
        draggedUnit = draggable;
    }

    // Update is called once per frame
    void Update()
    {
        //create a ray cast and set it to the mouses cursor position in game
        if (draggedUnit != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 10000))
            {
                transform.position = hit.point;
            }
        }
    }

    public void doObjectDrag(GameBoard map)
    {
        if (draggedUnit == null)
        {
            return;
        }

        // Get board location of the drag
        Vector2 locXandY = getBoardLocation(map);
        if (locXandY.x >= 0 && locXandY.x < GameBoard.SIZE_OF_BOARD - 1 && locXandY.y >= 0 && locXandY.y < GameBoard.SIZE_OF_BOARD - 1)
        {
            // Lock the object to the grid
           transform.localPosition = map.nodes[(int)locXandY.y, (int)locXandY.x].realLocation;
        }

        // If we click
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            // Identify which locations need to be blocked
            List<Vector2> locationsToBlock = new List<Vector2>();
            bool canPlace = true;

            // cut out a slice from where we are. This will mean that objects being dragged are generally top left corner aligned...
            for (int i = (int)locXandY.y; i < locXandY.y + size.y; i++)
            {
                if (i > GameBoard.SIZE_OF_BOARD - 1)
                {
                    return;
                }

                for (int j = (int)locXandY.x; j < locXandY.x + size.x; j++)
                {
                    if (j > GameBoard.SIZE_OF_BOARD - 1 )
                    {
                        return;
                    }

                    if (map.nodes[i, j].occupied)
                    {
                        return;
                    }
                    else
                    {
                        locationsToBlock.Add(new Vector2(i, j));
                    }
                }
            }

            // if the spaces are free
            if (canPlace)
            {
                // Mark those spaces as occupied, and link them to the occupying object. Direct link might be dumb.
                for (int i = 0; i < locationsToBlock.Count; i++)
                {
                    map.nodes[(int)locationsToBlock[i].x, (int)locationsToBlock[i].y].occupied = true;
                    map.nodes[(int)locationsToBlock[i].x, (int)locationsToBlock[i].y].occupyingUnit = draggedUnit;
                    draggedUnit.onUnitDeath += map.nodes[(int)locationsToBlock[i].x, (int)locationsToBlock[i].y].onUnitDied;
                }

                draggedUnit.transform.SetParent(map.transform);
                map.towers.Add(locXandY, draggedUnit);
            }
        }
    }


    // Turns the mouse position into a board location
    public Vector2 getBoardLocation(GameBoard map)
    {
        Vector3 myWorldLoc = transform.localPosition;

        // Inverted because raycast reasons
        myWorldLoc.z *= -1;

        // ok now we're 0-9....
        myWorldLoc.x += 4.5f;
        myWorldLoc.z += 4.5f;
        
        float boardLocX = (myWorldLoc.x / 9) * GameBoard.SIZE_OF_BOARD;
        float boardLocY = (myWorldLoc.z / 9) * GameBoard.SIZE_OF_BOARD;
    
        return new Vector2((int)boardLocX, (int)boardLocY);
    }

}
