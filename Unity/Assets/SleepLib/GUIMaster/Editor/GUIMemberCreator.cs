using UnityEngine;
using UnityEditor;
using System.Collections;

public class GUIMemberCreator : ScriptableObject {

	[MenuItem ("GUIMaster/Add GUIMember")]
    public static void Create()
    {
        Transform[] transforms = Selection.GetTransforms(SelectionMode.TopLevel | SelectionMode.OnlyUserModifiable);
 
		if(transforms.Length > 0)
		{
	       	foreach(Transform transform in transforms)
	        {
	            GameObject newChild = new GameObject("GUIMEMBER(NEW)");
	            newChild.transform.parent = transform;
				newChild.transform.localPosition = Vector3.zero;
				newChild.AddComponent<GUIMember>();
	        }
		}
		else
		{
			GameObject newChild = new GameObject("GUIMEMBER(NEW)");
			newChild.transform.localPosition = Vector3.zero;
			newChild.AddComponent<GUIMember>();
		}
    }
}
