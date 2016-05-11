#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

[ExecuteInEditMode]	
public class GUIMasterEditor : MonoBehaviour 
{
	public GUIStyle box;

	private void Start()
	{
		//Simulating the first resolution change to get everything up and running.
		GUIMaster.OnResolutionChanged();	
		Debug.Log("moo");

	}

	private void OnGUI()
	{
		List<Transform> transforms = new List<Transform>(Selection.GetTransforms(SelectionMode.TopLevel | SelectionMode.OnlyUserModifiable));
		
		if(!Application.isPlaying)
		{
			foreach(KeyValuePair<string, GUIMember> r in GUIMaster.GUIMembers)
			{
				if(r.Value != null)
				{
					if(!r.Value.show)
						continue;
					
					if(r.Value.previewTexture != null)
					{
						GUIStyle style = new GUIStyle();
						style.normal.background = r.Value.previewTexture;
						GUI.Box(r.Value.GetScaledRect(), "", style);
					}
					
					if(transforms.Contains(r.Value.transform))
					{
						GUI.color = Color.red;	
						GUIMaster.OnResolutionChanged();
					}
					
					GUI.Box(r.Value.GetScaledRect(), "", box);
					
					GUI.color = Color.white;
				}
			}
		}	
		
		GetMainGameView().Repaint();
	}

	private EditorWindow GetMainGameView() 
	{
		System.Type T = System.Type.GetType("UnityEditor.GameView,UnityEditor");
		System.Reflection.MethodInfo GetMainGameView = T.GetMethod("GetMainGameView",System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
		System.Object Res = GetMainGameView.Invoke(null,null);
		return (EditorWindow)Res;
	}
}
#endif
