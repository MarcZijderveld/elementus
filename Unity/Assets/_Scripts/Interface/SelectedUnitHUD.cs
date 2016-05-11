using UnityEngine;
using System.Collections;

public class SelectedUnitHUD : GUIMemberComponent 
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

	private	Controller 	_controller	= null; 
	public	Controller 	controller
	{
		get
		{
			if (_controller == null)
			{
				_controller = Hierarchy.GetComponentWithTag<Controller>("Controller");
			}
			return _controller;
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
	
	public string 		bgelement,
						readyButtonElem,
						arrowButtonElem,
						fireButtonElem,
						waterButtonElem,
						grassButtonElem,
						interactionElem;

	public Texture2D 	normalButton,
						highlightButton,
						fireButton,
						fireButtonHL,
						waterButton,
						waterButtonHL,
						grassButton,
						grassButtonHL,
						arrowButton,
						arrowButtonHL,
						backGround,
						grayButton;
	
	public int			depth 		= 0;
	
	public AudioClip	sound;

	private bool 		up = false,
						running = false,
						down = false;

	private void Update()
	{
		if(!data.resolving)
		{
			if(Input.GetButtonDown("End Turn"))
			{
				movementController.Ready();
			}
		}
	}

	private void OnGUI()
	{
		//This defines on which layer the GUI will be drawn.
		GUI.depth = depth;
		
		Rect rect = new Rect(); ;

		if(!data.resolving)
		{
			GUI.DrawTexture (GUIMaster.GetElementRect(bgelement), backGround);

			rect = GUIMaster.GetElementRect(readyButtonElem);
			
			GUI.DrawTexture(rect, normalButton);
			
			if(rect.Contains(Event.current.mousePosition))
			{
				GUI.DrawTexture(rect, highlightButton);
				if(Event.current.type == EventType.mouseUp)
				{
					GUI.DrawTexture(rect, normalButton);
					movementController.Ready();
				}
			}

			rect = GUIMaster.GetElementRect(fireButtonElem);

			GUI.DrawTexture(rect, fireButton);
			
			if(rect.Contains(Event.current.mousePosition))
			{
				GUI.DrawTexture(rect, fireButtonHL);
				if(Event.current.type == EventType.mouseUp)
				{
					GUI.DrawTexture(rect, fireButton);
					controller.SetUnitElement(Element_Enum.Types.fire);
				}
			}

			rect = GUIMaster.GetElementRect(waterButtonElem);

			GUI.DrawTexture(rect, waterButton);
			
			if(rect.Contains(Event.current.mousePosition))
			{
				GUI.DrawTexture(rect, waterButtonHL);
				if(Event.current.type == EventType.mouseUp)
				{
					GUI.DrawTexture(rect, waterButton);
					controller.SetUnitElement(Element_Enum.Types.water);
				}
			}

			rect = GUIMaster.GetElementRect(grassButtonElem);

			GUI.DrawTexture(rect, grassButton);
			
			if(rect.Contains(Event.current.mousePosition))
			{
				GUI.DrawTexture(rect, grassButtonHL);
				if(Event.current.type == EventType.mouseUp)
				{
					GUI.DrawTexture(rect, grassButton);
					controller.SetUnitElement(Element_Enum.Types.grass);
				}
			}
		}
	}
}

