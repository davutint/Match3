using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoundManager : MonoBehaviour
{
    public float roundTime = 60f;
    private UIManager _uiMan;

    private bool _endingRound = false;

    private Board _board;

    public int currentScore;
    public float displayScore;
    public float scoreSpeed;

    public int scoreTarget1, scoreTarget2, scoreTarget3;

    
    void Awake()
    {
        _uiMan = FindObjectOfType<UIManager>();
        _board = FindObjectOfType<Board>();
    }

    
    void Update()
    {
        if(roundTime > 0)
        {
            roundTime -= Time.deltaTime;

            if(roundTime <= 0)
            {
                roundTime = 0;

                _endingRound = true;
            }
        }

        if(_endingRound && _board.currentState == BoardState.move)
        {
            WinCheck();
            _endingRound = false;
        }

        _uiMan.timeText.text = roundTime.ToString("0.0") + "s";

        displayScore = Mathf.Lerp(displayScore, currentScore, scoreSpeed * Time.deltaTime);//animasyonlu şekilde skoru arttırıyoruz
        _uiMan.scoreText.text = displayScore.ToString("0");//decimal şekilde artmasını engellemek için bunu kullandım
    }

    private void WinCheck()//burada yıldız mevzusunu atıp level unlocka refactor edeceğim
    {
        _uiMan.roundOverScreen.SetActive(true);

        _uiMan.winScore.text = currentScore.ToString();

        if(currentScore >= scoreTarget3)
        {
            _uiMan.winText.text = "Congratulations! You earned 3 stars!";
            _uiMan.winStars3.SetActive(true);

            PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_Star1", 1);
            PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_Star2", 1);
            PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_Star3", 1);

        } else if (currentScore >= scoreTarget2)
        {
            _uiMan.winText.text = "Congratulations! You earned 2 stars!";
            _uiMan.winStars2.SetActive(true);

            PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_Star1", 1);
            PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_Star2", 1);

        } else if (currentScore >= scoreTarget1)
        {
            _uiMan.winText.text = "Congratulations! You earned 1 star!";
            _uiMan.winStars1.SetActive(true);

            PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_Star1", 1);

        } else
        {
            _uiMan.winText.text = "Oh no! No stars for you! Try again?";
        }

        //SFXManager.instance.PlayRoundOver();
    }
}
