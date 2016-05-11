using UnityEngine;
using System.Collections;

public class DeleteParticle : MonoBehaviour {
    ParticleSystem ps;
	// Use this for initialization
	void Start () {
        ps = gameObject.GetComponent<ParticleSystem>() ;
	}
	
	// Update is called once per frame
	void Update () {
	    if(ps)
             if(!ps.IsAlive())
                Destroy(gameObject);

	}
}
