using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Elemental_Ghosting : MonoBehaviour
{
    float height =-0.6f;
    public GameObject fire, water, grass;
    class Ghost
    {

        public GameObject ghostTrans;
        public Vector3 BeginPosition;
        public Vector3 EndPosition;
        public float timer = 0;
    }

    private Dictionary<int,Ghost> ghosts = new Dictionary<int,Ghost>();
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        foreach (KeyValuePair<int, Ghost> ghost in ghosts)
        {
            Ghost valus = ghost.Value;
            valus.ghostTrans.transform.position = Vector3.Lerp(valus.BeginPosition, valus.EndPosition, valus.timer);
            valus.timer += 0.005f;
            if (valus.timer >= 1)
                valus.timer = 0;

        }
	}

    public void Reset()
    {
        foreach (KeyValuePair<int, Ghost> ghost in ghosts)
        {
            Ghost valus = ghost.Value;
            Destroy(ghost.Value.ghostTrans);
        }
        ghosts.Clear();
    }
    public void changeElement(int id, Element_Enum.Types element)
    {
        if (ghosts.ContainsKey(id))
        {
            Destroy(ghosts[id].ghostTrans);
            ghosts[id].ghostTrans = makeObject(element);
            ghosts[id].ghostTrans.transform.position = ghosts[id].BeginPosition + new Vector3(0, height, 0);
            ghosts[id].ghostTrans.transform.LookAt(ghosts[id].EndPosition + new Vector3(0, height, 0));
        }
    }
    GameObject makeObject(Element_Enum.Types element)
    {
        GameObject ghosts = null;
        if (element == Element_Enum.Types.fire)
            ghosts = Instantiate(fire) as GameObject;
        if (element == Element_Enum.Types.water)
            ghosts = Instantiate(water) as GameObject;
        if (element == Element_Enum.Types.grass)
            ghosts = Instantiate(grass) as GameObject;
        return ghosts;
    }

    public void AddGhost(int id,Vector3 beginPostion,Vector3 endPosition,Element_Enum.Types element)
    {
        if(ghosts.ContainsKey(id))
        {
            Destroy(ghosts[id].ghostTrans);
            ghosts.Remove(id);
        }

        Ghost ghostelement = new Ghost();

        ghostelement.ghostTrans = makeObject(element);

        ghostelement.ghostTrans.transform.position = beginPostion + new Vector3(0, height, 0);
        ghostelement.ghostTrans.transform.LookAt(endPosition + new Vector3(0, height, 0));
        ghostelement.BeginPosition = beginPostion + new Vector3(0, height, 0);
        ghostelement.EndPosition = endPosition + new Vector3(0, height, 0);

        ghosts.Add(id,ghostelement);
	}

	public void RemoveGhost(int id)
	{
		if(ghosts.ContainsKey(id))
		{
			Destroy(ghosts[id].ghostTrans);
			ghosts.Remove(id);
		}
	}
}
