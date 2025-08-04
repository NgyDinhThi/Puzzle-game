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
        Debug.Log("Điểm tốt nhất = " + bestScore_.score);

    }   
    private void Start()
    {
        currentScores_ = 0;
        newBestScore = false;
        UpdateScores();
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
        if (currentScores_ > bestScore_.score)
        {
            newBestScore = true;
            bestScore_.score = currentScores_;
            
        }
        UpdateScores(); 
    }   
    
    private void UpdateScores()
    { 
        scoreText.text = currentScores_.ToString();
    }
    
    public void SaveBestScore(bool newBestScore)
    {
        BinaryDataStream.Save<BestScoreData>(bestScore_, bestScoreKey_);
    }    
}
