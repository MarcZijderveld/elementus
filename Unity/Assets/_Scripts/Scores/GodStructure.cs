using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class GodStructure : MonoBehaviour {
    public Transform CrystalPlayerOne;
    public Transform CrystalPlayerTwo;
    public Transform partical;
    public Transform endPartical;
    Transform healthCrystal;
    List<Transform> crystals = new List<Transform>();
    public bool localPlayer;
    public int GodStructID;
    int oldHealth;
    
    private PlayerDataHandler _playerDataHandler = null;
    public PlayerDataHandler playerDataHandler
    {
        get
        {
            if (_playerDataHandler == null)
            {
                _playerDataHandler = Hierarchy.GetComponentWithTag<PlayerDataHandler>("PlayerDataHandler");
            }
            return _playerDataHandler;
        }
    }


    private NetworkPlayerDataHandler _networkPlayerDataHandler = null;
    public NetworkPlayerDataHandler networkPlayerDataHandler
    {
        get
        {
            if (_networkPlayerDataHandler == null)
            {
                _networkPlayerDataHandler = Hierarchy.GetComponentWithTag<NetworkPlayerDataHandler>("NetworkPlayerDataHandler");
            }
            return _networkPlayerDataHandler;
        }
    } 
	// Use this for initialization
	void Start ()
    {
        //Debug.Log(playerDataHandler.GetID());
        if (GodStructID == playerDataHandler.GetID())
            localPlayer = true;
        
        if (localPlayer)
            healthCrystal = Instantiate(CrystalPlayerOne, transform.position + new Vector3(0, 1.334011f, 0), transform.rotation) as Transform;
        else
            healthCrystal = Instantiate(CrystalPlayerTwo, transform.position +  new Vector3(0, 1.334011f, 0), transform.rotation) as Transform;
        foreach (Transform child in healthCrystal)
        {
            crystals.Add(child);
        }

        if (localPlayer)
        {
            oldHealth = playerDataHandler.GetPoints();
        }
        else
        {
            oldHealth = networkPlayerDataHandler.GetPoints();
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
        int hp = 0;
        if (localPlayer)
            hp = playerDataHandler.GetPoints();
        else
            hp = networkPlayerDataHandler.GetPoints();
        

        if(oldHealth != hp)
        {
            if (hp != 0)
                Instantiate(partical, crystals[hp-1].position, transform.rotation);
            else
                Instantiate(endPartical, healthCrystal.position, endPartical.rotation);

        }
	    for(int i= hp -1; i < crystals.Count; i++ )
        {
            if (i < 0)
                break;
            crystals[i].renderer.enabled = false;
        }


        oldHealth = hp;

	}
}
