using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectButton : MonoBehaviour
{
	

	//public GameObject star1, star2, star3;
	[SerializeField]private int _level2UnlockScore;
	[SerializeField]private int _level3UnlockScore;
	[SerializeField]private int _level4UnlockScore;
	[SerializeField]private int _level5UnlockScore;
	
	[SerializeField]private GameObject[] _unlockImages;
	[SerializeField]private Button[] _levelButtons;
	
	[SerializeField]private int _level1HS;//bunlar test için
	[SerializeField]private int _level2HS;
	[SerializeField]private int _level3HS;
	[SerializeField]private int _level4HS;
	

	// Start is called before the first frame update
	
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
		//Startta leveli açmaya skorun yetip yetmediğinin kontrolü sağlanmalı
		//ancak level1 highscore sadece level2'yi açar ve level2'nin highscoru sadece level3 ü açmaya yarar, eğer level2'yi oynamazsan ne kadar yüksek skor yaparsan yap level3'ü açamazsın
		int level1HS=_level1HS;//bunların hepsinin karşınında o levelin yüksek skoru olmalı yani ---- int level1HS= Databasedeki level1 yüksek skor olmalı level2 level3 diye diğerleride databasedeki ile eşit olmalı
		int level2HS=_level2HS;
		int level3HS=_level3HS;
		int level4HS=_level4HS;
		
		if (level1HS>_level2UnlockScore)
		{
			_unlockImages[0].SetActive(false);
			_levelButtons[0].interactable=true;
			//butonlara erişip pasif veya aktif yapmak gerek
		}
		
		if (level2HS>_level3UnlockScore)
		{
			_unlockImages[1].SetActive(false);
			_levelButtons[1].interactable=true;
		}
		
		if (level3HS>_level4UnlockScore)
		{
			_unlockImages[2].SetActive(false);
			_levelButtons[2].interactable=true;
		}
		
		if (level4HS>_level5UnlockScore)
		{
			_unlockImages[3].SetActive(false);
			_levelButtons[3].interactable=true;
		}
		
		
	}
	

	
}
