using UnityEngine;
using System.Collections;

public class AnimationController : MonoBehaviour 
{	
	public AnimationStates animationStates { get; private set; }

	private void Start()
	{
		animationStates = GetComponent<AnimationStates>();
	}

	// Update is called once per frame
	void Update () 
	{
		if(animationStates.currentState == AnimationStates.AnimationStage.idle)
		{
			while(IsAnimating())
				return;

			animationStates.animationRoot.animation.Play("Idle");
		}
	}

	public void StartAnimation(string animation, AnimationStates.AnimationStage state)
	{
		animationStates.animationRoot.animation.Play(animation);
		animationStates.SetAnimationState(state);
	}

	public void SetAnimationState(AnimationStates.AnimationStage state)
	{
		animationStates.SetAnimationState (state);
	}

	public void DisableObject()
	{
		//animationStates.animationRoot.SetActive(false);
		foreach (Renderer ren in animationStates.animationRoot.GetComponentsInChildren<Renderer>()) 
		{
			ren.enabled = false;
		}
	}

	public void EnableObject()
	{
		foreach (Renderer ren in animationStates.animationRoot.GetComponentsInChildren<Renderer>()) 
		{
			ren.enabled = true;
		}
	}

	public bool IsAnimating()
	{
		return animationStates.animationRoot.animation.isPlaying;
	}
}
