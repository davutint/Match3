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
		level++;
		CalculateRequiredExp();
	}
	
	public void CalculateRequiredExp()//leveli int olarak firebasede tutup çekmek gerekiyor bu sayede minimum düzeyde efor harcamış oluruz
	{
		requiredExperience=levelConfigSO.GetRequiredExp(level);
		
	}
}
