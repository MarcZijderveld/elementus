using UnityEngine;
using System.Collections;

public class HexagonCycle : MonoBehaviour {
    Grid grid;
    public int number;
    public int x, y;
	// Use this for initialization
	void Start () {
	    grid = GameObject.Find("GridCreator").GetComponent<Grid>();
	}
	
	// Update is called once per frame
	public void Change () 
    {
        if (grid.editing == true)
        {
            number++;
            grid.changeHexagon(number, x, y);
            Destroy(gameObject);

        }
	}

    public void Change2()
    {
        if (grid.editing == true)
        {
            number--;
            grid.changeHexagon(number, x, y);
            Destroy(gameObject);

        }
    }
}
