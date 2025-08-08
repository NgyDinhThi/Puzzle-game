using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

[System.Serializable]
public class BestScoreData
{
    public int score = 0;
}

public class Score : MonoBehaviour
{
    public SquareTextureData squareTextureData;
    public TMP_Text scoreText;
    private bool newBestScore =false;
    private BestScoreData bestScore_ = new BestScoreData();

    private int currentScores_;
    private string bestScoreKey_ = "bskdata";

   

    private void Awake()
    {
        if (BinaryDataStream.Exist(bestScoreKey_))
        {
            StartCoroutine(ReadDataFile());
        }
    }

    private IEnumerator ReadDataFile()
    {
        bestScore_ = BinaryDataStream.Read<BestScoreData>(bestScoreKey_);
        yield return new WaitForEndOfFrame();
       
        GameEvents.UpdateBestScoreBar(currentScores_, bestScore_.score);
    }   
    private void Start()
    {
        currentScores_ = 0;
        newBestScore = false;
        UpdateScores();
        squareTextureData.SetStartColors();
        GameEvents.UpdateSquareColor?.Invoke(squareTextureData.currentColors);
    }

    private void OnEnable()
    {
        GameEvents.AddScores += AddScores;
        GameEvents.GameOver += SaveBestScore;

    }

    private void OnDisable()
    {
        GameEvents.AddScores -= AddScores;
        GameEvents.GameOver -= SaveBestScore;
        
    }

    private void AddScores(int scores)
    {
        currentScores_ += scores;

        // đổi while để bắt kịp nhiều mốc 100, 200, 300...
        while (currentScores_ >= squareTextureData.tresholdVal)
        {
            squareTextureData.UpdateColors(currentScores_);
            GameEvents.UpdateSquareColor?.Invoke(squareTextureData.currentColors);
        }

        if (currentScores_ > bestScore_.score)
        {
            newBestScore = true;
            bestScore_.score = currentScores_;
            SaveBestScore(true);
        }

        GameEvents.UpdateBestScoreBar(currentScores_, bestScore_.score);
        UpdateScores();
    }


    private void UpdateSquareColor()
    {
        if (GameEvents.UpdateSquareColor != null &&  currentScores_ >= squareTextureData.tresholdVal)
        {
            squareTextureData.UpdateColors(currentScores_);
            GameEvents.UpdateSquareColor(squareTextureData.currentColors);
        }

        GameEvents.UpdateSquareColor?.Invoke(squareTextureData.currentColors);
    }    
    
    private void UpdateScores()
    { 
        scoreText.text = currentScores_.ToString();
    }
    
    public void SaveBestScore(bool newBestScore)
    {
        BinaryDataStream.Save<BestScoreData>(bestScore_, bestScoreKey_);
    }

    public int GetCurrentScore()
    {
        return currentScores_;
    }

    public int GetBestScore()
    {
        return bestScore_.score;
    }

    public void RestoreCurrentScore(int value)
    {
        currentScores_ = Mathf.Max(0, value);

        squareTextureData.RecalculateByScore(currentScores_);
        GameEvents.UpdateSquareColor?.Invoke(squareTextureData.currentColors);

        UpdateScores();
        GameEvents.UpdateBestScoreBar?.Invoke(currentScores_, bestScore_.score);
    }

}
