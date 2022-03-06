using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //private int _score = 0;
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Image _livesImage;
    [SerializeField]
    private Sprite[] _livesprites;
    [SerializeField]
    private Text _gameOver;
    [SerializeField]
    private Text _restartText;

    private GameManager _gameManager;
    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _gameOver.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if (_gameManager == null)
        {
            Debug.LogError("Game Manager is NULL");
        }
    }
    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }
    public void UpdateLives(int currentLives)
    {
        _livesImage.sprite = _livesprites[currentLives];
        if (currentLives < 0)
        {
            currentLives = 0;
        }
        if (currentLives == 0)
        {
            new WaitForSeconds(2.8f);
            GameOverSequence();
        }
        
    }
    IEnumerator GameOverFlickerRoutine()
    {
        while(true)
        {
            _gameOver.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOver.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }
    void GameOverSequence()
    {
        _gameManager.GameOver();
        _gameOver.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
    }
}

