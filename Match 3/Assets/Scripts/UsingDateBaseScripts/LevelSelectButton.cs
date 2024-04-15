using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectButton : MonoBehaviour
{
	[Header("SCORES REQUIRED TO UNLOCK")]
	[SerializeField]private int _level2UnlockScore;
	[SerializeField]private int _level3UnlockScore;
	[SerializeField]private int _level4UnlockScore;
	[SerializeField]private int _level5UnlockScore;
	
	[SerializeField]private GameObject[] _unlockTexts;
	[SerializeField]private Button[] _levelButtons;
	
	void Start()
	{
		CheckTheLevelUnlocks();
	}

	public void LoadLevel(string levelToLoad)
	{
		
		SceneManager.LoadScene(levelToLoad);
	}
	
	
	
	private void CheckTheLevelUnlocks()
	{
		FirebaseManager.Instance.GetPlayerLevelHighScores(CheckHighScore);
	}
	
	public void CheckHighScore(int lvl_1,int lvl_2,int lvl_3,int lvl_4,int point)
	{
		int level1HS=lvl_1;//bunların hepsinin karşınında o levelin yüksek skoru olmalı yani ---- int level1HS= Databasedeki level1 yüksek skor olmalı level2 level3 diye diğerleride databasedeki ile eşit olmalı
		int level2HS=lvl_2;
		int level3HS=lvl_3;
		int level4HS=lvl_4;
		int _point=point;//kullanmayacağız
		 //bunu burada kullanmayacağız, ilerde ayrı leaderboardlarda belki kullanırız diye tutalım
		if (level1HS>_level2UnlockScore)
		{
			_unlockTexts[0].SetActive(false);
			_levelButtons[0].interactable=true;
			//butonlara erişip pasif veya aktif yapmak gerek
		}
		
		if (level2HS>_level3UnlockScore)
		{
			_unlockTexts[1].SetActive(false);
			_levelButtons[1].interactable=true;
		}
		
		if (level3HS>_level4UnlockScore)
		{
			_unlockTexts[2].SetActive(false);
			_levelButtons[2].interactable=true;
		}
		
		if (level4HS>_level5UnlockScore)
		{
			_unlockTexts[3].SetActive(false);
			_levelButtons[3].interactable=true;
		}
	}

	
}
