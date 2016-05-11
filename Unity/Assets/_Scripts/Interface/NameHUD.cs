using UnityEngine;
using System.Collections;

public class NameHUD : PhotonGUIMemberComponent 
{
	public string 	player1Elem, 
					player2Elem;

	public GUIStyle player1Style,
					player2Style;

	private GUIStyle player1RefStyle,
					player2RefStyle;

	public string	testName1,
					testName2;

	private void OnGUI()
	{
		player1RefStyle = GUIMaster.ResolutionGUIStyle(player1Style);
		player2RefStyle = GUIMaster.ResolutionGUIStyle(player2Style);

		if(!PhotonNetwork.offlineMode)
		{
			if(PhotonNetwork.player != null)
				GUI.Label(GUIMaster.GetElementRect(player1Elem), PhotonNetwork.player.name.ToString(), player1RefStyle);
			if(PhotonNetwork.otherPlayers.Length > 0)
				GUI.Label(GUIMaster.GetElementRect(player2Elem), PhotonNetwork.otherPlayers[0].name.ToString(), player2RefStyle);
		}
		if(!string.IsNullOrEmpty(testName1) && !string.IsNullOrEmpty(testName2))
		{
			GUI.Label(GUIMaster.GetElementRect(player1Elem), testName1, player1RefStyle);
			GUI.Label(GUIMaster.GetElementRect(player2Elem), testName2, player2RefStyle);;
		}
	}
}
