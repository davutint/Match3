using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Runtime.CompilerServices;

public class UIManager : MonoBehaviour
{
	[Header("ELEMENTS")]

	private const string ArkaPlanSes = "Sound";
	[SerializeField]private Sprite _openSoundSprite;
	[SerializeField]private Sprite _closeSoundSprite;
	[SerializeField]private Button _soundButton;
	
	int sound;
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
	bool isAdsActive=true;
	[SerializeField] GameObject shuffleAdImage;
	private void Awake()
	{
		_theBoard = FindObjectOfType<Board>();
		
	}

	private void Start()
	{
		if (PlayerPrefs.GetInt("isAdsActive",1)==0)
		{
			isAdsActive=false;
			shuffleAdImage.SetActive(false);
		}
		GetData();
		
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
		if (isAdsActive)
		{
			AdManager.instance.OdulluGoster();
		}
		else
		{
			_theBoard.ShuffleBoard();
		}
		//burada önce reklamı çağıracağız ve reklam görülüp kapandıktan sonra bu fonksiyonu çağıracağız
		//önce reklam cağır;
		//reklamın içinde suffleboard çağır
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
	public void Sound()
	{
		
		
		if (PlayerPrefs.GetInt("Sound",1)==1)
		{
			PlayerPrefs.SetInt("Sound",0);
			SFXManager.Instance.AnaSesDurdur();
		}
		else
		{
			PlayerPrefs.SetInt("Sound",1);
			SFXManager.Instance.AnaSesCal();
		}
		GetData();
		
	}
	private void GetData()
	{
		
		if (PlayerPrefs.GetInt("Sound",1)==1)
		{
			_soundButton.image.sprite=_openSoundSprite;
			SFXManager.Instance.AnaSesCal();
		}
		else
		{
			_soundButton.image.sprite=_closeSoundSprite;
			SFXManager.Instance.AnaSesDurdur();
		}
		
	}
}
