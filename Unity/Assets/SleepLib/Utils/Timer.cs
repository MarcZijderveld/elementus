using UnityEngine;
using System.Collections;

public struct Timer
{
	public readonly float	startTime,
							duration;
	public readonly bool	useRealTime;
	private float			durationModifier;	
	public Timer(float dur) : this(dur, false)
	{
	}
			
	public Timer(float dur, bool useRealTimeSinceStartup)
	{
		useRealTime			= useRealTimeSinceStartup;
		startTime			= useRealTime ? Time.realtimeSinceStartup : Time.time;
		duration			= dur;
		durationModifier	= 0f;
	}
	
	private float currentTime
	{
		get
		{
			return useRealTime ? Time.realtimeSinceStartup : Time.time; 
		}
	}
	
	public float time
	{
		get
		{
			return currentTime - startTime;
		}
	}
	
	public float remaining
	{
		get
		{
			return (duration + durationModifier) - time;
		}
	}
	
	public float progress
	{
		get
		{
			return Mathf.Clamp01((duration + durationModifier) == 0 ? 1f : time / (duration + durationModifier));
		}
	}
	
	public bool expired
	{
		get
		{
			return currentTime >= startTime + (duration + durationModifier);
		}
	}
	
	public void SubtractDuration(float time)
	{
		durationModifier -= time;
	}
	public void AddDuration(float time)
	{
		durationModifier += time;
	}
	
	public Coroutine Wait() 
	{
		return Coroutines.Run(WaitImpl());
	}
	
	private IEnumerator WaitImpl()
	{
		while (!expired)
			yield return null;
	}

	public static implicit operator bool(Timer m) 
	{
		return !m.expired;
	}
}
