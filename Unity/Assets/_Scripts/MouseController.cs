using UnityEngine;
using System.Collections;

public class MouseController : MonoBehaviour 
{
	public Vector3 	hitPoint 				{get; private set;}
	
	public Vector2	mousePos				{get; private set;}
	
	private void Update()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
				
		if (Physics.Raycast(ray, out hit) && hit.normal.y > 0.999)
		{
			hitPoint = hit.point;	
		}	
	}
	
	private void OnGUI()
	{
		mousePos				= Event.current.mousePosition;
	}
}
