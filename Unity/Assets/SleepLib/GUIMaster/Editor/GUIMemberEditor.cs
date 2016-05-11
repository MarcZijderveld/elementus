using UnityEngine;
using UnityEditor;
 
[CustomEditor(typeof(GUIMember))]
public class GUIMemberEditor : Editor
{
	private string oldIdentifier;
	
	public override void OnInspectorGUI()
	{
	    GUIMember t = (GUIMember)target;
	     
	 	oldIdentifier = t.identifier;
		
		DrawDefaultInspector ();
	
	    if(GUI.changed && oldIdentifier != t.identifier && !string.IsNullOrEmpty(oldIdentifier) && !string.IsNullOrEmpty(t.identifier))
	    {
	    	t.ChangeKey(oldIdentifier, t.identifier);
//			Debug.Log("changekey!");
	    }
		if(GUI.changed)
		{
			t.UpdateToMaster();	
//			Debug.Log("Update!");
		}
	}
}