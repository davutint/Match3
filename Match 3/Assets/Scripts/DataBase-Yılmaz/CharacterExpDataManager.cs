using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterExpDataManager : MonoBehaviour
{
	public LevelConfigSO levelConfigSO;
	private int level;
	private int experience;
	private int requiredExperience;
	
	
	public void IncreaseExp()
	{
		//bunu event şeklinde boarddan çağırıp exp arttıracağım
	}
	
	public void LevelUp()
	{
		level++; //BURADA DA ++ OLAN LEVELİ FİREBASE E YOLLAMAK GEREKİYOR
		CalculateRequiredExp();
	}
	
	public void SetFireBaseLevelUp(int level)
	{
		//SEN SADECE BURADA FİREBASE E LEVEL KISMINI YÜKLEMEMİ SAĞLA, BUNU LEVEL UP KISMINDA ÇAĞIRICAM
	}
	
	public void CalculateRequiredExp()//KARAKTER LEVELİNİ BURADA ÇEKMEM GEREKİYOR GEREKLİ XP DEĞERİNİ BUNA GÖRE AYARLIYORUM
	{
		requiredExperience=levelConfigSO.GetRequiredExp(level);
		
	}
}
