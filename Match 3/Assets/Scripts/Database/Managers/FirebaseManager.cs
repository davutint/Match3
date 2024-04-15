using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Linq;
using UnityEngine;

public class FirebaseManager : MonoBehaviour
{
	public static FirebaseManager Instance;
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

	public string Api= "11868008013-3hvreepdlp8u3uhnkv7t6v27a0o4dpcj.apps.googleusercontent.com"; //değiştir
	public FirebaseAuth Auth;
	public FirebaseUser User;

	private void Start()
	{
		CheckFirebase();
	}

	private void CheckFirebase()
	{
		FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
		{
			DependencyStatus status = task.Result;
			if (status == DependencyStatus.Available)
			{
				Auth = FirebaseAuth.DefaultInstance;
				User = Auth.CurrentUser;
				Debug.Log(User);
			}
			else
			{
				Debug.Log("Baglanti Basarisiz");
			}          
		});
	}
	public void SetPlayerData(string key,int value)
	{
		DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference.Child("Userss").Child(User.UserId);
		reference.Child(key).SetValueAsync(value);
	}
	public void PlayerData(Action<string,int,int,int,int,int> onSendData)
	{
		FirebaseDatabase.DefaultInstance.GetReference("Userss").OrderByKey().EqualTo(User.UserId).GetValueAsync().ContinueWithOnMainThread(task =>
		{
			if (task.IsFaulted || task.IsCanceled)
			{
				Debug.Log("Vericekme Hatasi");
				return;
			}
			DataSnapshot snapshot = task.Result;
			foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
			{	
				string playerName=childSnapshot.Child("username").Value.ToString();
				int level = int.Parse(childSnapshot.Child("level").Value.ToString());
				int lvl_1 = int.Parse(childSnapshot.Child("lvl_1").Value.ToString());
				int lvl_2 = int.Parse(childSnapshot.Child("lvl_2").Value.ToString());
				int lvl_3 = int.Parse(childSnapshot.Child("lvl_3").Value.ToString());
				int lvl_4 = int.Parse(childSnapshot.Child("lvl_4").Value.ToString());
				onSendData?.Invoke(playerName,level,lvl_1,lvl_2,lvl_3,lvl_4);
			}
		});
	}
	
	
	
	
	
	
	public void GetPlayerLevelHighScores(Action<int,int,int,int,int> onSendHighScoresData)
	{
		FirebaseDatabase.DefaultInstance.GetReference("Userss").OrderByKey().EqualTo(User.UserId).GetValueAsync().ContinueWithOnMainThread(task =>
		{
			if (task.IsFaulted || task.IsCanceled)
			{
				Debug.Log("Vericekme Hatasi");
				return;
			}
			DataSnapshot snapshot = task.Result;
			foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
			{	
				
				int lvl_1 = int.Parse(childSnapshot.Child("lvl_1").Value.ToString());
				int lvl_2 = int.Parse(childSnapshot.Child("lvl_2").Value.ToString());
				int lvl_3 = int.Parse(childSnapshot.Child("lvl_3").Value.ToString());
				int lvl_4 = int.Parse(childSnapshot.Child("lvl_4").Value.ToString());
				int point=int.Parse(childSnapshot.Child("point").Value.ToString());
				onSendHighScoresData?.Invoke(lvl_1,lvl_2,lvl_3,lvl_4,point);
			}
		});
	}
	
	public void GetPlayerLevelData(Action<int,int> onSendLevelDatas)
	{
		FirebaseDatabase.DefaultInstance.GetReference("Userss").OrderByKey().EqualTo(User.UserId).GetValueAsync().ContinueWithOnMainThread(task =>
		{
			if (task.IsFaulted || task.IsCanceled)
			{
				Debug.Log("Vericekme Hatasi");
				return;
			}
			DataSnapshot snapshot = task.Result;
			foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
			{	
				
				int level = int.Parse(childSnapshot.Child("level").Value.ToString());
				int currentExp = int.Parse(childSnapshot.Child("currentxp").Value.ToString());
				
				onSendLevelDatas?.Invoke(level,currentExp);
			}
		});
	}
	
	

	public void GetData(string playername,int level,int lvl_1,int lvl_2,int lvl_3,int lvl_4)
	{
		Debug.Log(level + " " + lvl_1 + " " + lvl_2 + " " + lvl_3 + " " + lvl_4);
	}

  
}
