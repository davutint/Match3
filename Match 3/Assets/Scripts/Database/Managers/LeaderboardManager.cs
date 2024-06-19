/*using System;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{
	public static LeaderboardManager Instance;
	private void Awake()
	{
		DontDestroyOnLoad(this);
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}

	LeaderboardProvider _provider = null;
	public enum LeaderboardProviderTypes {Firebase };
	public LeaderboardProviderTypes LeaderboardProviderType= LeaderboardProviderTypes.Firebase;
	private void Start()
	{
		switch (LeaderboardProviderType)
		{
			case LeaderboardProviderTypes.Firebase:
				_provider=GetComponent<LeaderboardFirebaseProvider>();
				break;
		}
		Debug.Log("Leaderboard provider : " + _provider.GetVendor());
	}

	public void GetLeaderboardData(Action<List<LeaderboardProvider.LeaderboardItem>> onComplete)
	{
		_provider?.GetData(onComplete);       
	}
	public void SubmitScore(string userName=null ,int score=0)
	{
		_provider?.SetScore(userName,score);
		
	}
}*/
