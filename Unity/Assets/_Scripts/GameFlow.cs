	using UnityEngine;
using System.Collections;

public class GameFlow : MonoBehaviour 
{
	public void ToggleGameOver()
	{
		Hierarchy.GetComponentWithTag<GameOverScreen>("GameOverScreen").Activate();
	}
	
	public void ToggleWin()
	{
		Hierarchy.GetComponentWithTag<GameWinScreen>("GameWinScreen").Activate();
	}
}
