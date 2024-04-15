using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoundManager : MonoBehaviour
{
	public float RoundTime = 60f;
	

	private bool _endingRound = false;

	private Board _board;

	public int CurrentScore;
	public float DisplayScore;
	public float ScoreSpeed;

	int _scene;
	public int ScoreTarget;
	[SerializeField]private UIManager _uiMan;
	
	[SerializeField]private CharacterExpDataManager _characterExpDataManager;
	void Awake()
	{
		
		_board = FindObjectOfType<Board>();
	}

	private void Start()
	{
		
		_uiMan.GoalScore.SetText("Score Goal: "+ScoreTarget);
		_scene=SceneManager.GetActiveScene().buildIndex;
		FirebaseManager.Instance.StartLevel(_scene);
	}
	
	void Update()
	{
		if(RoundTime > 0)
		{
			RoundTime -= Time.deltaTime;

			if(RoundTime <= 0)
			{
				RoundTime = 0;

				_endingRound = true;
			}
		}

		if(_endingRound && _board.CurrentState == BoardState.move)
		{
			WinCheck();
			_endingRound = false;
		}

		_uiMan.timeText.text = RoundTime.ToString("0.0") + "s";

		//DisplayScore = Mathf.Lerp(DisplayScore, CurrentScore, ScoreSpeed * Time.deltaTime);//animasyonlu şekilde skoru arttırıyoruz
		_uiMan.UiScoreText.SetText(CurrentScore.ToString());//decimal şekilde artmasını engellemek için bunu kullandım
	}
	
	

	private void WinCheck()//burada yıldız mevzusunu atıp level unlocka refactor edeceğim
	{
		FirebaseManager.Instance.GetPlayerLevelHighScores(ChechHighScore);// firebase kullanarak highscore kontrolü
		_characterExpDataManager.SetFireBaseLevelUpAndExperience();//karaktere xp veriyoruz ve leveli duruma göre arttırıyoruz
		
		
		
		_uiMan.RoundOverScreen.SetActive(true);

		
		if(CurrentScore >= ScoreTarget)
		{
			_uiMan.EndRounWinText.text = "Congratulations!You have completed the section! ";
			_uiMan.EndScore.SetText("Your Point:  "+CurrentScore.ToString());
			FirebaseManager.Instance.CompleteLevel(_scene); //analyticse level kazanıldığını yolluyoruz
			_uiMan.Star.SetActive(true);
		
			
		}
		else
		{
			_uiMan.EndScore.SetText("Your Point:  "+CurrentScore.ToString());
			_uiMan.EndRounWinText.text = "Oh no! You did not reach enough scores! Try again?";
			FirebaseManager.Instance.FailedLevel(_scene);
		}

		SFXManager.Instance.PlayRoundOver();
	}
	
	
	public void ChechHighScore(int lvl_1,int lvl_2,int lvl_3,int lvl_4,int point)// level unlock için yüksek skoreları kaydediyoruz, ayrıca ilerde de lazım olabilir
	{
		
		int level1HS=lvl_1;
		int level2HS=lvl_2;
		int level3HS=lvl_3;
		int level4HS=lvl_4;
	
		int point_=point;
		if (CurrentScore>point_)
		{
			FirebaseManager.Instance.SetPlayerData("point",CurrentScore); //genel yüksek skoru belirledik;
		}
		Debug.Log(level1HS+" "+level2HS+"  Level1 ve level2 yüksekskoru yazdırdım");
		switch (_scene)//3,4,5,6
		{
			case 3:
			FirebaseManager.Instance.StartLevel(_scene);
			if (CurrentScore>level1HS)//eski yüksek skor şu anki skordan küçükmü diye kontrol ediyoruz küçükse yeni skoru firebase e gönderiyoruz
			{
				FirebaseManager.Instance.SetPlayerData("lvl_1",CurrentScore);//yeni yüksek skor
			}
			break;
			case 4:
			FirebaseManager.Instance.StartLevel(_scene);
			if (CurrentScore>level2HS)//eski yüksek skor şu anki skordan küçükmü diye kontrol ediyoruz küçükse yeni skoru firebase e gönderiyoruz
			{
				FirebaseManager.Instance.SetPlayerData("lvl_2",CurrentScore);//yeni yüksek skor
			}
			
			break;
			case 5:
			FirebaseManager.Instance.StartLevel(_scene);
			if (CurrentScore>level3HS)//eski yüksek skor şu anki skordan küçükmü diye kontrol ediyoruz küçükse yeni skoru firebase e gönderiyoruz
			{
				FirebaseManager.Instance.SetPlayerData("lvl_3",CurrentScore);//yeni yüksek skor
			}
			break;
			case 6:
			FirebaseManager.Instance.StartLevel(_scene);
			if (CurrentScore>level4HS)//eski yüksek skor şu anki skordan küçükmü diye kontrol ediyoruz küçükse yeni skoru firebase e gönderiyoruz
			{
				FirebaseManager.Instance.SetPlayerData("lvl_4",CurrentScore);//yeni yüksek skor
			}
			break;
			default:
			break;
		}
	}
}
