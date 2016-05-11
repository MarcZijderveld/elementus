using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OptionsMenu : GUIMemberComponent
{
	public List<GUIMemberComponent> otherComponents = new List<GUIMemberComponent>();
	public List<GUIMemberComponent> myComponents 	= new List<GUIMemberComponent>();
	
	public string headerElement  	= "NICKNAME",
				  AAElement		 	= "AA_GRID",
				  VSyncElement   	= "VSYNC_GRID",
				  shadowElement	 	= "SHADOW_GRID",
	 			  shadowQElement 	= "SHADOWQ_GRID",
				  qualityElement 	= "QUALITY_GRID",
				  masterVolElement  = "MASTERVOL",
				  fxVolElement 		= "FXVOL",
				  musicVolElement   = "MUSICVOL",
				  voiceVolElement   = "VOICEVOL";
	
	public Texture2D header;
	
	public GUIStyle selectionGridStyle;
	
	private GUIStyle sourceSelectionGridStyle;
	
	public GUIStyle sliderStyle,
					thumbStyle;
	
	public string[] AATexts 	= new string[4],
					VsyncText 	= new string[2],
					ShadowText 	= new string[3],
					qualityText = new string[4];
	
	public int guiDepth 		= 0;
	
	private int shadowToggle 	= 0;
	
	private int AAToggle 		= 0;
	
	private int shadowQToggle   = 0;
	
	private int overalQuality 	= 0;
	
	private void OnGUI()
	{
		GUI.depth = guiDepth;
		
		sourceSelectionGridStyle = GUIMaster.ResolutionGUIStyle(selectionGridStyle);
		
		if(interactable)
		{
			foreach(GUIMemberComponent gmc in otherComponents)
				gmc.SetInteratable(false);
			
			foreach(GUIMemberComponent gmc in myComponents)
			{
				gmc.SetInteratable(true);
				gmc.enabled = true;
			}
				
			GUI.DrawTexture(GUIMaster.GetElementRect(headerElement), header);
			
			overalQuality = GUI.SelectionGrid(GUIMaster.GetElementRect(qualityElement), overalQuality, qualityText, 4, sourceSelectionGridStyle);
			
			SetQuality();
			
			AAToggle = GUI.SelectionGrid(GUIMaster.GetElementRect(AAElement), AAToggle, AATexts, 4, sourceSelectionGridStyle);
			
			SetAA();
			
			QualitySettings.vSyncCount = GUI.SelectionGrid(GUIMaster.GetElementRect(VSyncElement), QualitySettings.vSyncCount, VsyncText, 2, sourceSelectionGridStyle);
			
			shadowToggle = GUI.SelectionGrid(GUIMaster.GetElementRect(shadowElement), shadowToggle, VsyncText, 2, sourceSelectionGridStyle);
			
			if(shadowToggle == 0)
				QualitySettings.shadowDistance = 0;
			else
				QualitySettings.shadowDistance = 100;
			
			shadowQToggle = GUI.SelectionGrid(GUIMaster.GetElementRect(shadowQElement), shadowQToggle, ShadowText, 3, sourceSelectionGridStyle);
			
			SetShadowQ();
			
			PlayerPrefs.SetFloat("masterVolume", GUI.HorizontalSlider(GUIMaster.GetElementRect(masterVolElement), PlayerPrefs.GetFloat("masterVolume"), 0, 1, sliderStyle, GUI.skin.horizontalSliderThumb));
			
			PlayerPrefs.SetFloat("musicVolume", GUI.HorizontalSlider(GUIMaster.GetElementRect(musicVolElement), PlayerPrefs.GetFloat("musicVolume"), 0, 1, sliderStyle, GUI.skin.horizontalSliderThumb));
			
			PlayerPrefs.SetFloat("fxVolume", GUI.HorizontalSlider(GUIMaster.GetElementRect(fxVolElement), PlayerPrefs.GetFloat("fxVolume"), 0, 1, sliderStyle, GUI.skin.horizontalSliderThumb));
			
			PlayerPrefs.SetFloat("voiceVolume", GUI.HorizontalSlider(GUIMaster.GetElementRect(voiceVolElement), PlayerPrefs.GetFloat("voiceVolume"), 0, 1, sliderStyle, GUI.skin.horizontalSliderThumb));
			
			Hierarchy.GetComponentWithTag<SoundSettings>("SoundManager").UpdateVolumes();
		}
		else
		{
			foreach(GUIMemberComponent gmc in otherComponents)
				gmc.ToggleInteractable();
			foreach(GUIMemberComponent gmc in myComponents)
			{
				gmc.SetInteratable(false);
				gmc.enabled = false;
			}
		}
	}
	
	private void SetQuality()
	{
		if(overalQuality == 0)
			QualitySettings.SetQualityLevel(0);
		if(overalQuality == 1)
			QualitySettings.SetQualityLevel(1);
		if(overalQuality == 2)
			QualitySettings.SetQualityLevel(3);
		if(overalQuality == 3)
			QualitySettings.SetQualityLevel(4);
	}
	
	private void SetAA()
	{
		if(AAToggle == 0)
			QualitySettings.antiAliasing = 0;
		if(AAToggle == 1)
			QualitySettings.antiAliasing = 2;
		if(AAToggle == 2)
			QualitySettings.antiAliasing = 4;
		if(AAToggle == 3)
			QualitySettings.antiAliasing = 8;
	}
	
	private void SetShadowQ()
	{
		if(shadowQToggle == 0)
			QualitySettings.shadowCascades = 1;
		if(shadowQToggle == 1)
			QualitySettings.shadowCascades = 2;
		if(shadowQToggle == 2)
			QualitySettings.shadowCascades = 4;
	}
}
