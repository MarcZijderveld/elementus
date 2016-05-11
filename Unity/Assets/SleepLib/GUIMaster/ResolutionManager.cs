using UnityEngine;
using System.Collections;
/// <summary>
/// Resolution manager.
/// 
/// Default value for target resolution can be found here.
/// 
/// </summary>
[ExecuteInEditMode]
public class ResolutionManager : MonoBehaviour 
{
	private float 			lastScreenWidth = 0f;
	
	//Default resolution on which the program is targeted to be run.
	private static float	defaultResolutionWidth = 1920,
							defaultResolutionHeight = 1080;
	//--------------------------------------------------------------
	
	public enum scaleMode
	{
		keepPixelSize,
		scaleWidth,
		scaleHeight,
		scaleWithResolution
	}
	
	void Start()
	{
		lastScreenWidth = Screen.width;
	}
	
	void Update()
	{
		if(lastScreenWidth != Screen.width )
		{
			lastScreenWidth = Screen.width;
			GUIMaster.OnResolutionChanged();
		}
	}
	
	public static Vector2 GetDefaultResolution()
	{
		return new Vector2(defaultResolutionWidth, defaultResolutionHeight);
	}
}
