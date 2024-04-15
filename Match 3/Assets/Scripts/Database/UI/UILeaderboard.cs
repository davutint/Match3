using System.Collections;

using TMPro;
using UnityEngine;

public class UILeaderboard : MonoBehaviour
{
	public Transform LeaderboardRoot;
	public GameObject LeaderboardItem;
	[SerializeField]GameObject _leaderboardPanel;
	[SerializeField]UIAuth _uIAuth;
	
	private void Start()
	{
		FillLeaderboard();
	}

	public void SendRandomScores()
	{
		string username=_uIAuth.NameText.ToString();
		LeaderboardManager.Instance.SubmitScore(username,15); 
	}

	
	public void FillLeaderboard()
	{
		Debug.Log("yoketti");
		for (int i = 0;i< LeaderboardRoot.childCount; i++)
		{
			Destroy(LeaderboardRoot.GetChild(i).gameObject);
		}
	   
	   Debug.Log("girdi");
		LeaderboardManager.Instance.GetLeaderboardData((data) =>
		{
			GameObject item;
			for (int i = 0; i<data.Count; i++)
			{
				item = Instantiate(LeaderboardItem, LeaderboardRoot);
				item.transform.Find("TxtUserName").GetComponent<TextMeshProUGUI>().text = data[i].UserName.ToString();
				item.transform.Find("TxtScore").GetComponent<TextMeshProUGUI>().text = data[i].Score.ToString();//.PadLeft(6,'0');
			}
		});      
	}
	public void OpenLeaderboard()
	{
		_leaderboardPanel.SetActive(true);
		FillLeaderboard();
	}
	
	public void CloseLeaderboard()
	{
		_leaderboardPanel.SetActive(false);
	}
	
}
