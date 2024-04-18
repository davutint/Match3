using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
	
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
}
