
/* Copyright (c) 2012 MoPho' Games
 * All Rights Reserved
 * 
 * Please see the included 'LICENSE.TXT' for usage rights
 * If this asset was downloaded from the Unity Asset Store,
 * you may instead refer to the Unity Asset Store Customer EULA
 * If the asset was NOT purchased or downloaded from the Unity
 * Asset Store and no such 'LICENSE.TXT' is present, you may
 * assume that the software has been pirated.
 * */

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

using System.Collections;

using MoPhoGames.USpeak.Interface;

/// <summary>
/// Default talk controller. Can either send while a key is held, or toggle sending on key press
/// </summary>
[AddComponentMenu( "USpeak/Default Talk Controller" )]
public class DefaultTalkController : GUIMemberComponent, IUSpeakTalkController
{
	/// <summary>
	/// The toggle mode. 0 for Push To Talk, 1 for Toggle Talk
	/// </summary>
	[HideInInspector]
	[SerializeField]
	public int ToggleMode = 0; // PushToTalk

	private bool val = false;

	#region IUSpeakTalkController Members
	
	public Texture2D muted,
				     active;
	
	public string    micButton;
	
	private bool 	 show = false;
	
	public void OnInspectorGUI()
	{
			
	}

	private void OnGUI()
	{
		if(Hierarchy.GetComponentWithTag<Chat>("Chat").IsActive() && show)
		{
			if(val)
				GUI.DrawTexture(GUIMaster.GetElementRect(micButton), active);
			else
				GUI.DrawTexture(GUIMaster.GetElementRect(micButton), muted);
			
			if(GUIMaster.GetElementRect(micButton).Contains(Event.current.mousePosition))
			{
				if(Event.current.type == EventType.mouseUp)
				{
					val = !val;
				}
			}
		}	
	}
	
	public void SetShow(bool boolValue)
	{
		show = boolValue;
	}
	
	public bool ShouldSend()
	{
		if( ToggleMode == 0 )
		{
			val = Input.GetButton( "Voice Toggle" );
		}
		else
		{
			if( Input.GetButtonDown( "Voice Toggle" ) )
				val = !val;
		}
		return val;
	}

	#endregion
}