using UnityEngine;
using System.Collections;

public class PauseMenu : GUIMemberComponent 
{
	public string 	bgElement,
				    textElement,
					resumeElement,
					toLobbyElement,
					toMenuElement,
					bg2Element,
					menuButton;
	
	public Texture2D zwartePlark,
					 resumeNormal,
					 resumeHover,
					 lobbyNormal,
					 lobbyHover,
					 menuNormal,
					 menuHover,
					 bg,
					 blankBG,
					 buttonNormal,
					 buttonHover;
	
	public GUIStyle	 textStyle;
	
	private GUIStyle sourceTextStyle;
	
	private bool active = false;
	
	public AudioClip sound;

	public int depth;

	public bool pauseMenu {get; private set;}

	private string menuText;

	private void Start()
	{
		pauseMenu = true;
	}

	private void Update()
	{
		if(Input.GetButtonDown("Menu") && pauseMenu)
			active = true;
	}
	
	private void OnGUI()
	{
		GUI.depth = depth;
			
		GUI.DrawTexture(GUIMaster.GetElementRect(menuButton), buttonNormal);
		
		if(GUIMaster.GetElementRect(menuButton).Contains(Event.current.mousePosition))
		{
			GUI.DrawTexture(GUIMaster.GetElementRect(menuButton), buttonHover);
			if(Event.current.type == EventType.mouseUp)
			{
				active = true;
			}
		}
		
		if(active)
		{
			sourceTextStyle = GUIMaster.ResolutionGUIStyle(textStyle);
			
			GUI.DrawTexture(GUIMaster.GetElementRect(bgElement), zwartePlark);
			//GUI.Label(GUIMaster.GetElementRect(textElement), "Game Paused", sourceTextStyle);

			if(pauseMenu)
			{
				GUI.DrawTexture(GUIMaster.GetElementRect(bg2Element), bg);
				GUI.DrawTexture(GUIMaster.GetElementRect(resumeElement), resumeNormal);
					
				if(GUIMaster.GetElementRect(resumeElement).Contains(Event.current.mousePosition))
				{
					GUI.DrawTexture(GUIMaster.GetElementRect(resumeElement), resumeHover);
					if(Event.current.type == EventType.mouseUp)
					{
						Hierarchy.GetComponentWithTag<SoundSettings>("SoundManager").Play(sound);
						active = false;
					}
				}
			}
			else
			{
				GUI.DrawTexture(GUIMaster.GetElementRect(bg2Element), blankBG);
				GUI.Label(GUIMaster.GetElementRect(textElement), menuText, textStyle);
			}

			GUI.DrawTexture(GUIMaster.GetElementRect(toLobbyElement), lobbyNormal);
				
			if(GUIMaster.GetElementRect(toLobbyElement).Contains(Event.current.mousePosition))
			{
				GUI.DrawTexture(GUIMaster.GetElementRect(toLobbyElement), lobbyHover);
				if(Event.current.type == EventType.mouseUp)
				{
					Hierarchy.GetComponentWithTag<SoundSettings>("SoundManager").Play(sound);
					//PhotonView view = Hierarchy.GetComponentWithTag<PhotonView>("GameUtilities");
					PhotonNetwork.LeaveRoom();
					Application.Quit();
				}
			}
			
			GUI.DrawTexture(GUIMaster.GetElementRect(toMenuElement), menuNormal);
				
			if(GUIMaster.GetElementRect(toMenuElement).Contains(Event.current.mousePosition))
			{
				GUI.DrawTexture(GUIMaster.GetElementRect(toMenuElement), menuHover);
				if(Event.current.type == EventType.mouseUp)
				{
					Hierarchy.GetComponentWithTag<SoundSettings>("SoundManager").Play(sound);
					PhotonNetwork.LeaveRoom();
					Application.LoadLevel("_MainMenu");
				}
			}
		}
	}
	
	public void SetPause(bool @value, string text)
	{
		pauseMenu = value;
		menuText = text;
		active = true;
	}
}
