using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour 
{
	private	MovementController 	_movementController	= null; 
	public	MovementController 	movementController
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

	private	PlayerDataHandler 	_playerData	= null; 
	public	PlayerDataHandler 	playerData
	{
		get
		{
			if (_playerData == null)
			{
				_playerData = Hierarchy.GetComponentWithTag<PlayerDataHandler>("PlayerDataHandler");
			}
			return _playerData;
		}		
	}

	private	DataSync 	_data	= null; 
	public	DataSync 	data
	{
		get
		{
			if (_data == null)
			{
				_data = Hierarchy.GetComponentWithTag<DataSync>("DataSync");
			}
			return _data;
		}		
	} 

	public GameObject unit = null,
					  managerObject;
   	private Elementals_Manager manager;
    private Grid grid;
	private int id;

    private void Start()
    {
        managerObject = GameObject.Find("Elemenals_Manager");
        manager = GameObject.Find("Elemenals_Manager").GetComponent<Elementals_Manager>();
        grid = GameObject.Find("GridCreator").GetComponent<Grid>();
    }
	
	// Update is called once per frame
	private void Update () 
	{
        //set the values ready for highlighting
        if (unit != null)
        {
            unit.GetComponent<AnimationStates>().movement.active = true;
			id = unit.GetComponent<Elemental_Properties>().id;

        }
        //make a raycat to do the selection with
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        //elemental selection
        if (Physics.Raycast(ray, out hit, 100) && Input.GetButtonDown("Select") && hit.collider.gameObject.tag == "Hexagon" &&  data.resolving == false && unit == null)
        {
            GameObject hitobject = hit.collider.gameObject;
            Vector2 postion = grid.GetArrayLocation(hitobject);
            
            foreach (Transform child in managerObject.transform)
            {
                if (!child.gameObject.activeSelf)
                    continue;
				Elemental_Movement move = child.GetComponentInChildren<AnimationStates>().movement;

                if (move.currentX == (int)postion.x && move.currentY == (int)postion.y)
                {
                    if (child.GetComponent<Elemental_Properties>().isMine)
                    {
                        grid.ResetHighlight();
                        if (unit != null)
                        {
                            unit.GetComponentInChildren<AnimationStates>().movement.active = false;
                        }
                        unit = child.gameObject;
                    }
                }
            }
           
		}
        //hexagon selection
		if (Physics.Raycast(ray, out hit, 100) && Input.GetButtonDown("Select") && hit.collider.gameObject.tag == "Hexagon" && unit != null)
        {
	        if (hit.collider.gameObject.GetComponent<HighLight>().highlight == true)
	        {
	            //grid.ResetHighlight();
                
				Vector2 BeginPosition = unit.GetComponentInChildren<AnimationStates>().movement.GetCurrentPos();
                Vector2 position = grid.GetArrayLocation(hit.collider.gameObject) ;
                Element_Enum.Types beginType = unit.GetComponent<Elemental_Change>().element;
                movementController.AddMove(id, BeginPosition, position, beginType, beginType);
				Deselect();
	        }
        }
		if (Physics.Raycast(ray, out hit, 100) && Input.GetButtonDown("Select") && (hit.collider.gameObject.tag == "Hexagon" || hit.collider.gameObject.tag == "Empty"))
        {
            hit.collider.gameObject.GetComponent<HexagonCycle>().Change2();
        }

		if (Physics.Raycast(ray, out hit, 100) && Input.GetButtonDown("Select") && (hit.collider.gameObject.tag == "Hexagon" || hit.collider.gameObject.tag == "Empty"))
        {
            hit.collider.gameObject.GetComponent<HexagonCycle>().Change();
        }

        //selection for a hexagon
        if (Physics.Raycast(ray, out hit, 100))
        {
            if (hit.collider.gameObject.tag == "Hexagon" )
            {
				grid.ResetMouseOverHighlight();
				hit.collider.gameObject.GetComponent<HighLight>().mousover = true;	              
            }
        }

        if (Physics.Raycast(ray, out hit, 100))
            Debug.DrawLine(ray.origin, hit.point);

		if (Input.GetButtonDown("Element1") && unit != null)
		{
			SetUnitElement(Element_Enum.Types.water);
		}
		if (Input.GetButtonDown("Element2") && unit != null)
		{
			SetUnitElement(Element_Enum.Types.fire);
		}
		if (Input.GetButtonDown("Element3") && unit != null)
		{
			SetUnitElement(Element_Enum.Types.grass);
		}

		if (Input.GetButtonDown ("Deselect")) 
		{
			Deselect();
		}

		if (Input.GetButtonDown("Cancel"))
		{
			if(unit != null)
			{
				movementController.RemoveMove(id);
				Deselect();
			}
		}
	}

	//Change the currently selected unit element.
	public void SetUnitElement(Element_Enum.Types type)
	{
		if(unit != null)
		{
			Vector2 beginPosition = unit.GetComponentInChildren<AnimationStates>().movement.GetCurrentPos();
			unit.GetComponent<Elemental_Change>().StartCoroutine("ChangeElement", type);
			movementController.AddColor(id, beginPosition, beginPosition, type);
			playerData.SetMoves(-1);
		}
	}

	private void Deselect()
	{
		grid.ResetHighlight();
		
		if(unit != null)
		{
			unit.GetComponentInChildren<AnimationStates>().movement.active = false;
			unit = null;
		}
	}
}
