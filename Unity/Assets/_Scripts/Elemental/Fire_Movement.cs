using UnityEngine;
using System.Collections;

public class Fire_Movement : Elemental_Animator 
{    
	public GameObject particlePrefabIn,
					  particlePrefabOut;
	
	private GameObject spawned;
	
	public float inSpawn,
				 outSpawn;
	
	private void Start()
	{
		base.Start();
	}
	
	public override IEnumerator Move(MovementController.Move move)
	{
		//Debug.Log(move.id);
		Vector3 beginPos = transform.parent.parent.position;
		
		Transform trans = transform.parent.parent;
		
		trans.LookAt(grid.GetPosition((int)move.EndGridPosition.x, (int)move.EndGridPosition.y));
		trans.rotation = Quaternion.Euler(new Vector3(0, trans.eulerAngles.y, 0));
		
		animController.StartAnimation("Moving1", AnimationStates.AnimationStage.moving);
		
		while (animController.IsAnimating()) 
		{
			if(animation["Moving1"].time > inSpawn && animation["Moving1"].time < inSpawn + 0.1f)
			{
				if(spawned == null)
				{
					spawned = (GameObject)GameObject.Instantiate(particlePrefabIn, transform.position, particlePrefabIn.transform.rotation);
                    spawned.AddComponent<DeleteParticle>();
				}
			}
			
			yield return null;
		}
	
		spawned = null;
		
		animController.DisableObject();
		
		Timer t = new Timer(0.2f);
		
		while(t)
		{
			trans.position = Vector3.Lerp(beginPos, grid.GetPosition((int)move.EndGridPosition.x, (int)move.EndGridPosition.y) + new Vector3(0, 1, 0), t.progress);
			yield return null;
		}
		
		trans.position = grid.GetPosition((int)move.EndGridPosition.x, (int)move.EndGridPosition.y) + new Vector3(0, 1, 0);
		
		animController.animationStates.movement.SetGridPos(new Vector2(move.EndGridPosition.x, move.EndGridPosition.y));
		
		animController.EnableObject();
		
		animController.StartAnimation("Moving2", AnimationStates.AnimationStage.moving);
		
		while (animController.IsAnimating ()) 
		{
			if(animation["Moving2"].time > outSpawn && animation["Moving2"].time < outSpawn + 0.1f)
			{
				if(spawned == null)
				{
					spawned = (GameObject)GameObject.Instantiate(particlePrefabOut, transform.position, particlePrefabIn.transform.rotation);
                    spawned.AddComponent<DeleteParticle>();
				}
			}
			yield return null;
		}
		
		
		
		animController.SetAnimationState (AnimationStates.AnimationStage.idle);
		
		yield return null;	
	}
	
	public override IEnumerator MoveBack(MovementController.Move move)
	{
		Vector3 beginPos = transform.position;
		
		Transform trans = transform.parent.parent;
		
		trans.LookAt(grid.GetPosition((int)move.BeginGridPosition.x, (int)move.BeginGridPosition.y));
		trans.rotation = Quaternion.Euler(new Vector3(0, trans.eulerAngles.y, 0));
		
		animController.StartAnimation("Moving1", AnimationStates.AnimationStage.moving);
		
		while (animController.IsAnimating()) 
		{
			if(animation["Moving1"].time > inSpawn && animation["Moving1"].time < inSpawn + 0.1f)
			{
				if(spawned == null)
				{
					spawned = (GameObject)GameObject.Instantiate(particlePrefabIn, transform.position, particlePrefabIn.transform.rotation);	
					spawned.AddComponent<DeleteParticle>();
				}
			}
			
			yield return null;
		}
		
		DestroyImmediate ((Object)spawned, true);
		spawned = null;
		
		animController.DisableObject();
		
		Timer t = new Timer(0.5f);
		
		while(t)
		{
			trans.position = Vector3.Lerp(beginPos, grid.GetPosition((int)move.BeginGridPosition.x, (int)move.BeginGridPosition.y) + new Vector3(0, 1, 0), t.progress);
			yield return null;
		}
		
		trans.position = grid.GetPosition((int)move.BeginGridPosition.x, (int)move.BeginGridPosition.y) + new Vector3(0, 1, 0);
		
		animController.animationStates.movement.SetGridPos(new Vector2(move.BeginGridPosition.x, move.BeginGridPosition.y));
		
		animController.EnableObject();
		
		animController.StartAnimation("Moving2", AnimationStates.AnimationStage.moving);
		
		while (animController.IsAnimating ()) 
		{
			if(animation["Moving2"].time > outSpawn && animation["Moving2"].time < outSpawn + 0.1f)
			{
				if(spawned == null)
				{
					spawned = (GameObject)GameObject.Instantiate(particlePrefabOut, transform.position, particlePrefabIn.transform.rotation);	
					spawned.AddComponent<DeleteParticle>();
				}
			}
			yield return null;
		}
		
		//DestroyImmediate ((Object)spawned, true);
		
		animController.SetAnimationState (AnimationStates.AnimationStage.idle);
		
		trans.LookAt(grid.GetPosition((int)move.EndGridPosition.x, (int)move.EndGridPosition.y));
		trans.rotation = Quaternion.Euler(new Vector3(0, trans.eulerAngles.y, 0));
		
		yield return null;	
	}
}
