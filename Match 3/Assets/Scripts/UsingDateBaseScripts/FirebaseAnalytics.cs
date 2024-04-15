using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FirebaseAnalytics : MonoBehaviour
{
	// Start is called before the first frame update
	
	private float _startTime;

	public static object EventAppOpen { get; private set; }
	public static object ParameterValue { get; private set; }

	
	void Start()
	{
		// Oyuna giriş zamanını kaydet
		_startTime = Time.time;
		
	}

	void OnDestroy()
	{
		// Oyundan çıkış zamanını kaydet ve Firebase Analytics'e gönder
		float duration = Time.time - _startTime;
		LogGameDuration(duration);
		
	}
	
	void LogGameDuration(float duration)
	{
		// Firebase Analytics'e oyun süresini gönder
		FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventAppOpen, 
		FirebaseAnalytics.ParameterValue, duration);
	}

	private static void LogEvent(object eventAppOpen, object parameterValue, float duration)
	{
		throw new NotImplementedException();
	}
	
}

