using UnityEngine;
using System.Collections;

public class Finish : MonoBehaviour 
{
	private Transform player1,
					  player2;
	
	public bool finish;
	
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;	
		Gizmos.DrawWireCube(transform.position, collider.bounds.size);
	}
	
	private void OnTriggerEnter(Collider other) 
	{
        //Debug.Log(other.gameObject.tag);
        if (other.gameObject.tag == "Player1") 
            player1 = other.transform;
        if (other.gameObject.tag == "Player2")
            player2 = other.transform;
    }
	
	private void Update()
	{
		if(player1 != null && player2 != null || finish)
		{
			Hierarchy.GetComponentWithTag<GameFlow>("GameFlow").ToggleWin();
		}
	}
}
