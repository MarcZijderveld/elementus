using UnityEngine;
using System.Collections;

public class Coroutines : MonoBehaviour
{
	private static	Coroutines 	_instance	= null; 
	private static	Coroutines 	instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = (Coroutines) FindObjectOfType(typeof(Coroutines));
				if (_instance == null)
				{
					GameObject go = new GameObject("Coroutine Runner");
					DontDestroyOnLoad(go);
					_instance = (Coroutines) go.AddComponent(typeof(Coroutines));
				}
			}
			return _instance;
		}		
	} 
	
	void OnDisable()
	{
		StopAllCoroutines();
	}
	
	// start a coroutine. The return value can be yielded itself.
	public static Coroutine Run(IEnumerator function)
	{
		return instance.StartCoroutine(function);
	}
}