using UnityEngine;
using System.Collections;

public class FontProperties : MonoBehaviour 
{
	private int sourceFontSize = 0;
	
	public int SetSourceFontSize(int fontSize)
	{
		return sourceFontSize = fontSize;
	}
}
