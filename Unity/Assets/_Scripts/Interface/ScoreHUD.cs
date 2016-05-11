using UnityEngine;
using System.Collections;

public class ScoreHUD : PhotonGUIMemberComponent 
{
	private	NetworkPlayerDataHandler 	_networkPlayerData	= null; 
	public	NetworkPlayerDataHandler 	networkPlayerData
	{
		get
		{
			if (_networkPlayerData == null)
			{
				_networkPlayerData = Hierarchy.GetComponentWithTag<NetworkPlayerDataHandler>("NetworkPlayerDataHandler"	);
			}
			return _networkPlayerData;
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

	public string 	player1Elem, 
					player2Elem;

	public GUIStyle player1Style,
					player2Style;

	public Texture2D activeCrystal,
					 inactiveCrystal;

	public float offset = 2;

	public int 		maxScore = 10;

	private void OnGUI()
	{
		Rect elementRect1 = GUIMaster.GetElementRect(player1Elem);

		Rect elementRect2 = GUIMaster.GetElementRect(player2Elem);

		for(int i = 0; i < maxScore; i++)
		{
			GUI.DrawTexture(new Rect(elementRect1.x + (elementRect1.width + offset) * i, elementRect1.y, elementRect1.width, elementRect1.height), inactiveCrystal);
			GUI.DrawTexture(new Rect(elementRect2.x - (elementRect2.width + offset) * i, elementRect2.y, elementRect2.width, elementRect2.height), inactiveCrystal);
		}

		for(int i = 0; i < playerData.GetPoints(); i++)
		{
			GUI.DrawTexture(new Rect(elementRect1.x + (elementRect1.width + offset) * i, elementRect1.y, elementRect1.width, elementRect1.height), activeCrystal);
		}
		
		for(int i = 0; i < networkPlayerData.GetPoints(); i++)
		{
			GUI.DrawTexture(new Rect(elementRect2.x - (elementRect2.width + offset) * i, elementRect2.y, elementRect2.width, elementRect2.height), activeCrystal);
		}

		//GUI.Label(GUIMaster.GetElementRect(player1Elem), "Score: \n" + playerData.GetPoints().ToString(), player1Style);
		//GUI.Label(GUIMaster.GetElementRect(player2Elem), "Score: \n" + networkPlayerData.GetPoints().ToString(), player2Style);
	}
}
