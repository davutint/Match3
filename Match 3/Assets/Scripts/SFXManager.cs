using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
	public static SFXManager Instance;

	
	
	private void Awake()
	{
		Instance = this;
	}
	

	public AudioSource levelMusic;

	
	public void AnaSesCal()
	{
		levelMusic.Play();
	}
	public void AnaSesDurdur()
	{
		levelMusic.Stop();
	}
	
	/*public void PlayGemBreak()
	{
		gemSound.Stop();

		gemSound.pitch = Random.Range(.8f, 1.2f);

		gemSound.Play();
	}*/

	
}
