using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Hierarchy 
{
	public static T GetComponentWithTag<T>() where T : Component
	{
		return GetComponentWithTag<T>(typeof(T).Name);
	}
	
	public static T GetComponentWithTag<T>(string tag) where T : Component
	{
		T ret = null;
		GameObject[] gameObj	= GameObject.FindGameObjectsWithTag(tag);
		foreach (GameObject go in gameObj)
		{
			T t = go.GetComponentInChildren<T>();
			if (t != null)
			{
				ret = t;
				break;
			}
		}
		return ret;
	}
	
	public static List<T> GetComponentsWithTag<T>(string tag) where T : Component
	{
		List<T>			ret		= new List<T>();
		GameObject[]	gameObj	= GameObject.FindGameObjectsWithTag(tag);
		foreach (GameObject go in gameObj)
			ret.AddRange(go.GetComponentsInChildren<T>());
		return ret;
	}
	
	public static T GetComponentInParents<T>(Transform t) where T : Component
	{
		if (t == null)
			return null;
		T comp = t.GetComponent<T>();
		if (comp != null)
			return comp;
		else 
			return GetComponentInParents<T>(t.parent);
	}
		
	public static void SafeDestroy(GameObject go)
	{
		go.BroadcastMessage("OnSafeDestroy", null, SendMessageOptions.DontRequireReceiver);
		GameObject.Destroy(go);
	}
	
	public static GameObject FindObjectWithSubstring(string substring, GameObject findIn)
	{
		if (findIn.name.Contains(substring)) return findIn;
		foreach (Transform t in findIn.transform)
		{
			GameObject ret = FindObjectWithSubstring(substring, t.gameObject);
			if (ret != null)
				return ret; 
		}
		return null;
	}
	public static void FindObjectsWithSubstring(string substring, GameObject findIn, List<GameObject> ret)
	{
		if (findIn.name.Contains(substring)) ret.Add( findIn );
		foreach (Transform t in findIn.transform)
			FindObjectsWithSubstring(substring, t.gameObject, ret);
	}		
	// Look in obj and all its children for any GameObject with tag 'name'. It returns the first object found, or null 
	// if no such tag exists in the sub-tree  
	public static T GetChildComponentWithTag<T>(GameObject obj, string name) where T : Component
	{
		if (!obj)
			return null;
		if (obj.tag == name)
		{
			T ret = obj.GetComponent<T>();
			if (ret != null)
				return ret;
		}	
		foreach (Transform kid in obj.transform)
		{
			T ret = GetChildComponentWithTag<T>(kid.gameObject, name);
			if (ret != null)
				return ret;
		}
		return null;
	}
}