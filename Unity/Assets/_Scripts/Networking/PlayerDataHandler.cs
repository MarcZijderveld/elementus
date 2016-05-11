using UnityEngine;
using System.Collections;

public class PlayerDataHandler : Photon.MonoBehaviour 
{
	private	PlayerData 	_playerData	= null; 
	public	PlayerData 	playerData
	{
		get
		{
			if (_playerData == null)
			{
				_playerData = this.GetComponent<PlayerData>();
			}
			return _playerData;
		}		
	} 

	[RPC]
	public void SetID(int value)
	{
		playerData.SetID(value);
	}

	public int GetID()
	{
		return playerData.GetID();
	}
	
	public void AddPoints(int value)
	{
		playerData.AddPoints(value);
	}

	public int GetPoints()
	{
		return playerData.GetPoints();
	}

	public void SetMoves(int value)
	{
		playerData.SetMoves(value);
	}

	
	public bool CanMove()
	{
		if(MovesLeft() > 0)
		{
			return true;
		}
		
		return false;
	}

	public int MovesLeft()
	{
		return playerData.MovesLeft();
	}
}
