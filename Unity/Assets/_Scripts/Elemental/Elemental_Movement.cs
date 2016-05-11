using UnityEngine;
using System.Collections;

public class Elemental_Movement : MonoBehaviour 
{
    public int 	    currentX= 5, 
					currentY = 5;

    public bool 	active,
					go = false,
					first = false;

	public  Grid 	grid { get; private set; } 
   
    private Vector3 prevPosition;

    private float 	startTime = 0,
   				  	journeyLength = 0,
    			  	distCovered = 0,
				  	fracJourney = 0;

    public float 	smooth = 5.0F;

	public MovementController momvementController { get; private set; } 

	public void Start () 
    {
        momvementController = GameObject.Find("MovementController").GetComponent<MovementController>();
		//animController = GetComponent<AnimationController>();
        grid = GameObject.Find("GridCreator").GetComponent<Grid>();

        startTime = Time.time;
        transform.position = grid.GetPosition(currentX, currentY) + new Vector3(0, 1f, 0);
        prevPosition = transform.position;
	}

	public void SetGridPos(Vector2 pos)
	{
		currentX = (int)pos.x;
		currentY = (int)pos.y;
	}

	public Vector2 GetCurrentPos()
	{
		return new Vector2(currentX, currentY);
	}
	
    public void MoveToTile(GameObject tile)
    {
        prevPosition = transform.position;
        startTime = Time.time;
        
        Vector2 position = grid.GetArrayLocation(tile) ;
        currentX = (int)position.x;
        currentY = (int)position.y;

        journeyLength = Vector3.Distance(prevPosition, grid.GetPosition(currentX, currentY));
    }


	void Update () 
    {
        if (active)
        {
            grid.Highlight(currentX, currentY, 2);
        }
	}
}
