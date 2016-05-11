using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Elementals_Respawn : MonoBehaviour {
    public Dictionary<Transform, int> respawnlist = new Dictionary<Transform, int>();
    public int RespawnTime;
    public Vector2[] respawnTilesP1;
    public Vector2[] respawnTilesP2;
    private Grid _grid = null;

    public Grid grid
    {
        get
        {
            if (_grid == null)
            {
                _grid = Hierarchy.GetComponentWithTag<Grid>("Grid");
            }
            return _grid;
        }
    }
    private MovementController _movementController = null;

    public  MovementController movementController
    {
        get
        {
            if (_movementController == null)
            {
                _movementController = Hierarchy.GetComponentWithTag<MovementController>("MovementController");
            }
            return _movementController;
        }
    }
	public void CheckRespawns()
    {
        List<Transform> keys = new List<Transform>(respawnlist.Keys);
        foreach(Transform kvp in keys)
        {
            respawnlist[kvp]++;
            if( respawnlist[kvp] >= RespawnTime)
            {
                Debug.Log("respafasdwru");
                Respawn(kvp);
                respawnlist.Remove(kvp);

            }
        }
        
        foreach (Transform child in transform)
        {
            if(!child.gameObject.activeSelf)
            {
                if(!respawnlist.ContainsKey(child))
                {
                    Debug.Log("add");
                    respawnlist.Add(child, 0);
                }     
            }
        }
    }
    void SetRespawn(Vector2[] tileset, Transform elemental)
    {
        int randomTile = -1;
        bool foundTile = false;
        while (!foundTile)
        {
            randomTile++;
            foundTile = true;
            foreach (Transform child in transform)
            {
                Elemental_Movement movement = child.GetComponent<Elemental_Movement>();

                if (tileset[(int)randomTile].x == movement.currentX && tileset[(int)randomTile].y == movement.currentY)
                {

                    foundTile = false;
                    break;
                }
            }

        }
        Debug.Log(randomTile);
        elemental.gameObject.SetActive(true);
        movementController.AddUnit(elemental.GetComponent<Elemental_Properties>().id, elemental);
        elemental.GetComponent<Elemental_Movement>().SetGridPos(new Vector2((int)tileset[randomTile].x, (int)tileset   [randomTile].y));
        elemental.position = new Vector3(0, 1f, 0) + grid.GetPosition((int)tileset[randomTile].x, (int)tileset[randomTile].y);
    }

    void Respawn(Transform elemental)
    {
       
        if(elemental.GetComponent<Elemental_Properties>().id /100 == 1)
        {
            SetRespawn(respawnTilesP1, elemental);
        }
        else
        {
            SetRespawn(respawnTilesP2, elemental);
        }
    }

}
