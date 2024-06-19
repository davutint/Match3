using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
	
	[Header("ELEMENTS")]

	private const string ArkaPlanSes = "Sound";
	[SerializeField]private Sprite _openSoundSprite;
	[SerializeField]private Sprite _closeSoundSprite;
	[SerializeField]private Button _soundButton;
	
	[SerializeField]AudioSource menuSound;
	private void Awake()
	{
		
	}
	private void Start()
	{
		GetData();
	}
	
	public void Sound()
	{
		
		
		if (PlayerPrefs.GetInt("Sound",1)==1)
		{
			PlayerPrefs.SetInt("Sound",0);
			menuSound.Stop();
		}
		else
		{
			PlayerPrefs.SetInt("Sound",1);
			menuSound.Play();
		}
		GetData();
		
	}

	
	private void GetData()
	{
		
		if (PlayerPrefs.GetInt("Sound",1)==1)
		{
			_soundButton.image.sprite=_openSoundSprite;
			menuSound.Play();
		}
		else
		{
			_soundButton.image.sprite=_closeSoundSprite;
			menuSound.Stop();
		}
		
	}
}
