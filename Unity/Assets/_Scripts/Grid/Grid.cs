using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Grid : MonoBehaviour
{
    public bool editing;
	public float gridWidth = 0, gridHeight = 0;
	public Transform[] Hexagon = new Transform[2];
	public Transform ScoreHexagon;
	public Transform KingHexagon;
    public Transform MainKingHexagon;
	public Transform EmptyHexagon;
    public Transform[] Partical = new Transform[2];
	public Vector2[] scorePositions = new Vector2[2];
	public Vector2[] kingPositions = new Vector2[2];
	public GameObject[,] grid = null;
    private float xDistance = 1.0919307f, yDistance = 1.22055645f;

	public List<GridElement> gridElements = new List<GridElement>();

	[System.Serializable]
	public class GridElement
	{
		public string name;
		public GameObject obj;
		public Vector2 gridPos;
	}
    public Vector2 getDistanceBetweenTiles()
    {
        return new Vector2(xDistance, yDistance);
    }

	// Use this for initialization
	void Start ()
	{
        grid = new GameObject[(int)gridWidth, (int)gridHeight];
        if (editing)
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
            gridElements.Clear();

            //create the hexagon grid
           
            Vector3 postion;
            Random.seed = 42;
            int Width = ((int)gridWidth - 1) / 2;
            int Original = Width;
            for (int itteratorX = 0; itteratorX < gridWidth; itteratorX++)
            {
                float offsetX = 0;
                if (itteratorX % 2 == 1)
                {
                    offsetX = xDistance / 2f;
                }

                for (int itteratorY = 0; itteratorY < gridHeight; itteratorY++)
                {
                    postion = new Vector3(itteratorX * xDistance, 0, (itteratorY * yDistance) - offsetX);
                    Transform hexagon = Instantiate(Hexagon[0], postion, transform.rotation) as Transform;
                    hexagon.GetComponent<HexagonCycle>().number = 0;
                    hexagon.GetComponent<HexagonCycle>().x = itteratorX;
                    hexagon.GetComponent<HexagonCycle>().y = itteratorY;
                    hexagon.parent = transform;
                    grid[itteratorX, itteratorY] = hexagon.gameObject;
                  


                }

            }
            for (int i = 0; i < kingPositions.Length; i++)
            {
                Vector3 position = grid[(int)kingPositions[i].x, (int)kingPositions[i].y].transform.position;
                Destroy(grid[(int)kingPositions[i].x, (int)kingPositions[i].y]);
                Transform hexagon;
                if(i == 0)
                    hexagon = Instantiate(MainKingHexagon, position, transform.rotation) as Transform;
                else
                    hexagon = Instantiate(KingHexagon, position, transform.rotation) as Transform;
                hexagon.parent = transform;
                grid[(int)kingPositions[i].x, (int)kingPositions[i].y] = hexagon.gameObject;
            }

            for (int i = 0; i < scorePositions.Length; i++)
            {
                Vector3 position = grid[(int)scorePositions[i].x, (int)scorePositions[i].y].transform.position;
                Destroy(grid[(int)scorePositions[i].x, (int)scorePositions[i].y]);
                Transform hexagon = Instantiate(ScoreHexagon, position, transform.rotation) as Transform;
                hexagon.parent = transform;
                grid[(int)scorePositions[i].x, (int)scorePositions[i].y] = hexagon.gameObject;

                Transform particle = Instantiate(Partical[i], position, Partical[i].rotation) as Transform;
                particle.parent = transform;
            }

            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    GridElement elem = new GridElement();
                    elem.gridPos = new Vector2(x, y);
                    elem.obj = grid[x, y];
                    elem.name = elem.gridPos.ToString();
                    gridElements.Add(elem);
                   
                }
            }
        }
        else
        {
            foreach(GridElement element in gridElements)
            {
                grid[(int)element.gridPos.x,(int)element.gridPos.y] = element.obj;
               // element.obj.GetComponent<HighLight>().enabled = true;
                if (element.obj.tag == "Empty")
                {
                    element.obj.GetComponent<BoxCollider>().enabled = false;
                }
            }
        }
	}

	public void Highlight (int x, int y, int radius)
	{

			//check if the the array wont go out of bounds
			if (x >= 0 && x < gridWidth && y >= 0 && y < gridHeight) {
					//Debug.Log("HIGHLIGHTED");
                grid[x,y].GetComponent<HighLight>().highlight = true;
			} else {
					//Debug.LogWarning("RETURNING");
					//if the array is out of bounds stop seaching
					return;
			}
			radius--;     
			//do Recusion until the movement runs out.
			if (radius > 0) {
					//Debug.LogWarning("Radius is bigger then zero");
					Highlight (x, y + 1, radius);            
					Highlight (x + 1, y, radius);
					Highlight (x, y - 1, radius);           
					Highlight (x - 1, y, radius);

					//do the hexagon ofset;
					if (x % 2 == 1) {
							Highlight (x + 1, y - 1, radius);
							Highlight (x - 1, y - 1, radius);
					} else {
							Highlight (x + 1, y + 1, radius);
							Highlight (x - 1, y + 1, radius);
					}
			}
	}


	public Vector3 GetMiddle()
	{
		return GetPosition(Mathf.FloorToInt(gridWidth / 2), Mathf.FloorToInt(gridHeight / 2));	
	}
	
	public Vector3 GetPosition (int x, int y)
	{   
		// get the position of the tile in the world
		if (x >= 0 && x <= gridWidth && y >= 0 && y <= gridHeight)
				return grid [x, y].transform.position;
		else
				return new Vector3 (0, 0, 0);
	}

    public bool GetAvailable(int x, int y)
    {
        // get the position of the tile in the world
        if (grid[x, y].transform.tag == "Hexagon")
            return true;
        else
            return false;
    }

	public Vector2 GetArrayLocation (GameObject tile)
	{
		//get the position of a tile in the array
		for (int itteratorY = 0; itteratorY < gridHeight; itteratorY++) {

				for (int itteratorX = 0; itteratorX < gridWidth; itteratorX++) {
						if (grid [itteratorX, itteratorY] == tile)
								return new Vector2 (itteratorX, itteratorY);
				}

		}
		return new Vector2 (0, 0);
	}

	public void ResetHighlight ()
	{
		for (int itteratorY = 0; itteratorY < gridHeight; itteratorY++) {

				for (int itteratorX = 0; itteratorX < gridWidth; itteratorX++) {
						grid [itteratorX, itteratorY].GetComponent<HighLight> ().highlight = false;
						grid [itteratorX, itteratorY].GetComponent<HighLight> ().mousover = false;
				}
		}
	}

	public void ResetMouseOverHighlight ()
	{
        if (!editing)
        {
            for (int itteratorY = 0; itteratorY < gridHeight; itteratorY++)
            {

                for (int itteratorX = 0; itteratorX < gridWidth; itteratorX++)
                {
                    grid[itteratorX, itteratorY].GetComponent<HighLight>().mousover = false;
                }
            }
        }
	}

    public void changeHexagon(int number, int x, int y)
    {
        foreach (GridElement element in gridElements)
        {
            if ((int)element.gridPos.x == x && (int)element.gridPos.y == y)
            {
             
                if (number + 1 > Hexagon.Length)
                    number = 0;
                if (number < 0)
                    number = Hexagon.Length -1;

                Debug.Log(number);
                Vector3 postion = element.obj.transform.position;
                element.obj = (Instantiate(Hexagon[number], postion, transform.rotation) as Transform).gameObject;
                element.obj.transform.parent = transform;
                element.obj.GetComponent<HexagonCycle>().number = number;
                element.obj.GetComponent<HexagonCycle>().x = x;
                element.obj.GetComponent<HexagonCycle>().y = y;
            }
        }
    }
}
