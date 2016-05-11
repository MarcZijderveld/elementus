using UnityEngine;
using System.Collections;

public class Water_Attack : Elemental_Attack
{
	public GameObject projectile;

	public float trigger1,
				 trigger2,
				 trigger3,
				 trigger4,
				 trigger5,
				 travelTime;

	public Transform  spawnPoint1,
					  spawnPoint2,
					  spawnPoint3,
					  spawnPoint4,
					  spawnPoint5;

	private GameObject spawned1,
					   spawned2,
					   spawned3,
					   spawned4,
					   spawned5;

	// Use this for initialization
    private	void Start () 
	{
		base.Start();
	}
	
	public override IEnumerator Attack(Transform target)
	{
		Transform trans = transform.parent.parent;
		
		trans.LookAt(target);
		trans.rotation = Quaternion.Euler(new Vector3(0, trans.eulerAngles.y, 0));
		
		animController.StartAnimation("Attacking", AnimationStates.AnimationStage.combat);
		
		while (animController.IsAnimating()) 
		{
			//Debug.Log(animation["Attacking"].time);
			if(animation["Attacking"].time > trigger1 && animation["Attacking"].time < trigger1 + 0.1f)
			{
				if(spawned1 == null)
				{
					spawned1 = (GameObject)GameObject.Instantiate(projectile, spawnPoint1.position, projectile.transform.rotation);
					spawned1.AddComponent<DeleteParticle>();
				}

				Timer t = new Timer(travelTime);
				
				while(t)
				{
					spawned1.transform.position = Vector3.Lerp(spawnPoint1.position, target.position + new Vector3(0, 0f, 0), t.progress);
					yield return null;
				}
			}

			if(animation["Attacking"].time > trigger2 && animation["Attacking"].time < trigger2 + 0.1f)
			{
				if(spawned2 == null)
				{
					spawned2 = (GameObject)GameObject.Instantiate(projectile, spawnPoint2.position, projectile.transform.rotation);
					spawned2.AddComponent<DeleteParticle>();
				}
				
				Timer t = new Timer(travelTime);
				
				while(t)
				{
					spawned2.transform.position = Vector3.Lerp(spawnPoint2.position, target.position + new Vector3(0, 0f, 0), t.progress);
					yield return null;
				}
			}

			if(animation["Attacking"].time > trigger3 && animation["Attacking"].time < trigger3 + 0.1f)
			{
				if(spawned3 == null)
				{
					spawned3 = (GameObject)GameObject.Instantiate(projectile, spawnPoint3.position, projectile.transform.rotation);
					spawned3.AddComponent<DeleteParticle>();
				}
				
				Timer t = new Timer(travelTime);
				
				while(t)
				{
					spawned3.transform.position = Vector3.Lerp(spawnPoint3.position, target.position + new Vector3(0, 0f, 0), t.progress);
					yield return null;
				}
			}

			if(animation["Attacking"].time > trigger4 && animation["Attacking"].time < trigger4 + 0.1f)
			{
				if(spawned4 == null)
				{
					spawned4 = (GameObject)GameObject.Instantiate(projectile, spawnPoint4.position, projectile.transform.rotation);
					spawned4.AddComponent<DeleteParticle>();
				}
				
				Timer t = new Timer(travelTime);
				
				while(t)
				{
					spawned4.transform.position = Vector3.Lerp(spawnPoint4.position, target.position + new Vector3(0, 0, 0), t.progress);
					yield return null;
				}
			}

			if(animation["Attacking"].time > trigger5 && animation["Attacking"].time < trigger5 + 0.1f)
			{
				if(spawned5 == null)
				{
					spawned5 = (GameObject)GameObject.Instantiate(projectile, spawnPoint5.position, projectile.transform.rotation);
					spawned5.AddComponent<DeleteParticle>();
				}
				
				Timer t = new Timer(travelTime);
				
				while(t)
				{
					spawned5.transform.position = Vector3.Lerp(spawnPoint5.position, target.position + new Vector3(0, 0f, 0), t.progress);
					yield return null;
				}
			}
			
			yield return null;
		}

		yield return null;
	}
}
