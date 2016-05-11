using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class GameSequence : Photon.MonoBehaviour 
{
	public bool local = true;

	public int turn {get; private set;}

	private bool running = false;

	private void Start()
	{
		turn = 0;
#if UNITY_EDITOR
		if(!local)
		{
#endif
			PhotonView view = Hierarchy.GetComponentWithTag<PhotonView>("PlayerDataHandler");
			if(PhotonNetwork.isMasterClient)
			{
				for(int i = 0; i < PhotonNetwork.room.playerCount; i++)
				{
					view.RPC("SetID", PhotonPlayer.Find(PhotonNetwork.playerList[i].ID), i + 1);
				}
			}

			if(PhotonNetwork.isMasterClient)
				Hierarchy.GetComponentWithTag<PhotonView>("UnitManager").RPC("StartSpawn", PhotonTargets.All, "");
#if UNITY_EDITOR
		}
		else
		{
			PhotonNetwork.offlineMode = local;
			foreach(KeyValuePair<int, Transform> t in Hierarchy.GetComponentWithTag<MovementController>("MovementController").units)
			{
				t.Value.GetComponent<Elemental_Properties>().isMine = true;
			}
		}
#endif

		running = true;
		PhotonNetwork.isMessageQueueRunning = true;
	}


	public void NextTurn()
	{
		Hierarchy.GetComponentWithTag<PlayerDataHandler>("PlayerDataHandler").SetMoves(1);
		Hierarchy.GetComponentWithTag<PhotonView>("NetworkPlayerDataHandler").RPC("SetMoves", PhotonTargets.Others, 1);
		turn++;
	}

	public void EndGame()
	{
		running = false;
	}
}
