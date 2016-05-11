using UnityEngine;
using System.Collections;

public class NetworkPlayerData : MonoBehaviour 
{
	private int ID 		= 0,
	points  = 9,
	moves   = 0;
	
	public void SetID(int value)
	{
		ID = value;
	}
	
	public int GetID()
	{
		return ID;
	}
	
	public void AddPoints(int value)
	{
		points += value;
	}
	
	public int GetPoints()
	{
		return points;
	}
	
	public void SetMoves(int value)
	{
		moves += value;
	}
	
	public int MovesLeft()
	{
		return moves;
	}
}
