using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Score : MonoBehaviour
{
    public TMP_Text scoreText;

    private int currentScores_;
    private void Start()
    {
        currentScores_ = 0;
        UpdateScores();
    }

    private void OnEnable()
    {
        GameEvents.AddScores += AddScores;

    }

    private void OnDisable()
    {
        GameEvents.AddScores -= AddScores;
        
    }

    private void AddScores(int scores)
    {
        currentScores_ += scores;
        UpdateScores(); 
    }   
    
    private void UpdateScores()
    { 
        scoreText.text = currentScores_.ToString();
    }    
}
