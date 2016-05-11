using UnityEngine;
using System.Collections;

public class GUIImage : GUIMemberComponent					
{
	public Texture2D 	texture;
	public string		guiElement 	= "";
	public int			depth 		= 0;
	
	public void OnGUI()
	{	
		GUI.depth = depth;
		
		if(interactable)
			GUI.DrawTexture(GUIMaster.GetElementRect(guiElement), texture);
	}
}
