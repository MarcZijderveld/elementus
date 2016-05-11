using UnityEngine;
using System.Collections;

public class GameOver : GUIMemberComponent 
{
    public bool gameDone = false;
    public bool won = false;
    public bool draw = false;
	public bool lose = false;

    private NetworkPlayerDataHandler _NetworkPlayerDataHandeler = null;
    public NetworkPlayerDataHandler NetworkPlayerDataHandeler
    {
        get
        {
            if (_NetworkPlayerDataHandeler == null)
            {
                _NetworkPlayerDataHandeler = GameObject.Find("NetworkPlayerDataHandeler").GetComponent<NetworkPlayerDataHandler>();
            }
            return _NetworkPlayerDataHandeler;
        }
    }

    private PlayerDataHandler _PlayerDataHandeler = null;
    public PlayerDataHandler PlayerDataHandeler
    {
        get
        {
            if (_PlayerDataHandeler == null)
            {
                _PlayerDataHandeler = GameObject.Find("PlayerDataHandeler").GetComponent<PlayerDataHandler>();
            }
            return _PlayerDataHandeler;
        }
    }

    private MovementController _movementController = null;
    public MovementController movementController
    {
        get
        {
            if (_movementController == null)
            {
                _movementController = GameObject.Find("MovementController").GetComponent<MovementController>();
            }
            return _movementController;
        }
    } 

	public PauseMenu menu;

	// Update is called once per frame
	void Update () 
	{
		if(gameDone && draw)
			menu.SetPause(false, "Draw..");
		if(gameDone && won)
			menu.SetPause(false, "Victory!");
		if(gameDone && lose)
			menu.SetPause(false, "Defeated..");

        if (NetworkPlayerDataHandeler.GetPoints() <= 0 && PlayerDataHandeler.GetPoints() <= 0)
        {
            gameDone = true;
            draw = true;
            return;
        }

        if (NetworkPlayerDataHandeler.GetPoints() <= 0)
        {
            gameDone = true;
            won = true;
        }

        if (PlayerDataHandeler.GetPoints() <= 0)
        {
            gameDone = true;
			lose = true;
        }
	}
}
