using UnityEngine;
using System.Collections;

public class PhotonGUIMemberComponent : Photon.MonoBehaviour
{
	/*private	GUIMaster 	_GUIMaster	= null; 
	public	GUIMaster 	GUIMaster
	{
		get
		{
			if (_GUIMaster == null)
			{
				_GUIMaster = Hierarchy.GetComponentWithTag<GUIMaster>("GUIMaster");
			}
			return _GUIMaster;
		}		
	} */
	
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
