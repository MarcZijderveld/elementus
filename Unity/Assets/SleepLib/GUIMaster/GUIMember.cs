using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class GUIMember : MonoBehaviour 
{
	public string 	identifier = "NEW";
	public Rect 	rect = new Rect(100,100,100,100);
	public bool		show = true;
	
	private Rect 	scaledRect;
	
	public ResolutionManager.scaleMode scaleMode = ResolutionManager.scaleMode.scaleWithResolution;
	
	public Texture2D	 previewTexture = null;
	
	private void Awake()
	{
		GUIMaster.AddMember(identifier, this);
	}
	
	#if UNITY_EDITOR
	private void Update()
	{
		if(!Application.isPlaying)
			GUIMaster.UpdateMember(identifier, this);
	}
	#endif
	
	private void OnDestroy()
	{
		GUIMaster.RemoveMember(identifier);
	}
	
	public void ChangeKey(string oldKey, string newKey)
	{
		GUIMaster.ChangeIdentifier(oldKey, newKey);
	}
	
	public void UpdateToMaster()
	{
		this.gameObject.name = "GUIMEMBER(" + identifier + ")";
		GUIMaster.UpdateMember(identifier, this);
	}
	
	public void SetScaling(Rect scaleRect)
	{
		scaledRect = scaleRect;
	}
	
	public Rect GetScaledRect()
	{
		return scaledRect;
	}
}
