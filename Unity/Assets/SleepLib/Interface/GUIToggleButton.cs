using UnityEngine;
using System.Collections;

public class GUIToggleButton : GUIMemberComponent 
{
	public GUIMemberComponent toggleComponent;
	
	public string 		element;
	public Texture2D 	normalButton,
						highlightButton;
	
	public int			depth 		= 0;
	
	private bool 		changed = false;
	
	public AudioClip	sound;
	
	private void OnGUI()
	{		
		//This defines on which layer the GUI will be drawn.
		GUI.depth = depth;
		
		Rect rect = GUIMaster.GetElementRect(element);
		GUI.DrawTexture(rect, normalButton);
		
		if(rect.Contains(Event.current.mousePosition) && interactable)
		{
			GUI.DrawTexture(rect, highlightButton);
			if(Event.current.type == EventType.mouseUp)
			{
				Hierarchy.GetComponentWithTag<SoundSettings>("SoundManager").Play(sound);
//				Debug.Log("Moo");
				GUI.DrawTexture(rect, normalButton);
				toggleComponent.ToggleInteractable();
				
				toggleComponent.enabled = (!toggleComponent.enabled) ? true : false;
			}
		}
	}
}
