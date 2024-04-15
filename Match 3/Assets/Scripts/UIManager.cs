using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	public TMP_Text timeText;
	public TextMeshProUGUI UiScoreText;
	public TextMeshProUGUI GoalScore;

	public TextMeshProUGUI GainedExpText;
	public TextMeshProUGUI EndScore;


	public TMP_Text EndRounWinText;
	
	public GameObject Star;
	public GameObject RoundOverScreen;

	private Board _theBoard;

	public GameObject PauseScreen;
	public Slider Slider;
	public TextMeshProUGUI SliderBarText;
	public TextMeshProUGUI EndRoundLevelText;

	[Range(0.1f,2f)]
	[SerializeField]private float _skyboxRotateSpeed;
	private void Awake()
	{
		_theBoard = FindObjectOfType<Board>();
	}
	
	void Update()
	{
		RenderSettings.skybox.SetFloat("_Rotation",Time.time*_skyboxRotateSpeed);

		if(Input.GetKeyDown(KeyCode.Escape))
		{
			PauseUnpause();
		}
	}

	public void PauseUnpause()
	{
		if(!PauseScreen.activeInHierarchy)
		{
			PauseScreen.SetActive(true);
			Time.timeScale = 0f;
		} else
		{
			PauseScreen.SetActive(false);
			Time.timeScale = 1f;
		}
	}

   public void ShuffleBoard()
	{
		_theBoard.ShuffleBoard();
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	
	public void LoadLevel(string level)
	{	
		Time.timeScale = 1f;
		SceneManager.LoadScene(level);
	}

	public void TryAgain()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
