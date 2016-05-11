using UnityEngine;
using System.Collections;

public class KingOfTheHill : MonoBehaviour {
    int myUnitsCount, otherUnitsCount;
	// Use this for initialization
	void Start () {
        reset();
	}

    void reset()
    {
        myUnitsCount = 0;
        otherUnitsCount = 0;
    }
    public void addMine(int count)
    {
        myUnitsCount += count;
    }

    public void addEnemy(int count)
    {

        otherUnitsCount += count;
    }

    public void addScore()
    {
        Debug.Log("myUnitsCount:" + myUnitsCount);
        Debug.Log("otherUnitsCount:" + otherUnitsCount);
        if (myUnitsCount < otherUnitsCount)
        {
            Hierarchy.GetComponentWithTag<PlayerDataHandler>("PlayerDataHandler").AddPoints(-1);
            Hierarchy.GetComponentWithTag<PhotonView>("NetworkPlayerDataHandler").RPC("AddPoints", PhotonTargets.Others, -1);
        }
        reset();
    }
}
