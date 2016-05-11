using UnityEngine;
using System.Collections;

public class DisconnectCheck : MonoBehaviour 
{
	public PauseMenu menu;

	public bool test = true;

	private void Update()
	{
		if(test)
		{
			menu.SetPause(false, "Connection \n lost..");
		}
	}

	public void OnPhotonPlayerDisconnected(PhotonPlayer player)
	{
		menu.SetPause(false, "Connection \n lost..");
		
		//Debug.Log("MOOOOO");
	}
}
