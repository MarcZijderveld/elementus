	using UnityEngine;
using System.Collections;

public class Elemental_Attack : MonoBehaviour 
{
	public Grid grid 								{ get; private set; }
	
	public AnimationController animController 		{ get; private set; } 
	
	public MovementController momvementController 	{ get; private set; } 
	
	public void Start () 
	{
		momvementController = GameObject.Find("MovementController").GetComponent<MovementController>();
		grid = GameObject.Find("GridCreator").GetComponent<Grid>();
	}
	
	public virtual IEnumerator Attack(Transform target)
	{
		yield return null;
	}
	
	public void SetAnimController(AnimationController controller)
	{

		animController = controller;
	}
}
