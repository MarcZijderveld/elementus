using UnityEngine;
using System.Collections;

public class SceneLoadButton : GUIMemberComponent 
{	
	public string 		element;
	public Texture2D 	normalButton,
						highlightButton;
	
	public string 		sceneToLoad = "default";
	
	public int			depth 		= 0;
	
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
				GUI.DrawTexture(rect, normalButton);
				Application.LoadLevel(sceneToLoad);
			}
		}
	}
}
