using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DictionaryExtension;

/// <summary>
/// *--------------------------*
/// | Changelog / Terms of Use |
/// *--------------------------*
/// 
/// This is the Root / Master script for the GUIMaster system, each component subscribes to the master and the master will take care of all the scaling of the elements.
/// 
/// Changelog: 
/// - 1.2 > 26/11/2013 > Modified scaling for more precise scaling, added GUIStyle scaling for GUIContent like text. 
/// - 1.3 > 17/12/2013 > Changed the way the updates work to drastically improve performance (Overhead used to be 20% reduced to 0.1% as of now)
/// - 1.4 > 17/06/2014 > Class changed to a static to remove neccesity to get the GUIMaster class with a getter. 
/// 
/// Author: Marc Zijderveld
/// Original Date: 02/11/2013
/// Version: 1.4
/// 
/// [Components]
/// - GUIMember: The Gameobject element containing all the rectangles.
/// - GUIMemberComponent: A small include which you can inherit from for GUIElements.
/// - DictionaryExtensions: Added dictionary functionality.
/// - ResolutionManager: Contains information about the default resolution and triggers resolution changes.
/// </summary>

public static class GUIMaster 
{
	//The scale values in the width and height.
	private static float scaleX,
	scaleY;
	
	/// <summary>
	/// All the GUIMembers are stored here.
	/// </summary>
	public static Dictionary<string, GUIMember> GUIMembers = new Dictionary<string, GUIMember>();
	
	/// Change the GUIMembers identifier in the dictionary.
	/// </summary>
	public static bool ChangeIdentifier(string oldIdentifier, string newIdentifier)
	{
		return GUIMembers.ChangeKey(oldIdentifier, newIdentifier);
	}
	
	/// <summary>
	/// Update the GUIMembers content.
	/// </summary>
	public static void UpdateMember(string identifier, GUIMember member)
	{
		member.SetScaling(ResolutionRect(member.rect, member.scaleMode));
		GUIMembers[identifier] = member;
	}
	
	/// <summary>
	/// Add a GUIMember to the dictionary.
	/// </summary>
	public static void AddMember(string identifier, GUIMember member)
	{
		if(!GUIMembers.ContainsKey(identifier))
			GUIMembers.Add(identifier, member);
	}
	
	/// <summary>
	/// Remove a GUIMember from the dictionary.
	/// </summary>
	public static void RemoveMember(string identifier)
	{
		GUIMembers.Remove(identifier);
	}
	
	/// <summary>
	/// Scale the source retangle in the GUIMember with a certain scale based on the default resolution set in the ResolutionManager.cs.
	/// </summary>
	public static Rect ResolutionRect(Rect rectangle, ResolutionManager.scaleMode mode)
	{
		Rect returnRect = new Rect(0,0,0,0);	
		
		scaleX = Screen.width / ResolutionManager.GetDefaultResolution().x;
		scaleY = Screen.height / ResolutionManager.GetDefaultResolution().y;
		
		//Different scaling modes for each GUIMember. (Default is ScaleWithResolution)
		switch(mode)
		{
		case ResolutionManager.scaleMode.keepPixelSize:
			returnRect = new Rect(rectangle.x * scaleX, rectangle.y * scaleY, rectangle.width, rectangle.height);
			break;
		case ResolutionManager.scaleMode.scaleWidth:
			returnRect = new Rect(rectangle.x * scaleX, rectangle.y * scaleY, rectangle.width * scaleX, rectangle.height);
			break;
		case ResolutionManager.scaleMode.scaleHeight:
			returnRect = new Rect(rectangle.x * scaleX, rectangle.y * scaleY, rectangle.width, rectangle.height * scaleY);
			break;
		case ResolutionManager.scaleMode.scaleWithResolution:
			returnRect = new Rect(rectangle.x * scaleX, rectangle.y * scaleY, rectangle.width * scaleX, rectangle.height * scaleY);
			break;
		}
		
		//The return value of the scaled rectangle of the source rect.
		returnRect = new Rect(Mathf.Round(returnRect.x), Mathf.Round(returnRect.y), Mathf.Round(returnRect.width), Mathf.Round(returnRect.height));
		
		return returnRect;
	}
	
	/// <summary>
	/// Toggle rectangle update when the game window changes sizes.
	/// </summary>
	public static void OnResolutionChanged()
	{
		foreach(KeyValuePair<string, GUIMember> r in GUIMembers)
		{
			r.Value.SetScaling(ResolutionRect(r.Value.rect, r.Value.scaleMode));
		}
	}
	
	/// <summary>
	///Returns the rectangle of a certain element.
	///</summary>
	public static Rect GetElementRect(string element)
	{
		if(GUIMembers.ContainsKey(element))
		{
			//Debug.Log(GUIMembers[element].GetScaledRect());
			return GUIMembers[element].GetScaledRect();
		}
		else
			Debug.LogWarning("GUIMember with name: " + element + " is not present in the dictionary!");
		
		return new Rect(0,0,0,0);
	}
	
	/// <summary>
	///Returns the rectangle of a certain element with a extra offset for position (used for example for animating).
	///</summary>
	public static Rect GetElementRect(string element, float xOffset, float yOffset)
	{
		if(GUIMembers.ContainsKey(element))
		{
			Rect rect = GUIMembers[element].GetScaledRect();
			rect.x += xOffset * scaleX;
			rect.y += yOffset * scaleY;
			return rect;
		}
		else
			Debug.LogWarning("GUIMember with name: " + element + " is not present in the dictionary!");
		
		return new Rect(0,0,0,0);
	}
	
	/// <summary>
	/// Scales the input guiStyle and all its properties based on the current resolution.
	/// </summary>
	public static GUIStyle ResolutionGUIStyle(GUIStyle input)
	{
		GUIStyle style = new GUIStyle(input);
		style.fontSize = Mathf.RoundToInt(input.fontSize * scaleX);
		style.contentOffset = new Vector2(input.contentOffset.x * scaleX, input.contentOffset.y * scaleY);
		return style;
	}
	
	/// <summary>
	/// Add a GUIElement to the dictionary.
	/// </summary>
	public static void AddElement(string element, Rect rect) 
	{
		GUIMember member = new GUIMember();
		member.name = element;
		member.rect = rect;
		
		GUIMembers.Add(element, member);
	}
}
