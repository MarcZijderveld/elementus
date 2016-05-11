using UnityEngine;
using System.Collections;

public class PlayerData : MonoBehaviour 
{
    private SoundSettings _soundSettings = null;
    public SoundSettings soundSettings
    {
        get
        {
            if (_soundSettings == null)
            {
                _soundSettings = Hierarchy.GetComponentWithTag<SoundSettings>("SoundManager");
            }
            return _soundSettings;
        }
    } 

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
        soundSettings.Play("score");
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
