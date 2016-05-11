using UnityEngine;
using System.Collections;

public class GUIMemberComponent : MonoBehaviour
{
	public bool interactable {get; private set;}

	private void Start()
	{
		interactable = true;
	}
	
	public void ToggleInteractable()
	{
		interactable = !interactable;
	}	
	
	public void SetInteratable(bool interact)
	{
		interactable = interact;
	}
}
