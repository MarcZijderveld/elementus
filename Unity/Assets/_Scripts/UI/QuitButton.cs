using UnityEngine;
using System.Collections;

public class QuitButton : GUIMemberComponent 
{
	public string 		element;
	public Texture2D 	normalButton,
						highlightButton;
	
	public int			depth 		= 0;
	
	private QuitOverlay quitOverlay;
	
	private void OnGUI()
	{
		//This defines on which layer the GUI will be drawn.
		GUI.depth = depth;
		
		Rect rect = GUIMaster.GetElementRect(element);
		GUI.DrawTexture(rect, normalButton);
		
		if(rect.Contains(Event.current.mousePosition) && interactable)
		{
			GUI.DrawTexture(rect, highlightButton);
			if(Input.GetMouseButtonUp(0))
			{
				if(quitOverlay == null)
					quitOverlay = Hierarchy.GetComponentWithTag<QuitOverlay>("QuitOverlay");
				quitOverlay.ToggleInteractable();
				Application.Quit();
			}
		}
	}
}
