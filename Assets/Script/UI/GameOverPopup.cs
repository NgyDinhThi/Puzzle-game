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

    private void Start() { gameoverPopup.SetActive(false); }

    private void OnEnable() { GameEvents.GameOver += OnGameOver; }
    private void OnDisable() { GameEvents.GameOver -= OnGameOver; }

    private void OnGameOver(bool newBestScore)
    {
        int currentScore = scoreManager.GetCurrentScore();
        int bestScore = scoreManager.GetBestScore();

        currentScoreText.text = currentScore.ToString();
        bestScoreText.text = bestScore.ToString();

        losePopup.SetActive(!newBestScore);
        newBestScorePopup.SetActive(newBestScore);

        gameoverPopup.SetActive(true);
    }

    // Gắn vào button Try Again
    public void OnTryAgainClicked()
    {
        var resetter = Object.FindFirstObjectByType<AutoResetOnGameOver>(FindObjectsInactive.Include);
        if (resetter != null)
            resetter.ResetNow(true);

        // Ẩn popup và chơi lại
        gameoverPopup.SetActive(false);
    }
}
