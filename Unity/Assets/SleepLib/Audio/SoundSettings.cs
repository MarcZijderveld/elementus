using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundSettings : MonoBehaviour 
{
	public AudioClip music;
	public AudioClip[] sounds;
    public Dictionary<string, AudioClip> soundDictonary = new Dictionary<string,AudioClip>();
	public bool inGame = false;
	
	private void Start()
	{
		PlayBGMusic();
        BuildDictonary();

		if(!inGame)
			DontDestroyOnLoad(gameObject);

		if(!PlayerPrefs.HasKey("masterVolume"))
			PlayerPrefs.SetFloat("masterVolume", 1);
		if(!PlayerPrefs.HasKey("fxVolume"))
			PlayerPrefs.SetFloat("fxVolume", 1);
		if(!PlayerPrefs.HasKey("musicVolume"))
			PlayerPrefs.SetFloat("musicVolume", 1);
		if(!PlayerPrefs.HasKey("voiceVolume"))
			PlayerPrefs.SetFloat("voiceVolume", 1);
	}

    void BuildDictonary()
    {
        foreach (AudioClip sound in sounds)
        {
            soundDictonary.Add(sound.name, sound);
        }
    }

	/*private void OnLevelWasLoaded(int level)
	{
		if(level == 0)
		{
			Destroy(this.gameObject);
		}
	}*/
	
	public void StopAll()
	{
		List<AudioSource> sources = new List<AudioSource>(GetComponentsInChildren<AudioSource>());
		
		foreach(AudioSource source in sources)
		{
			if(source != null)
			{
				source.enabled = false;
			}
		}
	}
	
	public void Play(AudioClip clip) 
	{
		StartCoroutine("PlaySound", clip);
	}

    public void Play(string clip)
    {
        if (soundDictonary.ContainsKey(clip))
        {
            StartCoroutine("PlaySound", soundDictonary[clip]);
            return;
        }
        Debug.LogWarning("no audioclip called: " + clip);        
    }
	
	public void PlayBGMusic() 
	{
		StartCoroutine("PlayLooped", music);
	}

    public IEnumerator Play(string clip,float time)
    {
        yield return new WaitForSeconds(time);
        Play(clip);
    }
		
	public void UpdateVolumes()
	{
		foreach(AudioSource source in Hierarchy.GetComponentsWithTag<AudioSource>("FX"))
		{
			if(PlayerPrefs.GetFloat ("fxVolume") > PlayerPrefs.GetFloat("masterVolume"))
				source.volume = PlayerPrefs.GetFloat ("masterVolume");
			else
				source.volume = PlayerPrefs.GetFloat ("fxVolume");
		}
		foreach(AudioSource source in Hierarchy.GetComponentsWithTag<AudioSource>("BGM"))
		{
			if(PlayerPrefs.GetFloat ("musicVolume") > PlayerPrefs.GetFloat("masterVolume"))
				source.volume = PlayerPrefs.GetFloat ("masterVolume");
			else
				source.volume = PlayerPrefs.GetFloat ("musicVolume");
		}
		
		foreach(USpeaker uspeaker in Hierarchy.GetComponentsWithTag<USpeaker>("VoiceHandler"))
		{
			uspeaker.SpeakerVolume = PlayerPrefs.GetFloat ("voiceVolume");
		}
	}
	
	private IEnumerator PlaySound(AudioClip clip)
	{
		GameObject source = new GameObject();
		source.name = clip.name;
		source.transform.parent = gameObject.transform;
		AudioSource audio = source.AddComponent<AudioSource>();
		audio.clip = clip;
		source.tag = "FX";	
		audio.volume = PlayerPrefs.GetFloat ("soundVolume");
		
		audio.Play();
		
		//UpdateVolumes();
		
		while(audio.isPlaying)
			yield return null;
		
		Destroy(source);
	}
		
	private IEnumerator PlayLooped(AudioClip clip)
	{
		GameObject source = new GameObject();
		source.transform.parent = gameObject.transform;
		source.name = clip.name + " - Source";
		AudioSource audio = source.AddComponent<AudioSource>();
		audio.clip = clip;
			
		audio.volume = PlayerPrefs.GetFloat ("musicVolume");
		audio.loop = true;
		source.tag = "BGM";
		audio.Play();
		
		//UpdateVolumes();
		
		while(audio.isPlaying)
			yield return null;
		
		Destroy(source);
	}
}
