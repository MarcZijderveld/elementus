using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : GUIMemberComponent
{
	public Texture2D loadingScreen;

	public string element;

	private bool draw = false;

	public GUIImage image;

	[RPC]
	public void LoadLevel(string levelName)
	{
		draw = true;
		image.SetInteratable(false);
		Debug.Log("loaded level: " + levelName);
		Time.timeScale = 1;
		PhotonNetwork.LoadLevel(levelName);
	}

	private void OnGUI()
	{
		if(draw)
			GUI.DrawTexture(GUIMaster.GetElementRect(element), loadingScreen);
	}
	
	/*private void OnLevelWasLoaded()
	{
		if(PhotonNetwork.isMasterClient)
		{
			List<PhotonView> views = Hierarchy.GetComponentsWithTag<PhotonView>("PlayerSpawner");
			
			foreach(PhotonView view in views)
			{
				view.RPC("SpawnPlayer", PhotonTargets.All, PhotonNetwork.player.name);
			}
		}	
	}*/
}
