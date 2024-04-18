using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIAuth : MonoBehaviour
{
	public LevelConfigSO levelConfigSO;

	[Header("Set Name Settings")]
	public GameObject NamePanel;
	public TMP_InputField NameInput;
	public TextMeshProUGUI NameWarningText;
	public Button SetNameButton;

	public TextMeshProUGUI NameText;
	public TextMeshProUGUI LevelText;

	[Header("Login------------")]
	public GameObject LoginPanel;

	[Header("Game------------")]
	public GameObject GamePanel;

	[Header("Email Login------------")]
	public GameObject EmailLoginPanel;
	public TMP_InputField EmailLoginInput;
	public TMP_InputField PasswordLoginInput;

	[Header("Email SingUp------------")]
	public GameObject EmailSingUpPanel;
	public TMP_InputField EmailSingUpInput;
	public TMP_InputField PasswordSingUpInput;

	
	public Slider Slider;
	public TextMeshProUGUI SliderBarText;
	
	private int _requiredExperience;
	[SerializeField] private FirebaseAuthentication _firebaseAuthentication;
	
	
	private void Start()
	{
		NameInput.onValueChanged.AddListener(ChanegeUserNameListener);
		
	}
	public void ChanegeUserNameListener(string text)
	{
		if (text.Trim().Length > 2 && text.Trim().Length < 20)
		{
			_firebaseAuthentication.CheckUserName(text.Trim(), SetNameButton);
		}
		else
		{
			NameWarningText.text = "Kullanici Adi 3-20 Haneli Olmali";
			NameWarningText.color = Color.red;
			SetNameButton.interactable = false;
		}
	}
	public void SignOut()
	{
		_firebaseAuthentication.SingOut();
	}

	public void OpenPanel(GameObject panel)
	{
		ClosePanel();
		panel.SetActive(true);
	}

	private void ClosePanel()
	{
		NamePanel.SetActive(false);
		LoginPanel.SetActive(false);
		GamePanel.SetActive(false);
		EmailLoginPanel.SetActive(false);   
		EmailSingUpPanel.SetActive(false);
	}
	
	public void GetLevelDataStart(int currentLevel,int currentExp)//burada gereken puan tamamlanınca çağırılacak kod var. OYUNCU BAŞARILI OLURSA ÇALIŞACAK.
	{
		LevelText.text=currentLevel.ToString();
		Slider.maxValue=RequiredExperience(currentLevel);
		Slider.value=currentExp;
		SliderBarText.text=currentExp+"/"+Slider.maxValue.ToString();
	}
	
	public int RequiredExperience(int level)//Level up kısmını bunu kullanarak kontrol et
	{
		_requiredExperience=levelConfigSO.GetRequiredExp(level);
		Debug.Log("Gereken xp  "+_requiredExperience);
		return _requiredExperience;
		
	}
	
	public void ChangeStateGameMenu()
	{
		
	}
	
}
