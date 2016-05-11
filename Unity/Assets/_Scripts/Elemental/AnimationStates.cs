using UnityEngine;
using System.Collections;

public class AnimationStates : MonoBehaviour 
{
	public enum AnimationStage
	{
		idle,
		combat,
		moving, 
		death,
	}

	public AnimationStage currentState {get; private set;}
	public Elemental_Movement movement {get; private set;}
	public Elemental_Animator animator {get; private set;}
	public Elemental_Death death { get; private set; }
	public Elemental_Attack attack { get; private set; }

	public GameObject animationRoot {get; private set;}

	private void Start()
	{
		currentState = AnimationStage.idle;
	}

	public void SetAnimationState(AnimationStage state)
	{
		currentState = state;
	}

	public void SetAnimationRoot(GameObject root)
	{
		animationRoot = root;
		movement = GetComponent<Elemental_Movement>();
		animator = animationRoot.GetComponent<Elemental_Animator>();
		death = animationRoot.GetComponent<Elemental_Death> ();
		attack = animationRoot.GetComponent<Elemental_Attack>();
	}
}
