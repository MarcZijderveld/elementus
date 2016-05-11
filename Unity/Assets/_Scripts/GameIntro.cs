using UnityEngine;
using System.Collections;

public class GameIntro : GUIMemberComponent 
{
	public Texture2D madeLogo,
					 nhtvLogo,
					 gameLogo;
	
	private Texture2D currentTexture;
	
	public string 	 elem;
	
	private Color  	 color = Color.white;
	
	public string 	 sceneToLoad;
	
	void Start () 
	{
		PlayerPrefs.SetFloat ("soundVolume", 0.5f);
		PlayerPrefs.SetFloat ("musicVolume", 0.5f);
		StartCoroutine("Intro");
	}
	
	void OnGUI()
	{
		GUI.color = color;
		GUI.DrawTexture(GUIMaster.GetElementRect(elem), currentTexture);
	}
	
	public IEnumerator Intro()
	{
		currentTexture = nhtvLogo;
		color.a = 0;
		
		Timer time = new Timer(2);
		while(time)
		{
			color.a = time.progress;
			yield return null;
		}
		
		currentTexture = madeLogo;
		color.a = 0;
		
		Timer time2 = new Timer(2);
		while(time2)
		{
			color.a = time2.progress;
			yield return null;
		}
		
		currentTexture = gameLogo;
		color.a = 0;
		
		Timer time3 = new Timer(2);
		while(time3)
		{
			color.a = time3.progress;
			yield return null;
		}
		
		Application.LoadLevel(sceneToLoad);
	}
}
