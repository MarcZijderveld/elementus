using UnityEngine;
using System.Collections;

public class Elemental_kingOfTheHill : MonoBehaviour {
    private Grid grid;
    private Elemental_Movement movement;
    private Elemental_Properties properties;
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
	// Use this for initialization
	void Start () {
        grid = GameObject.Find("GridCreator").GetComponent<Grid>();
        properties = GetComponent<Elemental_Properties>();
        movement = GetComponentInChildren<AnimationStates>().movement;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void CheckForScore()
    {
        Vector2 pos = movement.GetCurrentPos();
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
