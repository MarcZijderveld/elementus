using UnityEngine;
using System.Collections;

public class Elemental_Death : Elemental_Animator 
{
	public GameObject particlePrefab;

	public float inSpawn;

	private GameObject spawned;

	private void Start()
	{
		base.Start ();
	}

	public override IEnumerator Death ()
	{
        if (transform.parent.parent.GetComponent<Elemental_Properties>().isMine)
        {
            Hierarchy.GetComponentWithTag<PlayerDataHandler>("PlayerDataHandler").AddPoints(-1);
            Hierarchy.GetComponentWithTag<PhotonView>("NetworkPlayerDataHandler").RPC("AddPoints", PhotonTargets.Others, -1);
        }
        momvementController.RemoveUnit (transform.parent.parent.GetComponent<Elemental_Properties>().id);

		animController.StartAnimation("Death", AnimationStates.AnimationStage.death);

		while (animController.IsAnimating()) 
		{
			if(animation["Death"].time > inSpawn && animation["Death"].time < inSpawn + 0.1f)
			{
				if(spawned == null)
				{
					spawned = (GameObject)GameObject.Instantiate(particlePrefab, transform.position, particlePrefab.transform.rotation);
					spawned.AddComponent<DeleteParticle>();
				}
			}
			
			yield return null;
		}
        animController.StartAnimation("Death", AnimationStates.AnimationStage.idle);
		this.transform.parent.parent.gameObject.SetActive (false);
        
		yield return null;
	}
}
