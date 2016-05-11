using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataSync : MonoBehaviour 
{
	private Dictionary<PhotonPlayer, bool> dataConfirmations = new Dictionary<PhotonPlayer, bool>();

	public bool resolving { get; private set; }

	private PhotonView photonView;

	public bool initConnected { get; private set; }

	private void Start()
	{
		initConnected = false;
		photonView = GetComponent<PhotonView>();
		resolving = false;
		SetDataConfirmations();
	}
	
	public void SetDataConfirmations()
	{
		dataConfirmations.Clear();
		if (PhotonNetwork.room != null)
		{
			for (int i = 0; i < PhotonNetwork.room.playerCount; i++)
			{
				dataConfirmations.Add(PhotonNetwork.playerList[i], false);
			}
		}
	}

	/*private void Update()
	{
		if(!initConnected)
		{
			SendDataConfirmation();
			if(CheckReady())
			{
				initConnected = true;
				SetDataConfirmations();
			}
		}
	}*/

	public void SendDataConfirmation()
	{
		photonView.RPC ("DataHandlerConfirmation", PhotonTargets.All, PhotonNetwork.player, true);
	}

	[RPC]
	private void DataHandlerConfirmation(PhotonPlayer player, bool confirmation)
	{
		if(dataConfirmations.ContainsKey(player))
			dataConfirmations[player] = confirmation;
	}

	public void SetResolve(bool @value)
	{
		resolving = value;
	}

	public bool CheckReady()
	{
		foreach(KeyValuePair<PhotonPlayer, bool> kvp in dataConfirmations)
		{
			if(kvp.Value == false)
			{
				return false;	
			}
		}	
		return true;;
	}
}
