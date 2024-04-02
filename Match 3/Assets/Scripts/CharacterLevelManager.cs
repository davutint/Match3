using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterLevelManager : MonoBehaviour
{
	[SerializeField]private TextMeshProUGUI _levelText;
	[SerializeField]private TextMeshProUGUI _nameText;
	[SerializeField]private Slider _slider;
	[SerializeField]private TextMeshProUGUI _sliderText;
	
	
	private int currentXp;
	private int needenXp;
	private void Start()
	{
		SetCharacterLevel();
	}
   public void SetCharacterLevel()
   {
		_sliderText.SetText(currentXp+"/"+needenXp);
   }
   
   public void LevelUp()
   {
		//gerekli olan xp verisine erişip aşağıdaki gibi vermek gerekiyor
		SetCharacterLevel();
		
   }
   
   public void SetNeededXp()
   {
		needenXp=currentXp*5;
   }
   
   public void GainXP(int scoreValue)
   {
		currentXp+=scoreValue/100;
		//sldier artmalı
   }
   
   
}
