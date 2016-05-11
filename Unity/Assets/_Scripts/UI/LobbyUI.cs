using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LobbyUI : PhotonGUIMemberComponent
{
	public bool debugging = false;	
	
	public enum ConnectionStatus
	{
		disconnected,
		inLobby,
		creatingRoom,
		inGame,
		host,
		client
	}
	
	public GUIStyle 			lobbyBrowserStyle;
	
	private bool 				privateRoom = false;
	
	private string 				roomName = "",
								playerName = "";
	
	private int    				maxPlayers = 2;
	
	private ConnectionStatus 	status 	= ConnectionStatus.disconnected;
	
	public string 				roomLevel = "";
	
	private void Awake()
	{
			
	}
	
    public void OnGUI()
	{		
		LobbyLayout();
		
		switch(status)
		{
			case ConnectionStatus.disconnected:
				ConnectionWizard();
				break;
			case ConnectionStatus.inLobby:
				LobbyLayout();
				LobbyBrowser();
				break;
			case ConnectionStatus.creatingRoom:
				RoomWizard();
				break;
			case ConnectionStatus.host:
				RoomLobby(true);
				break;
			case ConnectionStatus.client:
				RoomLobby(false);
				break;
			case ConnectionStatus.inGame:
				break;
		}
	}
	
	private void ConnectionWizard()
	{
		GUI.Label(new Rect(10, 80, 100, 30), "Player name: ");
		playerName = GUI.TextField(new Rect(115, 80, 100, 20), playerName);
		if(GUI.Button(new Rect(10, 120, 150, 40), "Connect to Server"))
		{
			PhotonNetwork.ConnectUsingSettings("v0.1");
			PhotonNetwork.player.name = playerName;
			status = ConnectionStatus.inLobby;
		}
	}
	
	private void LobbyLayout()
	{
		GUI.Box(new Rect(0f,0f, Screen.width, Screen.height), "");
		
		if(!string.IsNullOrEmpty(PhotonNetwork.player.name))
			GUI.Label(new Rect(10, 10, 400, 30), "Welcome " + PhotonNetwork.player.name + ", to the Brobots Lobby!");	
		
		GUI.Label(new Rect(10, 50, 250, 30), "Connection Status: " + PhotonNetwork.connectionStateDetailed.ToString());
	}
	
	private void RoomWizard()
	{
		GUILayout.BeginArea(new Rect(10, 30, 300, 200), "");
		GUILayout.Space(50);
		GUILayout.BeginHorizontal();
		GUILayout.Label("Roomname: ");
		roomName = GUILayout.TextField(roomName, GUILayout.MaxWidth(170));
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label("Max players: ");
		maxPlayers = int.Parse(GUILayout.TextField(maxPlayers.ToString()));
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		privateRoom = GUILayout.Toggle(privateRoom, "Private room:");
		GUILayout.EndHorizontal();
		if(GUILayout.Button("Create Room"))
		{
			PhotonNetwork.CreateRoom(roomName, !privateRoom, true, maxPlayers); 
			status = ConnectionStatus.host;
		}
		GUILayout.EndArea();
	}
	
	private void Update()
	{
		if(debugging)
		{
			if(Application.isEditor)
			{
				if(PhotonNetwork.connected)
				{
					Debug.Log("Total players in rooms: " + PhotonNetwork.countOfPlayersInRooms.ToString());
					Debug.Log("Total players on master: " + PhotonNetwork.countOfPlayers.ToString());
					Debug.Log("Number of rooms: " + PhotonNetwork.countOfRooms.ToString());
				}
			}
		}
	}
	
	private void LobbyBrowser()
	{
		if(GUI.Button(new Rect(10, 80, 150, 40), "Create Room"))
		{
			status = ConnectionStatus.creatingRoom;
		}
		if(GUI.Button(new Rect(160, 80, 150, 40), "Disconnect"))	
		{
			PhotonNetwork.Disconnect();
			status = ConnectionStatus.disconnected;
		}
		
		GUI.Box(new Rect(10, 130, Screen.width - 10, Screen.height - 135), "");
		GUILayout.BeginArea(new Rect(15, 140,  Screen.width - 15, Screen.height - 140));
		GUILayout.BeginHorizontal();
		GUILayout.Box("Gamename:");
		GUILayout.Box("Players:");
		GUILayout.Box("Max Players:");
		GUILayout.Box("Join:");
		GUILayout.EndHorizontal();
		
		if (PhotonNetwork.GetRoomList().Length == 0)
        {
            GUILayout.Label("..no games available..");
        }
		else
		{
			List<RoomInfo> roomInfos = new List<RoomInfo>(PhotonNetwork.GetRoomList());
			
			if(roomInfos.Count > 0)
			{
				foreach (RoomInfo info in roomInfos)
				{
				    GUILayout.BeginHorizontal();
					GUILayout.Box(info.name);
					GUILayout.Box(info.playerCount.ToString());
					GUILayout.Box(info.maxPlayers.ToString());
					if(GUILayout.Button("Join!"))
					{
						PhotonNetwork.JoinRoom(info.name);

						status = ConnectionStatus.client;
					}
					GUILayout.EndHorizontal();
				} 
			}
		}
		GUILayout.EndArea();
	}
	
	private void RoomLobby(bool host)
	{
		GUILayout.BeginArea(new Rect(10, 75, 300, 200), "");
		if(GUILayout.Button("Quit to Lobby"))
		{
			status = ConnectionStatus.inLobby;
			PhotonNetwork.LeaveRoom();
		}	
		GUILayout.BeginHorizontal(GUILayout.Width(300));
		GUILayout.Box("Player: ", GUILayout.Width(200));
		GUILayout.Box("Ping: ", GUILayout.Width(100));
		GUILayout.EndHorizontal();
		foreach(PhotonPlayer player in PhotonNetwork.playerList)
		{
			GUILayout.BeginHorizontal(GUILayout.Width(300));
			GUILayout.Box(player.name, GUILayout.Width(200));
			GUILayout.EndHorizontal();
		}
		if(host)
		{
			if(PhotonNetwork.isMasterClient)
			{
				if(GUILayout.Button("Start Game"))
				{
					if(PhotonNetwork.playerList.Length == maxPlayers || debugging)
					{
						PhotonView view = Hierarchy.GetComponentWithTag<PhotonView>("GameUtilities");
							view.RPC("LoadLevel", PhotonTargets.All, roomLevel);
					}
				}
			}
		}
		GUILayout.EndArea();
	}
}
