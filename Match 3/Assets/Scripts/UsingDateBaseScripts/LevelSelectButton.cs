using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectButton : MonoBehaviour
{
	[Header("SCORES REQUIRED TO UNLOCK")]
	[SerializeField]private int _level2UnlockScore;
	[SerializeField]private TextMeshProUGUI _level2UnlockScoreText;
	[SerializeField]private Button[] _levelButtons;
	int highScoreForUnlock;
	
	void Start()
	{
		if (!PlayerPrefs.HasKey("HighScore"))
		{
			highScoreForUnlock=0;
			PlayerPrefs.SetInt("HighScore",highScoreForUnlock);
		}
		else
		{
			highScoreForUnlock=PlayerPrefs.GetInt("HighScore");
		}
		
		CheckHighScore(highScoreForUnlock);
	}

	public void LoadLevel(string levelToLoad)
	{
		
		SceneManager.LoadScene(levelToLoad);
	}
	
	
	
	
	public void CheckHighScore(int highScoreForUnlock)
	{
		int level1HS=highScoreForUnlock;//bunların hepsinin karşınında o levelin yüksek skoru olmalı yani ---- int level1HS= Databasedeki level1 yüksek skor olmalı level2 level3 diye diğerleride databasedeki ile eşit olmalı
		
		if (level1HS>_level2UnlockScore)
		{
			_level2UnlockScoreText.text="Playing in Andromeda Galaxy earns the player 1.5x points.";
   			_levelButtons[0].GetComponentInChildren<TextMeshPro>().SetText("START");
			_levelButtons[0].interactable=true;
			//butonlara erişip pasif veya aktif yapmak gerek
		}
		
		
	}

	
}
