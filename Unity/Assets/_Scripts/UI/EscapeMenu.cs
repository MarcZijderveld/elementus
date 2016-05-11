using UnityEngine;
using System.Collections;

public class EscapeMenu : MonoBehaviour 
{

/*	private	GUIMaster 	_GUIMaster	= null; 
	private	GUIMaster 	GUIMaster
	{
		get
		{
			if (_GUIMaster == null)
			{
				_GUIMaster = Hierarchy.GetComponentWithTag<GUIMaster>("GUIMaster");
			}
			return _GUIMaster;
		}		
	} */
	
	public string 		bgElement,
						textElement,
						resumeElement,
						quitElement;
	
	public int			depth 		= 0;
	
	private bool		active		= false;
	
	private void OnGUI()
	{
		if(Input.GetKeyUp(KeyCode.Escape))
		{
			active = true;
		}
		if(active)
		{
			GUI.Box(GUIMaster.GetElementRect(bgElement), "");
			
			GUI.Label(GUIMaster.GetElementRect(textElement), "Pause Menu");
			
			if(GUI.Button(GUIMaster.GetElementRect(resumeElement), "Resume"))
			{
				active = false;
			}
			
			if(GUI.Button(GUIMaster.GetElementRect(quitElement), "Quit to Menu"))
			{
				active = false;
				Application.LoadLevel("_MainMenu");
			}
		}
	}
}
