using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BestScoreBar : MonoBehaviour
{
    public Image fillImage;
    public TMP_Text bestScoreText;

    private void OnEnable()
    {
        GameEvents.UpdateBestScoreBar += UpdateBestScoreBar;
    }
    private void OnDisable()
    {
        GameEvents.UpdateBestScoreBar -= UpdateBestScoreBar; 
        
    }

    private void UpdateBestScoreBar(int currentScore, int bestScore)
    {
        float currentPercent = (float)currentScore /(float) bestScore;

        fillImage.fillAmount = currentPercent;
        bestScoreText.text = bestScore.ToString();
    }    
}
