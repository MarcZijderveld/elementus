using UnityEngine;
using System.Collections;

public class ForceRotation : MonoBehaviour 
{
	public Vector3 rotation = Vector3.zero;
	// Update is called once per frame
	void Update () 
	{
		transform.rotation = Quaternion.Euler(rotation);
	}
}
