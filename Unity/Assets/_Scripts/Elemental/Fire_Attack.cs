using UnityEngine;
using System.Collections;

public class Fire_Attack : Elemental_Attack
{
	public GameObject projectile;

	public float trigger1,
				 trigger2,
				 trigger3,
				 travelTime;

	public Transform spawnPoint1,
					  spawnPoint2,
					  spawnPoint3;

	private GameObject spawned1,
					   spawned2,
					   spawned3;

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
					//spawned.AddComponent<DeleteParticle>();
				}

				Timer t = new Timer(travelTime);
				
				while(t)
				{
					spawned1.transform.position = Vector3.Lerp(spawnPoint1.position, target.position - new Vector3(0, 0.7f, 0), t.progress);
					yield return null;
				}
			}

			if(animation["Attacking"].time > trigger2 && animation["Attacking"].time < trigger2 + 0.1f)
			{
				if(spawned2 == null)
				{
					spawned2 = (GameObject)GameObject.Instantiate(projectile, spawnPoint2.position, projectile.transform.rotation);
					//spawned.AddComponent<DeleteParticle>();
				}
				
				Timer t = new Timer(travelTime);
				
				while(t)
				{
					spawned2.transform.position = Vector3.Lerp(spawnPoint2.position, target.position - new Vector3(0, 0.7f, 0), t.progress);
					yield return null;
				}
			}

			if(animation["Attacking"].time > trigger3 && animation["Attacking"].time < trigger3 + 0.1f)
			{
				if(spawned3 == null)
				{
					spawned3 = (GameObject)GameObject.Instantiate(projectile, spawnPoint3.position, projectile.transform.rotation);
					//spawned.AddComponent<DeleteParticle>();
				}
				
				Timer t = new Timer(travelTime);
				
				while(t)
				{
					spawned3.transform.position = Vector3.Lerp(spawnPoint3.position, target.position - new Vector3(0, 0.7f, 0), t.progress);
					yield return null;
				}
			}
			
			yield return null;
		}

		yield return null;
	}
}
