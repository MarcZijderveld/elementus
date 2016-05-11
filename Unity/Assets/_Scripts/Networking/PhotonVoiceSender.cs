using UnityEngine;
using System.Collections;

using MoPhoGames.USpeak.Interface;

public class PhotonVoiceSender : Photon.MonoBehaviour, ISpeechDataHandler		
{
	USpeaker spk;
	
	void Awake()
	{
		spk = USpeaker.Get(this);
		
		if(photonView.isMine)
		{
			spk.SpeakerMode = SpeakerMode.Local;
		}
		else 
			spk.SpeakerMode = SpeakerMode.Remote;
		
	}
	
	public void USpeakOnSerializeAudio(byte[] data)
	{
		photonView.RPC("Vc", PhotonTargets.All, data);
	}
	
	public void USpeakInitializeSettings(int data)
	{
		photonView.RPC("Init", PhotonTargets.All, data);
	}
	
	[RPC]
	void Init(int data)
	{
		spk.InitializeSettings(data);
	}
	
	[RPC]
	void Vc(byte[] data)
	{
		spk.ReceiveAudio(data);
	}
}
