using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MovementController : Photon.MonoBehaviour
{
    float waterTiming = 2.2f;
    float fireTiming = 1.2f;
    float grasTiming = 0.8f;

    private SoundSettings _soundSettings = null; 
    public SoundSettings soundSettings
    {
        get
        {
            if (_soundSettings == null)
            {
                _soundSettings = Hierarchy.GetComponentWithTag<SoundSettings>("SoundManager");
            }
            return _soundSettings;
        }
    } 

    private KingOfTheHill _kingOfTheHill = null;
    public KingOfTheHill kingOfTheHill
    {
        get
        {
            if (_kingOfTheHill == null)
            {
                _kingOfTheHill = GameObject.Find("KingOfTheHill").GetComponent<KingOfTheHill>(); ;
            }
            return _kingOfTheHill;
        }
    } 

	private	CameraController 	_cameraControl	= null; 
	public	CameraController 	cameraControl
	{
		get
		{
			if (_cameraControl == null)
			{
				_cameraControl = Hierarchy.GetComponentWithTag<CameraController>("MainCamera");
			}
			return _cameraControl;
		}		
	} 

	private	ResultMessages 	_results	= null; 
	public	ResultMessages 	results
	{
		get
		{
			if (_results == null)
			{
				_results = Hierarchy.GetComponentWithTag<ResultMessages>("ResultMessages");
			}
			return _results;
		}		
	}

    private Elementals_Respawn _elementals_Respawn = null;
    public Elementals_Respawn elementals_Respawn
	{
		get
		{
            if (_elementals_Respawn == null)
			{
                _elementals_Respawn = Hierarchy.GetComponentWithTag<Elementals_Respawn>("UnitManager");
			}
            return _elementals_Respawn;
		}		
	}

    private DataSync _data = null;
    public DataSync data
    {
        get
        {
            if (_data == null)
            {
                _data = Hierarchy.GetComponentWithTag<DataSync>("DataSync");
            }
            return _data;
        }
    } 

    public bool resolve = false;

    public struct Move
    {
         public   int id;
         public   Vector2 BeginGridPosition;
         public   Vector2 EndGridPosition;
         public   Element_Enum.Types beginType;
         public   Element_Enum.Types endType;
    }

    public Dictionary<int, Transform> 	units 				= new Dictionary<int, Transform>();
	private Dictionary<int, Move>		localMoves 			= new Dictionary<int, Move>(),
	 									networkMoves 		= new Dictionary<int, Move>(),
	 									localClashMoves 	= new Dictionary<int, Move>(),
										networkClashMoves 	= new Dictionary<int, Move>();

    private Controller 		controller;
    private Grid 			grid;
	private GameSequence 	gameSequence;

	private bool			running 	= false,
							localDone	= false,
							networkDone = false;

    private Elemental_Ghosting ghosting;

    private CrystalsSpawning _crystal = null;
    public CrystalsSpawning crystal
    {
        get
        {
            if (_crystal == null)
            {
                _crystal = GameObject.Find("CrystalManager").GetComponent<CrystalsSpawning>();
            }
            return _crystal;
        }
    } 
        
	// Use this for initialization
	void Start () 
	{
        controller = GameObject.Find("Controller").GetComponent<Controller>();
        grid = GameObject.Find("GridCreator").GetComponent<Grid>();
		gameSequence = Hierarchy.GetComponentWithTag<GameSequence>("GameSequence");
        ghosting = GameObject.Find("Elemenals_Manager").GetComponent<Elemental_Ghosting>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (resolve)
		{
           	Resolve();
			resolve = false;
		}
		if(data.resolving)
		{
			if(!data.CheckReady())
			{
				return;
			}
			if(!running)
			{
				StartCoroutine("Resolve");
			}
		}
	}

	public void Ready()
	{
		foreach (KeyValuePair<int, Move> entry in localMoves) 
		{
			photonView.RPC ("SendMoveData", PhotonTargets.Others, entry.Value.id, entry.Value.BeginGridPosition, entry.Value.EndGridPosition, entry.Value.beginType, entry.Value.endType);
		}

		data.SendDataConfirmation();
		data.SetResolve (true);

		if (controller.unit != null) 
		{
			controller.unit.GetComponentInChildren<AnimationStates>().movement.active = false;
			controller.unit = null;
			grid.ResetHighlight ();
		}
	}

    public void AddMove(int id, Vector2 BeginGridPosition, Vector2 EndGridPosition, Element_Enum.Types beginType, Element_Enum.Types endType)
    {

        if(!CheckAvailable(EndGridPosition))
            return;
        Move move = new Move();
        move.id = id;
        move.BeginGridPosition = BeginGridPosition;
        move.EndGridPosition = EndGridPosition;
        move.beginType = beginType;
		move.endType = beginType;
        ghosting.AddGhost(id, grid.GetPosition((int)BeginGridPosition.x, (int)BeginGridPosition.y), grid.GetPosition((int)EndGridPosition.x, (int)EndGridPosition.y), endType);

		if(localMoves.ContainsKey(id))
		{
			move.beginType = localMoves[id].beginType;
			move.endType = localMoves[id].beginType;
			localMoves.Remove(id);
		}

		localMoves.Add(id, move);
    }

	public void RemoveMove(int id)
	{
		if(localMoves.ContainsKey(id))
		{
			localMoves.Remove(id);
			ghosting.RemoveGhost(id);
		}
	}

	public void AddColor(int id, Vector2 BeginGridPosition, Vector2 EndGridPosition, Element_Enum.Types beginType)
	{
		Move move = new Move();

		move.id = id;
		move.beginType = beginType;
		move.endType = beginType;
		move.BeginGridPosition = BeginGridPosition;
		move.EndGridPosition = EndGridPosition;
        ghosting.changeElement(id, beginType);
		if(localMoves.ContainsKey(id))
		{
			Move overrideMove = localMoves[id];
			overrideMove.beginType = beginType;
			overrideMove.endType = beginType;
			localMoves[id] = overrideMove;
			return;
		}
		else
		{
			localMoves.Add(id, move);
		}
	}

    public void AddUnit(int id,Transform unit)
    {
		if(!units.ContainsKey(id))
        	units.Add(id, unit);
    }

	public void RemoveUnit(int unit)
	{
		if(units.ContainsKey(unit))
			units.Remove(unit);
	}

	[RPC]
	private void SendMoveData(int id, Vector2 begin, Vector2 end, int beginType, int endType)
	{
		Move move = new Move();
		move.id = id;
		move.BeginGridPosition = begin;
		move.EndGridPosition = end;
		move.beginType = (Element_Enum.Types) beginType;
		move.endType = (Element_Enum.Types) beginType;

		if(!networkMoves.ContainsKey(id))
			networkMoves.Add(id, move);
	}
	
	public IEnumerator LocalResolve()
	{
		localDone = false;

		List<int> buffer = new List<int>(localMoves.Keys);
		foreach (int i in buffer)
		{
			Move move = localMoves[i];
			units[i].GetComponent<Elemental_Change>().StartCoroutine("ChangeElement", move.endType);
			yield return units[i].GetComponentInChildren<AnimationStates>().animator.StartCoroutine("Move", move);
		}

		localDone = true;

		yield return null;
	}

	public IEnumerator NetworkResolve()
	{
		networkDone = false;

		List<int> buffer = new List<int>(networkMoves.Keys);
		foreach (int i in buffer)
		{
			Move move = networkMoves[i];
			units[i].GetComponent<Elemental_Change>().StartCoroutine("ChangeElement", move.endType);
			yield return units[i].GetComponentInChildren<AnimationStates>().animator.StartCoroutine("Move", move);
		}

		networkDone = true;

		yield return null;
	}

	private bool same = false;

	void ResolveElements(Transform winner, Transform loser)
	{

	}

	public IEnumerator ClashResolve()
	{
		List<int> buffer1 = new List<int>(localClashMoves.Keys);
		List<int> buffer2 = new List<int>(networkClashMoves.Keys);
		
		//Match the local and network moves and clash.
		foreach (int i in buffer1)
		{
			foreach(int j in buffer2)
			{
				if(!localClashMoves.ContainsKey(i) || !networkClashMoves.ContainsKey(j))
					continue;

				Move localMove = localClashMoves[i];
				Move networkMove = networkClashMoves[j];

				if(localMove.EndGridPosition == networkMove.EndGridPosition)
				{
					//Debug.Log("pos the same!");
					if(localMove.endType == networkMove.endType)
					{
						//Debug.Log("Both the same!");
						same = true;
					}

					//Debug.Log("focusOn");
					yield return cameraControl.StartCoroutine("FocusOn", grid.GetPosition((int)localMove.EndGridPosition.x, (int)localMove.EndGridPosition.y));

					//Debug.Log("changeelement");
					yield return units[i].GetComponent<Elemental_Change>().StartCoroutine("ChangeElement",localMove.endType);
					yield return units[j].GetComponent<Elemental_Change>().StartCoroutine("ChangeElement",networkMove.endType);

					//yield return new WaitForSeconds(1.5f);
					if(localMove.endType == Element_Enum.Types.fire && networkMove.endType == Element_Enum.Types.water)
					{
                        soundWater();
                        soundSettings.Play("deathFire");
						yield return units[networkMove.id].GetComponentInChildren<AnimationStates>().attack.StartCoroutine("Attack", units[localMove.id]);
						ResolveElements(units[networkMove.id], units[localMove.id]);
						units[localMove.id].GetComponentInChildren<AnimationStates>().death.StartCoroutine("Death");
						results.StartCoroutine("StartText", ResultMessages.TextType.lose);
                        

					}
					if(localMove.endType == Element_Enum.Types.fire && networkMove.endType == Element_Enum.Types.grass)
					{
                        soundFire();
                        soundSettings.Play("deathGrass");
						yield return units[localMove.id].GetComponentInChildren<AnimationStates>().attack.StartCoroutine("Attack", units[networkMove.id]);
						ResolveElements(units[localMove.id], units[networkMove.id]);
						units[networkMove.id].gameObject.GetComponentInChildren<AnimationStates>().death.StartCoroutine("Death");
						results.StartCoroutine("StartText", ResultMessages.TextType.win);
                       

					}
					if(localMove.endType == Element_Enum.Types.water && networkMove.endType == Element_Enum.Types.grass)
					{
                        soundSettings.Play("combatGrass");
                        soundSettings.Play("deathWater");
						yield return units[networkMove.id].GetComponentInChildren<AnimationStates>().attack.StartCoroutine("Attack", units[localMove.id]);
						ResolveElements(units[networkMove.id], units[localMove.id]);
						units[localMove.id].gameObject.GetComponentInChildren<AnimationStates>().death.StartCoroutine("Death");
						results.StartCoroutine("StartText", ResultMessages.TextType.lose);
                       

					}
					if(localMove.endType == Element_Enum.Types.water && networkMove.endType == Element_Enum.Types.fire)
					{
                        soundWater();
                        soundSettings.Play("deathFire");
						yield return units[localMove.id].GetComponentInChildren<AnimationStates>().attack.StartCoroutine("Attack", units[networkMove.id]);
						ResolveElements(units[localMove.id], units[networkMove.id]);
						units[networkMove.id].gameObject.GetComponentInChildren<AnimationStates>().death.StartCoroutine("Death");
						results.StartCoroutine("StartText", ResultMessages.TextType.win);


					}
					if(localMove.endType == Element_Enum.Types.grass && networkMove.endType == Element_Enum.Types.fire)
					{
                        soundFire();
                        soundSettings.Play("deathGrass");
						yield return units[networkMove.id].GetComponentInChildren<AnimationStates>().attack.StartCoroutine("Attack", units[localMove.id]);
						ResolveElements(units[networkMove.id], units[localMove.id]);
						units[localMove.id].gameObject.GetComponentInChildren<AnimationStates>().death.StartCoroutine("Death");

						results.StartCoroutine("StartText", ResultMessages.TextType.lose);
                       
					}
					if(localMove.endType == Element_Enum.Types.grass && networkMove.endType == Element_Enum.Types.water)
					{
                        soundSettings.Play("combatGrass");
                        soundSettings.Play("deathWater");
						yield return units[localMove.id].GetComponentInChildren<AnimationStates>().attack.StartCoroutine("Attack", units[networkMove.id]);
						ResolveElements(units[localMove.id], units[networkMove.id]);
						units[networkMove.id].gameObject.GetComponentInChildren<AnimationStates>().death.StartCoroutine("Death");
						results.StartCoroutine("StartText", ResultMessages.TextType.win);

					}

					if(units.ContainsKey(i))
					{
						//Debug.LogError(units[i].gameObject.GetComponent<Elemental_Properties>().id);	
						//Debug.Log("Contains Local");
						yield return units[i].GetComponentInChildren<AnimationStates>().animator.StartCoroutine("Move", localMove);
					}
					if(units.ContainsKey(j))
					{
						//Debug.LogError(units[j].gameObject.GetComponent<Elemental_Properties>().id);	
						//Debug.Log("Continas Network");
						yield return units[j].GetComponentInChildren<AnimationStates>().animator.StartCoroutine("Move", networkMove);
					}

					if(same)	
					{
						//Debug.Log("moveBack");
						yield return units[i].GetComponentInChildren<AnimationStates>().animator.StartCoroutine("MoveBack", localMove);
						yield return units[j].GetComponentInChildren<AnimationStates>().animator.StartCoroutine("MoveBack", networkMove);
						results.StartCoroutine("StartText", ResultMessages.TextType.draw);
						same = false;
					}

					//Debug.Log("removeEntries");
					localClashMoves.Remove(i);
					networkClashMoves.Remove(j);
					break;
				}
			}
		}

		Debug.Log ("double resolve done");

		//Debug.Log("Local " + localClashMoves.Count);
		//Debug.Log("Network " + networkClashMoves.Count);

		List<int> buffer3 = new List<int>(localClashMoves.Keys);
		List<int> buffer4 = new List<int>(networkClashMoves.Keys);

		//Local moves which one unit moving.

		Debug.Log("Local " + localClashMoves.Count);

		if(localClashMoves.Count > 0)
		{
			foreach(int i in buffer3)
			{
				Move localMove = localClashMoves[i];

				//Transform unit = units.First(k => k.Value.GetComponentInChildren<AnimationStates>().movement.GetCurrentPos() == localMove.EndGridPosition).Value;
				Transform unit;

				if(units.FirstOrDefault(k => k.Value.GetComponentInChildren<AnimationStates>().movement.GetCurrentPos() == localMove.EndGridPosition).Value == null)
					continue;
				
				unit = units.FirstOrDefault(k => k.Value.GetComponentInChildren<AnimationStates>().movement.GetCurrentPos() == localMove.EndGridPosition).Value;

				//Debug.LogError("moooo22");

				if(localMove.endType == unit.GetComponent<Elemental_Change>().element)
					same = true;
					
				yield return cameraControl.StartCoroutine("FocusOn", grid.GetPosition((int)localMove.EndGridPosition.x, (int)localMove.EndGridPosition.y));
					
				yield return units[i].GetComponent<Elemental_Change>().StartCoroutine("ChangeElement",localMove.endType);

				if(localMove.endType == Element_Enum.Types.fire && unit.GetComponent<Elemental_Change>().element == Element_Enum.Types.water)
				{
                    soundWater();
					yield return unit.GetComponentInChildren<AnimationStates>().attack.StartCoroutine("Attack", units[localMove.id]);
					ResolveElements(unit, units[localMove.id]);
					units[localMove.id].gameObject.GetComponentInChildren<AnimationStates>().death.StartCoroutine("Death");
					results.StartCoroutine("StartText", ResultMessages.TextType.lose);
				}
				if(localMove.endType == Element_Enum.Types.fire && unit.GetComponent<Elemental_Change>().element == Element_Enum.Types.grass)
				{
                    soundFire();
					yield return units[localMove.id].GetComponentInChildren<AnimationStates>().attack.StartCoroutine("Attack", unit);
					ResolveElements(units[localMove.id],unit);
					unit.gameObject.GetComponentInChildren<AnimationStates>().death.StartCoroutine("Death");
					results.StartCoroutine("StartText", ResultMessages.TextType.win);
				}
				if(localMove.endType == Element_Enum.Types.water && unit.GetComponent<Elemental_Change>().element == Element_Enum.Types.grass)
				{
                     StartCoroutine(soundSettings.Play("combatGrass",grasTiming));
					yield return unit.GetComponentInChildren<AnimationStates>().attack.StartCoroutine("Attack", units[localMove.id]);
					ResolveElements(unit, units[localMove.id]);
					units[localMove.id].gameObject.GetComponentInChildren<AnimationStates>().death.StartCoroutine("Death");
					results.StartCoroutine("StartText", ResultMessages.TextType.lose);
				}
				if(localMove.endType == Element_Enum.Types.water && unit.GetComponent<Elemental_Change>().element == Element_Enum.Types.fire)
				{
                    soundWater();
					yield return units[localMove.id].GetComponentInChildren<AnimationStates>().attack.StartCoroutine("Attack", unit);
					ResolveElements(units[localMove.id],unit);
					unit.gameObject.GetComponentInChildren<AnimationStates>().death.StartCoroutine("Death");
					results.StartCoroutine("StartText", ResultMessages.TextType.win);
				}
				if(localMove.endType == Element_Enum.Types.grass && unit.GetComponent<Elemental_Change>().element == Element_Enum.Types.fire)
				{
                    soundFire();
					yield return unit.GetComponentInChildren<AnimationStates>().attack.StartCoroutine("Attack", units[localMove.id]);
					ResolveElements(unit, units[localMove.id]);
					units[localMove.id].gameObject.GetComponentInChildren<AnimationStates>().death.StartCoroutine("Death");
					results.StartCoroutine("StartText", ResultMessages.TextType.lose);
				}
				if(localMove.endType == Element_Enum.Types.grass && unit.GetComponent<Elemental_Change>().element == Element_Enum.Types.water)
				{
                    StartCoroutine(soundSettings.Play("combatGrass",grasTiming));
					yield return units[localMove.id].GetComponentInChildren<AnimationStates>().attack.StartCoroutine("Attack", unit);
					ResolveElements(units[localMove.id],unit);
					unit.gameObject.GetComponentInChildren<AnimationStates>().death.StartCoroutine("Death");
					results.StartCoroutine("StartText", ResultMessages.TextType.win);
				}

				if(units.ContainsKey(i))
				{
					//Debug.Log("Contains local");
					yield return units[i].GetComponentInChildren<AnimationStates>().animator.StartCoroutine("Move", localMove);
				}

				if(same)
				{
					yield return units[i].GetComponentInChildren<AnimationStates>().animator.StartCoroutine("MoveBack", localMove);
					same = false;
					results.StartCoroutine("StartText", ResultMessages.TextType.draw);
				}
					
				localClashMoves.Remove(i);
				//Debug.Log(localMove.id);
			}
		}


		Debug.Log("Network " + networkClashMoves.Count);
		
		//Network moves which one unit moving.
		if(networkClashMoves.Count > 0)
		{
			foreach(int j in buffer4)
			{
				Move networkMove = networkClashMoves[j];

				//Transform unit = units.First(k => k.Value.GetComponentInChildren<AnimationStates>().movement.GetCurrentPos() == networkMove.EndGridPosition).Value;
				Transform unit;
			
				if(units.FirstOrDefault(k => k.Value.GetComponentInChildren<AnimationStates>().movement.GetCurrentPos() == networkMove.EndGridPosition).Value == null)
				   continue;

				unit = units.FirstOrDefault(k => k.Value.GetComponentInChildren<AnimationStates>().movement.GetCurrentPos() == networkMove.EndGridPosition).Value;

				//Debug.LogError("mooooo");

				if(networkMove.endType == unit.GetComponent<Elemental_Change>().element)
					same = true;
				
				yield return cameraControl.StartCoroutine("FocusOn", grid.GetPosition((int)networkMove.EndGridPosition.x, (int)networkMove.EndGridPosition.y));
				
				yield return units[j].GetComponent<Elemental_Change>().StartCoroutine("ChangeElement",networkMove.endType);

				if(networkMove.endType == Element_Enum.Types.fire && unit.GetComponent<Elemental_Change>().element == Element_Enum.Types.water)
				{
                    soundWater();
					yield return unit.GetComponentInChildren<AnimationStates>().attack.StartCoroutine("Attack", units[networkMove.id]);
					ResolveElements(unit, units[networkMove.id]);
					units[networkMove.id].gameObject.GetComponentInChildren<AnimationStates>().death.StartCoroutine("Death");
					results.StartCoroutine("StartText", ResultMessages.TextType.win);
				}
				if(networkMove.endType == Element_Enum.Types.fire && unit.GetComponent<Elemental_Change>().element == Element_Enum.Types.grass)
				{

                    soundFire();
					yield return units[networkMove.id].GetComponentInChildren<AnimationStates>().attack.StartCoroutine("Attack", unit);
					ResolveElements(units[networkMove.id], unit);
					unit.gameObject.GetComponentInChildren<AnimationStates>().death.StartCoroutine("Death");
					results.StartCoroutine("StartText", ResultMessages.TextType.lose);
				}
				if(networkMove.endType == Element_Enum.Types.water && unit.GetComponent<Elemental_Change>().element == Element_Enum.Types.grass)
				{
                    StartCoroutine(soundSettings.Play("combatGrass", grasTiming));    
					yield return unit.GetComponentInChildren<AnimationStates>().attack.StartCoroutine("Attack", units[networkMove.id]);
					ResolveElements(unit, units[networkMove.id]);
					units[networkMove.id].gameObject.GetComponentInChildren<AnimationStates>().death.StartCoroutine("Death");
					results.StartCoroutine("StartText", ResultMessages.TextType.win);
				}
				if(networkMove.endType == Element_Enum.Types.water && unit.GetComponent<Elemental_Change>().element == Element_Enum.Types.fire)
				{
                    soundWater();
					yield return units[networkMove.id].GetComponentInChildren<AnimationStates>().attack.StartCoroutine("Attack", unit);
					ResolveElements(units[networkMove.id], unit);
					unit.gameObject.GetComponentInChildren<AnimationStates>().death.StartCoroutine("Death");
					results.StartCoroutine("StartText", ResultMessages.TextType.lose);
				}
				if(networkMove.endType == Element_Enum.Types.grass && unit.GetComponent<Elemental_Change>().element == Element_Enum.Types.fire)
				{
                    soundFire();
					yield return unit.GetComponentInChildren<AnimationStates>().attack.StartCoroutine("Attack", units[networkMove.id]);
					ResolveElements(unit, units[networkMove.id]);
					units[networkMove.id].gameObject.GetComponentInChildren<AnimationStates>().death.StartCoroutine("Death");
					results.StartCoroutine("StartText", ResultMessages.TextType.win);
				}
				if(networkMove.endType == Element_Enum.Types.grass && unit.GetComponent<Elemental_Change>().element == Element_Enum.Types.water)
				{
                    StartCoroutine(soundSettings.Play("combatGrass", grasTiming));
					yield return units[networkMove.id].GetComponentInChildren<AnimationStates>().attack.StartCoroutine("Attack", unit);
					ResolveElements(units[networkMove.id], unit);
					unit.gameObject.GetComponentInChildren<AnimationStates>().death.StartCoroutine("Death");
					results.StartCoroutine("StartText", ResultMessages.TextType.lose);
				}

				if(units.ContainsKey(j))
				{
					//Debug.Log("Contains network");
					yield return units[j].GetComponentInChildren<AnimationStates>().animator.StartCoroutine("Move", networkMove);
				}
				if(same)
				{
					yield return units[j].GetComponentInChildren<AnimationStates>().animator.StartCoroutine("MoveBack", networkMove);
					same = false;
					results.StartCoroutine("StartText", ResultMessages.TextType.draw);
				}
				networkClashMoves.Remove(j);	
			}
		}

		yield return cameraControl.StartCoroutine("Reset");

		yield return null;
	}

    public bool CheckAvailable(Vector2 pos)
    {
        foreach (KeyValuePair<int, Move> move in localMoves)
        {
            if (localMoves.ContainsKey(move.Key))
            {
                if (move.Value.EndGridPosition == pos)
                {
                    return false;
                }
            }
        }
        return true;
    }
    /// <Sound>
    ///

    void soundFire()
    {
        StartCoroutine(soundSettings.Play("combatFire", fireTiming));
        StartCoroutine(soundSettings.Play("combatFire", 2f));
        StartCoroutine(soundSettings.Play("combatFire", 3.2f));

    }

    void soundWater()
    {
        StartCoroutine(soundSettings.Play("combatWater", 2.5f));
        StartCoroutine(soundSettings.Play("combatWater", 2.7f));
        StartCoroutine(soundSettings.Play("combatWater", 2.9f));
        StartCoroutine(soundSettings.Play("combatWater", 3.1f));
        StartCoroutine(soundSettings.Play("combatWater", 3.3f));
    }


    public IEnumerator Resolve()
	{
		running = true;
        ghosting.Reset();
		localClashMoves = localMoves.Where(k1 => networkMoves.Any(k2 => k1.Value.EndGridPosition == k2.Value.EndGridPosition)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
		networkClashMoves = networkMoves.Where(k1 => localMoves.Any(k2 => k1.Value.EndGridPosition == k2.Value.EndGridPosition)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

		foreach(KeyValuePair<int, Move> move in localMoves)
		{
			foreach(KeyValuePair<int, Transform> unit in units)
			{
				if(move.Key != unit.Key)
				{
					if(move.Value.EndGridPosition == unit.Value.GetComponentInChildren<AnimationStates>().movement.GetCurrentPos() && !networkMoves.ContainsKey(unit.Value.GetComponent<Elemental_Properties>().id))
					{
						if(!localClashMoves.ContainsKey(move.Key))
							localClashMoves.Add(move.Key, move.Value);
					}
				}
			}
		}

		foreach(KeyValuePair<int, Move> move in networkMoves)
		{
			foreach(KeyValuePair<int, Transform> unit in units)
			{
				if(move.Key != unit.Key)
				{
					if(move.Value.EndGridPosition == unit.Value.GetComponentInChildren<AnimationStates>().movement.GetCurrentPos() && !localMoves.ContainsKey(unit.Value.GetComponent<Elemental_Properties>().id))
					{
						if(!networkClashMoves.ContainsKey(move.Key))
							networkClashMoves.Add(move.Key, move.Value);
					}
				}
			}
		}

		foreach(KeyValuePair<int, Move> move in localClashMoves)
		{
			if(localMoves.ContainsKey(move.Key))
				localMoves.Remove(move.Key);
		}

		foreach(KeyValuePair<int, Move> move in networkClashMoves)
		{
			if(networkMoves.ContainsKey(move.Key))
				networkMoves.Remove(move.Key);
		}

//		Debug.Log ("LocalResolve");

		StartCoroutine("LocalResolve");

//		Debug.Log ("NetworkResolve");

		StartCoroutine("NetworkResolve");

		while(!localDone || !networkDone)
			yield return null;

//		Debug.Log ("Both Done");

		if(localClashMoves.Count > 0 || networkClashMoves.Count > 0)
			yield return StartCoroutine("ClashResolve");

//		Debug.Log ("Clashresolve done");

        foreach (KeyValuePair<int, Transform> entry in units)
		{
			units[entry.Key].GetComponent<Elemental_kingOfTheHill>().CheckForScore();
		}

        elementals_Respawn.CheckRespawns();
        grid.ResetHighlight();
		localMoves.Clear();
		networkMoves.Clear();
		data.SetDataConfirmations ();
		gameSequence.NextTurn();
        //crystal.Spawn();
        kingOfTheHill.addScore();
		data.SetResolve(false);
		running = false;
	}
}
