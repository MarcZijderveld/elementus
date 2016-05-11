using UnityEngine;
using System.Collections;

public class ResultMessages : GUIMemberComponent
{
	public Texture 	    winStyle,
						drawStyle,
						loseStyle;




	public string 		textElement;

	private bool		win,
						lose,
						draw;

	public enum TextType 
	{
		win,
		lose,
		draw
	}

	private void OnGUI()
	{
		

		if(win)
		{
            GUI.DrawTexture(GUIMaster.GetElementRect(textElement), winStyle);
		}
		if(lose)
		{
            GUI.DrawTexture(GUIMaster.GetElementRect(textElement), loseStyle);
		}
		if(draw)
		{
			GUI.DrawTexture(GUIMaster.GetElementRect(textElement), drawStyle );
		}
	}

	public IEnumerator StartText(TextType textType)
	{
		switch(textType)
		{
			case TextType.win:
			win = true;
			break;
			case TextType.lose:
			lose = true;
			break;
			case TextType.draw:
			draw = true;
			break;
		}

		yield return new WaitForSeconds(0.5f);

		switch(textType)
		{
		case TextType.win:
			win = false;
			break;
		case TextType.lose:
			lose = false;
			break;
		case TextType.draw:
			draw = false;
			break;
		}
	}
}
