using UnityEngine;
using System.Collections;

public class Grass_Attack : Elemental_Attack
{
	public GameObject projectile;

	public float trigger1;

	private GameObject spawned1;

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
			if(animation["Attacking"].time > trigger1 && animation["Attacking"].time < trigger1 + 0.1f)
			{
				if(spawned1 == null)
				{
					spawned1 = (GameObject)GameObject.Instantiate(projectile, target.position - new Vector3(0,1,0), projectile.transform.rotation);
					spawned1.GetComponentInChildren<ParticleSystem>().gameObject.AddComponent<DeleteParticle>();
				}
			}
					
			yield return null;
		}

		spawned1 = null;

		yield return null;
	}
}
