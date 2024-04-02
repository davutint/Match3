using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
	[SerializeField]private int _totalScore;
	[SerializeField]private string _playerName;
	[SerializeField]private int _level1Score;
	[SerializeField]private int _level2Score;
	[SerializeField]private int _level3Score;
	[SerializeField]private int _level4Score;
	[SerializeField]private int _level5Score;
	
	private string level1P="Level1HighScore";
	private string level2P="Level2HighScore";
	private string level3P="Level3HighScore";
	private string level4P="Level4HighScore";
	private string level5P="Level5HighScore";
	
	private string totalScore="TotalScore";
	
	private void Start()
	{
		CheckScores();
	}
	public void SendScoresToServer()
	{
		//Her levelin yüksek skorunu ilk oturum olacağı için playerprefsde kontrolü sağlayacağım
		//bu level1 vs bunlar her levelin yüksek skorları olacaklar, her levelde oyuncu daha önce kaç puan aldığını görebilsin diye
		
	}
	
	
	public void CalculateScoreToLevel()
	{
		//burayı ui eklemedem önce yaptım
		//Firebasede level ayarlama varsa total scora göre level verebiliriz yoksa boşver
	}
	
	
	private void CheckScores()
	{
		_level1Score=PlayerPrefs.GetInt(level1P);
		_level2Score=PlayerPrefs.GetInt(level2P);
		_level3Score=PlayerPrefs.GetInt(level3P);
		_level4Score=PlayerPrefs.GetInt(level4P);
		_level5Score=PlayerPrefs.GetInt(level5P);
	}
	
	private void GetHighScoresFromServer()
	{
		//firebaseden bu levellerin yüksek skorlarını almalı ve bunları playerprefs olarak tutmalıyım, level sahnelerinde anlık kontrolü böyle sağlarım,
		//levele ait yüksek skore kontrolü fonksiyonuna bir fonksiyon daha eklenmeli ve o anki yüksek skore o anda firebase yollanmalı
		
		//local değişkenlere serverdeki skoreları atayıp bu local değerleri playerprefse atmalısın
		//yukarda ihtiyacın olan string keyler var
		
	}
}
