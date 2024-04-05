using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
	public TMP_Text timeText;
	public TMP_Text scoreText;

	public TMP_Text winScore;
	public TMP_Text winText;
	public GameObject winStars1, winStars2, winStars3;

	public GameObject roundOverScreen;

	private Board theBoard;

	public string LevelSelect;

	public GameObject pauseScreen;

	[Range(0.1f,2f)]
	[SerializeField]private float _skyboxRotateSpeed;
	private void Awake()
	{
		theBoard = FindObjectOfType<Board>();
	}

	// Start is called before the first frame update
	void Start()
	{
		winStars1.SetActive(false);
		winStars2.SetActive(false);
		winStars3.SetActive(false);
	}

	// Update is called once per frame
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
		if(!pauseScreen.activeInHierarchy)
		{
			pauseScreen.SetActive(true);
			Time.timeScale = 0f;
		} else
		{
			pauseScreen.SetActive(false);
			Time.timeScale = 1f;
		}
	}

   public void ShuffleBoard()
	{
		theBoard.ShuffleBoard();
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
