using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLobby : PhotonGUIMemberComponent 
{
	[System.Serializable]
	public class LevelInfo
	{
		public string level;
		public Texture2D image;
		public string size;
		public string units;
	}

	public string 	 headerElement,
					 player1Element,
					 player2Element,
					 levelSelectElement,
					 lobbyElement,
		             playersElement,
					 bgElement,
					 icon1Element,
					 icon2Element,
					 levelImage,
					 levelBG,
					 levelSize,
					 levelUnits;
	
	public GUIStyle	 headerStyle,
					 meStyle,
					 otherStyle,
					 playersStyle,
					 levelStyle,
					 infoStyle;
	
	private GUIStyle sourceHeaderStyle,
					 meSourceStyle,
					 otherSouceStyle,
					 playersSourceStyle,
					 levelSourceStyle,
					 infoSourceStyle;
	
	public Texture2D headerBG,
					 playerBG,
					 levelSelectNormal,
					 levelSelectHover,	
					 playNormal,
					 playHover,
					 disconnectNormal,
					 disconnectHover,
					 BG,
	                 icon1,
	                 icon2,
					 levelSelectText,
					 infoBG;
	
	private string	 levelString;

	public GUIImage	 playerImage;
	
	public List<LevelInfo>	 levelStrings;
	
	public bool 	 levelSelect =  false;
	
	public AudioClip sound;

	public float 	xOffset = 0;
	
	private void OnGUI()
	{
		sourceHeaderStyle = GUIMaster.ResolutionGUIStyle(headerStyle);
		meSourceStyle = GUIMaster.ResolutionGUIStyle(meStyle);
		otherSouceStyle = GUIMaster.ResolutionGUIStyle(otherStyle);
		playersSourceStyle = GUIMaster.ResolutionGUIStyle(playersStyle);
		levelSourceStyle = GUIMaster.ResolutionGUIStyle(levelStyle);
		infoSourceStyle = GUIMaster.ResolutionGUIStyle(infoStyle);
		
		GUI.DrawTexture(GUIMaster.GetElementRect(headerElement), headerBG);
		
		GUI.Label(GUIMaster.GetElementRect(playersElement), "Players", playersSourceStyle);
		
		if(PhotonNetwork.room != null)
		{
			string[] names = PhotonNetwork.room.name.Split('&');
			
			GUI.Label(GUIMaster.GetElementRect(headerElement), names[1], sourceHeaderStyle);
			
			if(PhotonNetwork.playerList.Length > 0)
			{
				foreach(PhotonPlayer p in PhotonNetwork.playerList)
				{
					if(p.isMasterClient)
						DrawPlayer(p, player1Element);
					else
						DrawPlayer(p, player2Element);
				}	
			}
		}
		
		if(PhotonNetwork.isMasterClient)
		{
			if(PhotonNetwork.room.maxPlayers == PhotonNetwork.room.playerCount)
				PhotonNetwork.room.open = false;
			else
				PhotonNetwork.room.open = true;
			
			if(PhotonNetwork.room.maxPlayers == PhotonNetwork.room.playerCount)
			{
				GUI.DrawTexture(GUIMaster.GetElementRect(levelSelectElement), levelSelectNormal);
				
				if(!levelSelect)
				{
					if(GUIMaster.GetElementRect(levelSelectElement).Contains(Event.current.mousePosition))
					{
						GUI.DrawTexture(GUIMaster.GetElementRect(levelSelectElement), levelSelectHover);
						
						if(Event.current.type == EventType.mouseDown)
						{
							Hierarchy.GetComponentWithTag<SoundSettings>("SoundManager").Play(sound);
							levelSelect = true;
							return;
						}
					}
				}	
				else if(levelSelect)
				{
					playerImage.SetInteratable(false);

					GUI.DrawTexture(GUIMaster.GetElementRect(bgElement), BG);
					
					GUI.DrawTexture(GUIMaster.GetElementRect(headerElement), levelSelectText);

					GUI.DrawTexture(GUIMaster.GetElementRect(levelBG), infoBG);
					
					for(int i = 0; i < levelStrings.Count; i++) 
					{
						Rect rect = GUIMaster.GetElementRect(icon1Element);
						Rect tempRect = new Rect(rect.x + (xOffset * i) + (rect.width * i), rect.y, rect.width, rect.height);
						Rect textRect = new Rect(rect.x + (xOffset * i) + (rect.width * i), rect.y + 20, rect.width, rect.height);

						GUI.DrawTexture(tempRect, icon1);
						GUI.Label(tempRect, (i+1).ToString(), levelSourceStyle);

						if(tempRect.Contains(Event.current.mousePosition))
						{
							GUI.DrawTexture(tempRect, icon2);
							GUI.Label(tempRect, (i+1).ToString(), levelSourceStyle);
							GUI.DrawTexture(GUIMaster.GetElementRect(levelImage), levelStrings[i].image);
							GUI.Label(GUIMaster.GetElementRect(levelUnits), "Units: " + levelStrings[i].units, infoSourceStyle);
							GUI.Label(GUIMaster.GetElementRect(levelSize), "Size:  " + levelStrings[i].size, infoSourceStyle);
							
							if(Event.current.type == EventType.mouseDown)
							{
								playerImage.SetInteratable(false);
								Hierarchy.GetComponentWithTag<SoundSettings>("SoundManager").Play(sound);
								PhotonView view = Hierarchy.GetComponentWithTag<PhotonView>("GameUtilities");
								//GUI.DrawTexture(GUIMaster.GetElementRect(bgElement), loadingScreen);
								view.RPC("LoadLevel", PhotonTargets.All, levelStrings[i].level);
							}
						}
					}	
					
					/*GUI.DrawTexture(GUIMaster.GetElementRect(levelSelectElement), playNormal);
				
					if(GUIMaster.GetElementRect(levelSelectElement).Contains(Event.current.mousePosition))
					{
						GUI.DrawTexture(GUIMaster.GetElementRect(levelSelectElement), playHover);
						
						if(Event.current.type == EventType.mouseDown)
						{
							Hierarchy.GetComponentWithTag<SoundSettings>("SoundManager").Play(sound);
							PhotonView view = Hierarchy.GetComponentWithTag<PhotonView>("GameUtilities");
							view.RPC("LoadLevel", PhotonTargets.All, levelString);
							PhotonNetwork.isMessageQueueRunning = false;
							//GUI.DrawTexture(GUIMaster.GetElementRect(bgElement), loadingScreen);
						}
					}*/
				}
			}
		}
		
		GUI.DrawTexture(GUIMaster.GetElementRect(lobbyElement), disconnectNormal);
		
		if(GUIMaster.GetElementRect(lobbyElement).Contains(Event.current.mousePosition))
		{
			GUI.DrawTexture(GUIMaster.GetElementRect(lobbyElement), disconnectHover);
			
			if(Event.current.type == EventType.mouseUp)
			{
				Hierarchy.GetComponentWithTag<SoundSettings>("SoundManager").Play(sound);
				PhotonNetwork.LeaveRoom();
				Application.LoadLevel("_NetworkLobby");
			}
		}
	}
	
	private void DrawPlayer(PhotonPlayer player, string element)
	{
		GUI.DrawTexture(GUIMaster.GetElementRect(element), playerBG);

		if(player.isMasterClient)
		{
			GUI.Label(GUIMaster.GetElementRect(element), player.name, meSourceStyle);
		}
		else
		{
			GUI.Label(GUIMaster.GetElementRect(element), player.name, otherSouceStyle);
		}
	}
}
