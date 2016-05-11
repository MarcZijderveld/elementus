using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Camera))]
public class InvertCamera : MonoBehaviour 
{
	public bool invertCamera = false;
	
	public float culling = -1;
	
	private void Start()
	{
		enabled = invertCamera;
	}

	private void OnPreCull () 
	{
		camera.ResetWorldToCameraMatrix ();
		camera.ResetProjectionMatrix ();
		camera.projectionMatrix = camera.projectionMatrix * Matrix4x4.Scale(new Vector3 (-1, 1, 1));
	}
	 
	private void OnPreRender () 
	{
		GL.SetRevertBackfacing (true);
	}
	 
	private void OnPostRender () 
	{
		GL.SetRevertBackfacing (false);
	}
	
	public void InvertPlayer1()
	{
		//culling *= -1;
		invertCamera = !invertCamera;
		enabled = invertCamera;
	}
	
	public void InvertPlayer2()
	{
		//culling *= -1;
		enabled = !invertCamera;
		invertCamera = !invertCamera;
		enabled = invertCamera;
		enabled = !invertCamera;
	}
}
