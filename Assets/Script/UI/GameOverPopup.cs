using TMPro;
using UnityEngine;

public class GameOverPopup : MonoBehaviour
{
    public GameObject gameoverPopup;
    public GameObject losePopup;
    public GameObject newBestScorePopup;
    public TMP_Text currentScoreText;
    public TMP_Text bestScoreText;
    public Score scoreManager;

    private void Start()
    {
        gameoverPopup.SetActive(false);
    }

    private void OnEnable()
    {
        GameEvents.GameOver += OnGameOver;
    }
    private void OnDisable()
    {
        GameEvents.GameOver -= OnGameOver;
        
    }
    private void OnGameOver(bool newBestScore)
    {
        int currentScore = scoreManager.GetCurrentScore();
        int bestScore = scoreManager.GetBestScore();

        // Hiển thị lên UI
        currentScoreText.text = currentScore.ToString();
        bestScoreText.text = bestScore.ToString();

        gameoverPopup.SetActive(true);
        Debug.Log("GameOverPopup active: " + gameoverPopup.activeInHierarchy);
        losePopup.SetActive(false);
        newBestScorePopup.SetActive(true);
    }


}
