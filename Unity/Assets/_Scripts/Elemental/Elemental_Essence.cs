using UnityEngine;
using System.Collections;

public class Elemental_Essence : MonoBehaviour 
{

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
    const int maxAmount = 2;
    private GameObject[] essences = new GameObject[maxAmount];
    public GameObject essence;
    public int essenceAmount;
    public int testV = 0;
    public GameObject essenceHolder;
    private Elemental_Movement movement;
	private Elemental_Properties properties;
    private Grid grid;
    private bool onScoreHex;
	// Use this for initializatio
    //create the esscense so they are ready for use.
	void Start () 
    {
        onScoreHex = false; ;
        movement = GetComponentInChildren<AnimationStates>().movement;
        grid = GameObject.Find("GridCreator").GetComponent<Grid>(); 
		properties = GetComponent<Elemental_Properties>();
        for (int i = 0; i < maxAmount; i++)
        {
            GameObject ess = (GameObject)Object.Instantiate(essence);
            ess.transform.parent = essenceHolder.transform;
            essences[i] = ess;         
        }
        UpdateEssence();
	
	}

    //set the right amount of essence active and set them to the right position.
    void UpdateEssence()
    {
        for (int i = 0; i < maxAmount; i++)
        {
            essences[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < essenceAmount; i++)
        {
            essences[i].gameObject.SetActive(true);
            if (essenceAmount % 2 == 1)
            {
                essences[i].transform.localPosition = new Vector3(0, -0.8f, 0);
            }
            else
            {
                essences[i].transform.localPosition = new Vector3(-essenceAmount / 4.0f + i * 1, -0.8f, 0);
            }
		}
    }
    //change the amount of esscene in the unit, returns true if something changed.
    public bool ModifyEssence(int value)
    {
        int oldV = essenceAmount;
        if (value != 0)
        {
            essenceAmount += value;
            essenceAmount = Mathf.Clamp(essenceAmount, 0, 2);
            UpdateEssence();
        }
        if (oldV == essenceAmount)
        {
            return false;
        }
        if (oldV < essenceAmount)
        {

        }

        return true;
    }
 
    //check if the unit is scoring this round.
    public void CheckForScore()
    {
        bool onhexnow = false;
        Vector2 pos = movement.GetCurrentPos();
        for(int i = 0; i < grid.scorePositions.Length;i++)
        {
            if (grid.scorePositions[i] == pos)
            {
                if (onScoreHex == true)
                {
                  
                    if (ModifyEssence(-1) == true)
                    {                 
						if(properties.isMine)
						{
							Hierarchy.GetComponentWithTag<PlayerDataHandler>("PlayerDataHandler").AddPoints(1);
							Hierarchy.GetComponentWithTag<PhotonView>("NetworkPlayerDataHandler").RPC("AddPoints", PhotonTargets.Others, 1);
						}
                    }
                }
                onhexnow = true;
                onScoreHex = true;
            }
        }
        if (onhexnow == false)
            onScoreHex = false;

         //TODO:move this to a more logical place
        for (int i = 0; i < grid.kingPositions.Length; i++)
        {
            if (grid.kingPositions[i] == pos)
            {
                if (properties.isMine)
                {
                    kingOfTheHill.addMine(1);
                }
                else
                {
                    kingOfTheHill.addEnemy(1);
                }
            }
        }
    }
}
