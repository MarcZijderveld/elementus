using UnityEngine;
using System.Collections;

public class ConnectionManager : Photon.MonoBehaviour 
{
	public string currentGameVersion = "v0.5b";
	
	void Start () 
	{
		PhotonNetwork.ConnectUsingSettings(currentGameVersion);
	}
}
