using UnityEngine;
using System.Collections;

public class MoveHUD : PhotonGUIMemberComponent 
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

	private GUIStyle player1RefStyle,
					 player2RefStyle;

	private void OnGUI()
	{
		player1RefStyle = GUIMaster.ResolutionGUIStyle(player1Style);
		player2RefStyle = GUIMaster.ResolutionGUIStyle(player2Style);

		GUI.Label(GUIMaster.GetElementRect(player1Elem), "Mana: " + playerData.MovesLeft().ToString(), player1RefStyle);
		GUI.Label(GUIMaster.GetElementRect(player2Elem), "Mana: " + networkPlayerData.MovesLeft().ToString(), player2RefStyle	);
	}
}
