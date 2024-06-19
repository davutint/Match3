using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CharacterExpDataManager : MonoBehaviour
{
	public LevelConfigSO LevelConfigSO;
	private int _level;
	private int _experience;
	private int _requiredExperience;
	
	[SerializeField]RoundManager _roundManager;
	[SerializeField]UIManager _uiManager;
	private void Awake()
	{
		//FirebaseManager.Instance.GetPlayerLevelData(GetLevelDataStart);
	}
	
	public void SetFireBaseLevelUpAndExperience()// HER BÖLÜM SONUNDA ÇAĞIR
	{
		//FirebaseManager.Instance.GetPlayerLevelData(GetLevelData);

	}
	
	private void GetLevelDataStart(int currentLevel,int currentExp)//burada gereken puan tamamlanınca çağırılacak kod var. OYUNCU BAŞARILI OLURSA ÇALIŞACAK.
	{
		_level=currentLevel;
		_experience=currentExp;
		RequiredExperience(_level);
		Debug.Log("şimdiki level firebaseden çektik  "+_level+" şimdiki exp firebaseden geldi  : "+_experience );
	}
	
	private void GetLevelData(int currentLevel,int currentExp)//burada gereken puan tamamlanınca çağırılacak kod var. OYUNCU BAŞARILI OLURSA ÇALIŞACAK.
	{
		_level=currentLevel;
		
		int newExp=IncreasedExp(currentExp);
		
		if (newExp>=RequiredExperience(_level))//Level Up
		{
			_level++;
			//FirebaseManager.Instance.SetPlayerData("level",_level);
			_uiManager.Slider.maxValue=RequiredExperience(_level);
			_uiManager.EndRoundLevelText.text=_level.ToString();

		}
		
		
	
		//FirebaseManager.Instance.SetPlayerData("currentxp",newExp);// Oyuncuya exp ver
		_uiManager.Slider.maxValue=RequiredExperience(_level);
		_uiManager.Slider.value=newExp;
		_uiManager.SliderBarText.text=newExp+"/"+_uiManager.Slider.maxValue.ToString();//eğer slider düzgün gözükürse anamenü için neededxp kısmınıda çekebiliriz
		_uiManager.EndRoundLevelText.text=_level.ToString();
		_uiManager.GainedExpText.text="Experience Gained: "+(_roundManager.CurrentScore/20).ToString();
	}
	public int IncreasedExp(int newExp)
	{
		newExp+=_roundManager.CurrentScore/10;
		return newExp;
	}
	
	public int RequiredExperience(int level)//Level up kısmını bunu kullanarak kontrol et
	{
		_requiredExperience=LevelConfigSO.GetRequiredExp(level);
		Debug.Log("Gereken xp  "+_requiredExperience);
		return _requiredExperience;
		
	}
}
