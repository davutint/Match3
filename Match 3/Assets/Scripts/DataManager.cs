using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
	public static DataManager instance;
	[SerializeField]private int _totalScore;
	[SerializeField]private string _playerName;
	[SerializeField]private int _level1Score;
	[SerializeField]private int _level2Score;
	[SerializeField]private int _level3Score;
	[SerializeField]private int _level4Score;
	[SerializeField]private int _level5Score;
	
	
	private void Awake()
	{
		if (instance==null)
		{
			instance=this;
		}
		else
		{
			Destroy(gameObject);
		}
	}	
	
	public void SendHighScoresToServer(int newHighScore,int level)//string levelde olabilir bilmiyorum firebasede hangi türde tutuluyor
	{
		//Her levelin yüksek skorunu ilk oturum olacağı için playerprefsde kontrolü sağlayacağım
		//bu level1 vs bunlar her levelin yüksek skorları olacaklar, her levelde oyuncu daha önce kaç puan aldığını görebilsin diye
		
	}
	
	public string PlayerName()//eğer oluru varsa aynı şekilde player ismi
	{
		string name="Davut";
		return name;
	}
	
	
	
	public int CheckScoreForLevels(int level)
	{
		//Eğer levele göre yüksek skoru kontrol edebileceğim bir sistem olursa iyi olur olmazsa kafana göre
		//bunu level unlockta kullanırım
		int level1234Score=1;
		return level1234Score;
	}
	
}
