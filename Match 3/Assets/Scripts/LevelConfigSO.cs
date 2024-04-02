using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="LevelConfig",menuName ="ScriptableObjects/LevelConfig",order =1)]
public class LevelConfigSO : ScriptableObject
{
	[Header("Animation Curve")]
	public AnimationCurve animationCurve;
	public int maxLevel;
	public int maxRequiredExp;
	
	public int GetRequiredExp(int level)
	{
		int requiredExperience=Mathf.RoundToInt(animationCurve.Evaluate(Mathf.InverseLerp(0,maxLevel,level))*maxRequiredExp);
		return requiredExperience;
	}
}
