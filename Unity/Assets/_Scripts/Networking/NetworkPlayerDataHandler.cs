using UnityEngine;
using System.Collections;

public class NetworkPlayerDataHandler : Photon.MonoBehaviour 
{
	private	NetworkPlayerData 	_networkPlayerData	= null; 
	public	NetworkPlayerData 	networkPlayerData
	{
		get
		{
			if (_networkPlayerData == null)
			{
				_networkPlayerData = this.GetComponent<NetworkPlayerData>();
			}
			return _networkPlayerData;
		}		
	} 
	
	[RPC]
	public void SetID(int value)
	{
		networkPlayerData.SetID(value);
	}
	
	public int GetID()
	{
		return networkPlayerData.GetID();
	}

	[RPC]
	public void AddPoints(int value)
	{
		networkPlayerData.AddPoints(value);
	}
	
	public int GetPoints()
	{
		return networkPlayerData.GetPoints();
	}

	[RPC]
	public void SetMoves(int value)
	{
		networkPlayerData.SetMoves(value);
	}
	
	public int MovesLeft()
	{
		return networkPlayerData.MovesLeft();
	}
}
