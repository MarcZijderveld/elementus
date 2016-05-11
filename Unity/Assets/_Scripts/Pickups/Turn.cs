using UnityEngine;
using System.Collections;

public class Turn : MonoBehaviour {
    float turn ;
	// Use this for initialization
	void Start () {
	turn=1;
	}
	
	// Update is called once per frame
    
	void Update () {
        
        transform.Rotate(new Vector3(0, turn, 0));
	}
}
