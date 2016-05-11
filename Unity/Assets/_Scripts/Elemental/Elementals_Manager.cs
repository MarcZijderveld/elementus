using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Elementals_Manager : Photon.MonoBehaviour 
{
    public GameObject[] Godstructers;
	[System.Serializable]
	public class SpawnPosition
	{
		public int player;
		public List<Vector2> positions = new List<Vector2>();
		public Vector3 rotation;
	}

	private	PlayerDataHandler 	_playerDataHandler	= null; 
	public	PlayerDataHandler 	playerDataHandler
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

    public bool go = false;
    public Transform Elemental;
	public int playerAmount = 2;
    public int UnitCount = 5;

	public bool local = true;

	public List<SpawnPosition> spawnPositions = new List<SpawnPosition>();

    private MovementController momvementController;

	private void Start()
    {
        Godstructers = GameObject.FindGameObjectsWithTag("GodStructure");
		if(Hierarchy.GetComponentWithTag<GameSequence>("GameSequence").local)
		{
			playerDataHandler.SetID(0);	
			StartSpawn("");
		}
	}

	[RPC]
    void StartSpawn(string debug)
    {
        momvementController = GameObject.Find("MovementController").GetComponent<MovementController>();
        for(int p = 1; p <= playerAmount; p++)
        {
            for(int i = 0; i < UnitCount;i++)
            {
				CreateUnits(p * 100 + i,p == playerDataHandler.GetID(), (p - 1), i);
            }
        }
    }

    void CreateUnits(int id,bool Mine, int player, int index)
    {
        Transform elemental = Instantiate(Elemental) as Transform;
		elemental.GetComponent<Elemental_Change>().Init();

		SpawnPosition pos = spawnPositions.First(s => s.player == player);

		elemental.GetComponent<AnimationStates>().movement.SetGridPos(pos.positions[index]);

		foreach (Elemental_Animator anim in elemental.GetComponentsInChildren<Elemental_Animator>()) 
		{
			anim.SetAnimController (elemental.GetComponent<AnimationController>());
		}

		foreach (Elemental_Attack attk in elemental.GetComponentsInChildren<Elemental_Attack>()) 
		{
			attk.SetAnimController (elemental.GetComponent<AnimationController>());
		}
	
		foreach (Renderer ren in elemental.GetComponentsInChildren<Renderer>()) 
		{
			ren.enabled = false;
		}

		elemental.transform.rotation = Quaternion.Euler(pos.rotation);

        Elemental_Properties elemental_Properties = elemental.GetComponent<Elemental_Properties>();
        elemental_Properties.id = id;
        elemental_Properties.isMine = Mine;
		elemental.parent = transform;
		elemental.GetComponent<Elemental_Change> ().ChangeElement (Element_Enum.Types.fire);
		momvementController.AddUnit(id, elemental);
    }
}
