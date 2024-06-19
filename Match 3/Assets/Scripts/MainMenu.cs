using UnityEngine;
using UnityEngine.SceneManagement;



public class MainMenu : MonoBehaviour
{
	[SerializeField]GameObject settingsPanel;
	[SerializeField]Settings settings;
	[SerializeField]Vector3 targetPosForOpen;
	[SerializeField]Vector3 targetPosForClose;
	[SerializeField]float tweenTime;
	[SerializeField]LeanTweenType tweenType;
	
	[Range(0,2)]
	[SerializeField]private float _skyboxRotateSpeed;
	public void StartGame(string levelToLoad)
	{
		SceneManager.LoadScene(levelToLoad);
	}
	
	private void Update()
	{
		RenderSettings.skybox.SetFloat("_Rotation",Time.time*_skyboxRotateSpeed);
	}

	public void QuitGame()
	{
		Application.Quit();
	}
	
	public void SettingOpen()
	{
		
		settingsPanel.LeanMoveLocal(targetPosForOpen,tweenTime).setEase(tweenType);
	
	}
	public void SettingClosed()
	{
		settingsPanel.LeanMoveLocal(targetPosForClose,tweenTime).setEase(tweenType);
	
	}
	
	
}
