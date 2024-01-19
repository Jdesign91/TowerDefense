using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Builder
{
    public GameBoard board;

    // In the future I can imagine adding other building types here. Blockades and whatnot perhaps.
    public Tower buildTower(TowerInfo tower)
    {
        Tower loadedTower = Resources.Load<Tower>(tower.resourcePath);
        loadedTower = GameObject.Instantiate<Tower>(loadedTower, board.dragObject.transform);
        loadedTower.info = tower;
        board.dragObject.trackDragable(loadedTower, loadedTower.size);

        return loadedTower;
    }
}

