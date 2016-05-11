using UnityEngine;
using System.Collections;

public class CrystalsSpawning : MonoBehaviour
{
    public Transform crystal;
    private Grid _grid = null;
    public bool go = false;
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
    private void Update()
    {
        if(go)
        {
            Spawn();
            go = false;
        }
    }

    public void Spawn()
    {
        if (Random.value * 10 < 2)
        {
            int x = (int)(Random.value * grid.gridWidth);
            int y = (int)(Random.value * grid.gridHeight);
            while (!grid.GetAvailable(x, y))
            {
                x = (int)(Random.value * grid.gridWidth);
                y = (int)(Random.value * grid.gridHeight);
            }
            SpawnCrystal(x, y);
        }
    }

    void SpawnCrystal(int x,int y)
    {
       Instantiate(crystal, grid.GetPosition(x, y),transform.rotation);
    }
}
