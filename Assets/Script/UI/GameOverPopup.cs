using UnityEngine;

public class GameOverPopup : MonoBehaviour
{
    public GameObject gameoverPopup;
    public GameObject losePopup;
    public GameObject newBestScorePopup;

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
        gameoverPopup.SetActive(true);
        losePopup.SetActive(false);
        newBestScorePopup.SetActive(true);
    }
}
